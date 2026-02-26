using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Abstractions.Identity;

public sealed record CreateUserRequest(string Email, string Password, string FullName, string Role);

public interface IIdentityService
{
    Task<Result<int>> CreateUserAsync(CreateUserRequest request, CancellationToken ct);
    Task<Result<AuthTokensResponse>> LoginAsync(string email, string password, CancellationToken ct);
    Task<Result<AuthTokensResponse>> RegisterAsync(string email, string password, string fullName, CancellationToken ct);
    Task<Result<AuthTokensResponse>> RefreshAsync(string refreshToken, CancellationToken ct);
    Task<Result> ForgotPasswordAsync(string email, CancellationToken ct);
    Task<Result> ResetPasswordAsync(string email, string resetToken, string newPassword, CancellationToken ct);
}

public sealed record AuthTokensResponse(int UserId, string Email, string AccessToken, string RefreshToken);
