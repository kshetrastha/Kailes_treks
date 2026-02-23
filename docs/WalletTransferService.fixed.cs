using globalRewardDist.Application.Abstractions.Persistence;
using globalRewardDist.Application.Auth.Models;
using globalRewardDist.Application.Common;
using globalRewardDist.Application.Services;
using globalRewardDist.Application.WalletTransfers;
using globalRewardDist.Application.WalletTransfers.Models;
using globalRewardDist.Domain.Entities.Client;
using globalRewardDist.Domain.Enums;
using globalRewardDist.Infrastructure.Contracts.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace globalRewardDist.Infrastructure.Services;

public sealed class WalletTransferService : IWalletTransferService
{
    private readonly AppDbContext _db;
    private readonly IWalletRepository _walletRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IOtpRepository _otpRepository;
    private readonly IOptionsMonitor<WalletTransferOptions> _options;
    private readonly ILogger<WalletTransferService> _logger;

    public WalletTransferService(
        AppDbContext db,
        IWalletRepository walletRepository,
        IUserReadRepository userReadRepository,
        IOtpRepository otpRepository,
        IOptionsMonitor<WalletTransferOptions> options,
        ILogger<WalletTransferService> logger)
    {
        _db = db;
        _walletRepository = walletRepository;
        _userReadRepository = userReadRepository;
        _otpRepository = otpRepository;
        _options = options;
        _logger = logger;
    }

    public Task<ResponseModel<WalletTransferLimitsDto>> GetLimitsAsync(CancellationToken ct)
    {
        var limits = GetLimits();
        return Task.FromResult(ResponseModel<WalletTransferLimitsDto>.Ok(limits));
    }

    public async Task<ResponseModel> SendOtpAsync(string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ResponseModel.Bad("Email is required.");

        await _otpRepository.SendAsync(email.Trim(), OtpPurpose.Transfer, ct);
        return ResponseModel.Ok(null, "OTP sent successfully.");
    }

    public async Task<ResponseModel<WalletTransferEligibilityDto>> CheckEligibilityAsync(
        WalletTransferEligibilityRequest request,
        CancellationToken ct)
    {
        var evaluation = await EvaluateEligibilityAsync(request, ct);
        if (evaluation.Response.StatusCode != StatusCodes.Status200OK)
            return evaluation.Response;

        var limits = GetLimits();
        return ResponseModel<WalletTransferEligibilityDto>.Ok(new WalletTransferEligibilityDto(
            true,
            "Eligible for transfer.",
            evaluation.Balance,
            limits.MinimumAmount,
            limits.MaximumAmount));
    }

    public Task<ResponseModel<WalletTransferResultDto>> ExecuteTransferAsync(
        WalletTransferRequest request,
        CancellationToken ct)
        => ExecuteTransferInternalAsync(request, isAdminTransfer: false, ct);

    public Task<ResponseModel<WalletTransferResultDto>> ExecuteAdminTransferAsync(
        WalletTransferRequest request,
        CancellationToken ct)
        => ExecuteTransferInternalAsync(request, isAdminTransfer: true, ct);

