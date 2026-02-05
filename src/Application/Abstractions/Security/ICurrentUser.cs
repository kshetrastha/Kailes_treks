using System.Security.Claims;

namespace TravelCleanArch.Application.Abstractions.Security;

public interface ICurrentUser
{
    int? UserId { get; }
    string? Email { get; }
    IReadOnlyCollection<Claim> Claims { get; }
}
