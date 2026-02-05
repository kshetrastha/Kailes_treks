using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Infrastructure.Identity;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Seeding;

public sealed class IdentitySeeder(
    AppDbContext db,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ILogger<IdentitySeeder> logger)
{
    public async Task MigrateAndSeedAsync(CancellationToken ct)
    {
        await db.Database.MigrateAsync(ct);

        await EnsureRoleAsync(AppRoles.Admin);
        await EnsureRoleAsync(AppRoles.Editor);
        await EnsureRoleAsync(AppRoles.Viewer);
        await EnsureRoleAsync(AppRoles.Customer);

        await EnsureAdminAsync(ct);
    }

    private async Task EnsureRoleAsync(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return;

        var res = await roleManager.CreateAsync(new AppRole { Name = roleName });
        if (!res.Succeeded)
            throw new InvalidOperationException($"Failed creating role '{roleName}': {string.Join("; ", res.Errors.Select(e => e.Description))}");
    }

    private async Task EnsureAdminAsync(CancellationToken ct)
    {
        const string adminEmail = "admin@local";
        const string adminPassword = "Admin123$"; // change immediately for real environments

        var admin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail, ct);
        if (admin is null)
        {
            admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Default Admin"
            };

            var create = await userManager.CreateAsync(admin, adminPassword);
            if (!create.Succeeded)
                throw new InvalidOperationException($"Failed creating admin: {string.Join("; ", create.Errors.Select(e => e.Description))}");

            logger.LogInformation("Seeded default admin user: {Email}", adminEmail);
        }

        if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
        {
            var add = await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            if (!add.Succeeded)
                throw new InvalidOperationException($"Failed adding admin role: {string.Join("; ", add.Errors.Select(e => e.Description))}");
        }
    }
}
