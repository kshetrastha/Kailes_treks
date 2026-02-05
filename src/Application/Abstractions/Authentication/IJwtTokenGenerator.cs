using System.Security.Claims;

namespace TravelCleanArch.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string email, IList<string> roles, IReadOnlyCollection<Claim> extraClaims);
}
