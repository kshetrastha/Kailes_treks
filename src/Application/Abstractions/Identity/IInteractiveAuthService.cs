using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Abstractions.Identity;

public interface IInteractiveAuthService
{
    Task<Result> RegisterCustomerAsync(string email, string password, string fullName, CancellationToken ct);
    Task<Result> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct);
    Task SignOutAsync(CancellationToken ct);
}
