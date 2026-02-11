using System.Security.Claims;
using TravelCleanArch.Application.Abstractions.Security;

namespace TravelCleanArch.Web.Security;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public int? UserId
    {
        get
        {
            var value = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email => accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyCollection<Claim> Claims =>
        accessor.HttpContext?.User?.Claims?.ToArray() ?? Array.Empty<Claim>();
}
