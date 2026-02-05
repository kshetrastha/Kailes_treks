using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Abstractions.Identity;

public sealed record CreateUserRequest(string Email, string Password, string FullName, string Role);

public interface IIdentityService
{
    Task<Result<int>> CreateUserAsync(CreateUserRequest request, CancellationToken ct);
    Task<Result<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct);
}

public sealed record LoginResponse(int UserId, string Email, string AccessToken);
