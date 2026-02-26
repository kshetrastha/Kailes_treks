using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TravelCleanArch.Application.Abstractions.Authentication;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Infrastructure.Identity;

public sealed class IdentityService(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    IJwtTokenGenerator tokenGenerator,
    ILogger<IdentityService> logger) : IIdentityService
{
    private const string RefreshTokenProvider = "TravelCleanArch";
    private const string RefreshTokenName = "RefreshToken";

    public async Task<Result<int>> CreateUserAsync(CreateUserRequest request, CancellationToken ct)
    {
        var role = string.IsNullOrWhiteSpace(request.Role) ? AppRoles.Customer : request.Role;

        if (role is not (AppRoles.Admin or AppRoles.Customer))
            return Result<int>.Failure("role.invalid", "Role must be Admin or Customer.");

        if (!await roleManager.RoleExistsAsync(role))
            return Result<int>.Failure("role.missing", $"Role '{role}' does not exist.");

        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return Result<int>.Failure("user.exists", "A user with this email already exists.");

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var create = await userManager.CreateAsync(user, request.Password);
        if (!create.Succeeded)
            return Result<int>.Failure("user.create_failed", string.Join("; ", create.Errors.Select(e => e.Description)));

        var addRole = await userManager.AddToRoleAsync(user, role);
        if (!addRole.Succeeded)
        {
            logger.LogWarning("User created but role assignment failed: {Errors}", string.Join("; ", addRole.Errors.Select(e => e.Description)));
            return Result<int>.Failure("user.role_failed", string.Join("; ", addRole.Errors.Select(e => e.Description)));
        }

        return Result<int>.Success(user.Id);
    }

    public async Task<Result<AuthTokensResponse>> RegisterAsync(string email, string password, string fullName, CancellationToken ct)
    {
        var createUserResult = await CreateUserAsync(new CreateUserRequest(email, password, fullName, AppRoles.Customer), ct);
        if (!createUserResult.IsSuccess)
            return Result<AuthTokensResponse>.Failure(createUserResult.Error!.Code, createUserResult.Error.Message);

        return await LoginAsync(email, password, ct);
    }

    public async Task<Result<AuthTokensResponse>> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<AuthTokensResponse>.Failure("auth.invalid", "Invalid credentials.");

        var ok = await userManager.CheckPasswordAsync(user, password);
        if (!ok)
            return Result<AuthTokensResponse>.Failure("auth.invalid", "Invalid credentials.");

        return await GenerateTokensAsync(user);
    }

    public async Task<Result<AuthTokensResponse>> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var tokenParts = refreshToken.Split(':', 2);
        if (tokenParts.Length != 2 || !int.TryParse(tokenParts[0], out var userId))
            return Result<AuthTokensResponse>.Failure("auth.refresh_invalid", "Refresh token is invalid.");

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<AuthTokensResponse>.Failure("auth.refresh_invalid", "Refresh token is invalid.");

        var storedToken = await userManager.GetAuthenticationTokenAsync(user, RefreshTokenProvider, RefreshTokenName);
        if (string.IsNullOrWhiteSpace(storedToken) || storedToken != refreshToken)
            return Result<AuthTokensResponse>.Failure("auth.refresh_invalid", "Refresh token is invalid.");

        return await GenerateTokensAsync(user);
    }

    public async Task<Result> ForgotPasswordAsync(string email, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Success();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        logger.LogInformation("Password reset token generated for {Email}. Token: {ResetToken}", email, token);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string email, string resetToken, string newPassword, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure("auth.reset_failed", "Password reset failed.");

        var reset = await userManager.ResetPasswordAsync(user, resetToken, newPassword);
        if (!reset.Succeeded)
            return Result.Failure("auth.reset_failed", string.Join("; ", reset.Errors.Select(e => e.Description)));

        return Result.Success();
    }

    private async Task<Result<AuthTokensResponse>> GenerateTokensAsync(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenGenerator.GenerateToken(user.Id, user.Email!, roles, Array.Empty<System.Security.Claims.Claim>());
        var refreshToken = $"{user.Id}:{Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))}";

        await userManager.SetAuthenticationTokenAsync(user, RefreshTokenProvider, RefreshTokenName, refreshToken);

        return Result<AuthTokensResponse>.Success(new AuthTokensResponse(user.Id, user.Email!, accessToken, refreshToken));
    }
}
