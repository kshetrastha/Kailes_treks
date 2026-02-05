using Microsoft.AspNetCore.Identity;

namespace TravelCleanArch.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
}

public sealed class AppRole : IdentityRole<int>
{
}