    private async Task<ResponseModel<WalletTransferResultDto>> ExecuteTransferInternalAsync(
        WalletTransferRequest request,
        bool isAdminTransfer,
        CancellationToken ct)
    {
        var evaluation = await EvaluateEligibilityAsync(new WalletTransferEligibilityRequest(
                request.SenderClientId,
                request.ReceiverUserName,
                request.WalletBucket,
                request.Amount),
            ct);

        if (evaluation.Response.StatusCode != StatusCodes.Status200OK)
            return ResponseModel<WalletTransferResultDto>.Bad(evaluation.Response.Msg, evaluation.Response.StatusCode);

        if (string.IsNullOrWhiteSpace(request.Otp))
            return ResponseModel<WalletTransferResultDto>.Bad("OTP is required.");

        var sender = evaluation.SenderProfile!;
        var receiver = evaluation.ReceiverProfile!;

        var otpResponse = await _otpRepository.VerifyAsync(sender.Email, OtpPurpose.Transfer, request.Otp, ct);
        if (otpResponse.StatusCode != StatusCodes.Status200OK)
            return ResponseModel<WalletTransferResultDto>.Bad(otpResponse.Msg, otpResponse.StatusCode);

        var referencePrefix = isAdminTransfer ? "AWTR" : "WTR";
        var reference = $"{referencePrefix}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        var strategy = _db.Database.CreateExecutionStrategy();
        var responseModel = new ResponseModel<WalletTransferResultDto>();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var debited = await _walletRepository.TryDebitAsync(
                    request.SenderClientId,
                    request.WalletBucket,
                    request.Amount,
                    ct);

                if (!debited)
                {
                    responseModel = ResponseModel<WalletTransferResultDto>.Bad("Insufficient balance for transfer.");
                    await transaction.RollbackAsync(ct);
                    return;
                }

                await _walletRepository.CreditAsync(receiver.Id, request.WalletBucket, request.Amount, ct);

                var senderWallet = await _walletRepository.GetByClientIdAsync(request.SenderClientId, ct);
                var receiverWallet = await _walletRepository.GetByClientIdAsync(receiver.Id, ct);

                if (senderWallet is null || receiverWallet is null)
                {
                    responseModel = ResponseModel<WalletTransferResultDto>.Bad("Wallet not found.");
                    await transaction.RollbackAsync(ct);
                    return;
                }

                var senderBalance = GetWalletBalance(senderWallet, request.WalletBucket);
                var receiverBalance = GetWalletBalance(receiverWallet, request.WalletBucket);

                await _walletRepository.AddTransactionAsync(new GRDHWalletTransaction
                {
                    ClientId = request.SenderClientId,
                    WalletBucket = request.WalletBucket,
                    Amount = -request.Amount,
                    BalanceAfter = senderBalance,
                    Description = $"Transfer to {receiver.ReferralCode} ({receiver.FullName})",
                    Reference = reference
                }, ct);

                await _walletRepository.AddTransactionAsync(new GRDHWalletTransaction
                {
                    ClientId = receiver.Id,
                    WalletBucket = request.WalletBucket,
                    Amount = request.Amount,
                    BalanceAfter = receiverBalance,
                    Description = $"Transfer from {sender.ReferralCode} ({sender.FullName})",
                    Reference = reference
                }, ct);

                if (isAdminTransfer)
                {
                    await _db.AdminWalletTransferHistories.AddAsync(new AdminWalletTransferHistory
                    {
                        Reference = reference,
                        SenderClientId = sender.Id,
                        ReceiverClientId = receiver.Id,
                        SenderUserName = sender.ReferralCode,
                        ReceiverUserName = receiver.ReferralCode,
                        WalletBucket = request.WalletBucket,
                        Amount = request.Amount,
                        SenderBalanceAfter = senderBalance,
                        ReceiverBalanceAfter = receiverBalance
                    }, ct);
                }
                else
                {
                    await _db.WalletTransferHistories.AddAsync(new WalletTransferHistory
                    {
                        Reference = reference,
                        SenderClientId = sender.Id,
                        ReceiverClientId = receiver.Id,
                        SenderUserName = sender.ReferralCode,
                        ReceiverUserName = receiver.ReferralCode,
                        WalletBucket = request.WalletBucket,
                        Amount = request.Amount,
                        SenderBalanceAfter = senderBalance,
                        ReceiverBalanceAfter = receiverBalance
                    }, ct);
                }

                await _walletRepository.SaveAsync(ct);
                await transaction.CommitAsync(ct);

                responseModel = ResponseModel<WalletTransferResultDto>.Ok(new WalletTransferResultDto(
                    reference,
                    senderBalance,
                    receiverBalance,
                    DateTime.Now));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wallet transfer failed for sender {SenderId}", request.SenderClientId);
                await transaction.RollbackAsync(ct);
                responseModel = ResponseModel<WalletTransferResultDto>.Bad("Transfer failed. Please try again later.");
            }
        });

        return responseModel;
    }

    private WalletTransferLimitsDto GetLimits()
    {
        var options = _options.CurrentValue;
        return new WalletTransferLimitsDto(options.MinimumAmount, options.MaximumAmount);
    }

    private async Task<(ResponseModel<WalletTransferEligibilityDto> Response, UserProfileDto? SenderProfile, UserProfileDto? ReceiverProfile, decimal Balance)>
        EvaluateEligibilityAsync(WalletTransferEligibilityRequest request, CancellationToken ct)
    {
        if (request.SenderClientId <= 0)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Sender is required."), null, null, 0m);

        if (string.IsNullOrWhiteSpace(request.ReceiverUserName))
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Receiver username is required."), null, null, 0m);

        if (!Enum.IsDefined(typeof(WalletBucket), request.WalletBucket))
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Invalid wallet type."), null, null, 0m);

        if (request.Amount <= 0)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Transfer amount must be greater than zero."), null, null, 0m);

        var limits = GetLimits();
        if (request.Amount < limits.MinimumAmount || request.Amount > limits.MaximumAmount)
        {
            return (ResponseModel<WalletTransferEligibilityDto>.Bad(
                $"Transfer amount must be between {limits.MinimumAmount} and {limits.MaximumAmount}."), null, null, 0m);
        }

        var senderProfile = await _userReadRepository.GetProfileAsync(request.SenderClientId, ct);
        if (senderProfile is null || !senderProfile.IsActive)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Sender is not active or does not exist."), null, null, 0m);

        var receiverProfile = await _userReadRepository.GetProfileUsingUserNameAsync(request.ReceiverUserName.Trim(), ct);
        if (receiverProfile is null || !receiverProfile.IsActive)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Receiver is not active or does not exist."), null, null, 0m);

        if (receiverProfile.Id == senderProfile.Id)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Sender and receiver must be different users."), null, null, 0m);

        var senderWallet = await _walletRepository.GetByClientIdAsync(request.SenderClientId, ct);
        if (senderWallet is null)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Sender wallet not found."), null, null, 0m);

        var receiverWallet = await _walletRepository.GetByClientIdAsync(receiverProfile.Id, ct);
        if (receiverWallet is null)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Receiver wallet not found."), null, null, 0m);

        var senderBalance = GetWalletBalance(senderWallet, request.WalletBucket);
        if (senderBalance < request.Amount)
            return (ResponseModel<WalletTransferEligibilityDto>.Bad("Insufficient wallet balance."), null, null, senderBalance);

        return (ResponseModel<WalletTransferEligibilityDto>.Ok(new WalletTransferEligibilityDto(
                true,
                "Eligible for transfer.",
                senderBalance,
                limits.MinimumAmount,
                limits.MaximumAmount)),
            senderProfile,
            receiverProfile,
            senderBalance);
    }

    private static decimal GetWalletBalance(GRDHWallet wallet, WalletBucket bucket) => bucket switch
    {
        WalletBucket.Ecosystem => wallet.Ecosystem,
        WalletBucket.MagicHub => wallet.MagicHub,
        WalletBucket.PrimeHub => wallet.PrimeHub,
        WalletBucket.GrowthHub => wallet.GrowthHub,
        WalletBucket.PrimePerformanceFund => wallet.PrimePerformanceFund,
        WalletBucket.GrowthPerformanceFund => wallet.GrowthPerformanceFund,
        WalletBucket.EliteFund => wallet.EliteFund,
        WalletBucket.EliteReferralFund => wallet.EliteReferralFund,
        WalletBucket.DirectWallet => wallet.DirectWallet,
        WalletBucket.CommunityWallet => wallet.CommunityWallet,
        WalletBucket.CompanyWallet => wallet.CompanyWallet,
        _ => 0m
    };
}
