using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Infrastructure.Identity;

public sealed class InteractiveAuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    RoleManager<AppRole> roleManager,
    ILogger<InteractiveAuthService> logger) : IInteractiveAuthService
{
    public async Task<Result> RegisterCustomerAsync(string email, string password, string fullName, CancellationToken ct)
    {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing is not null)
            return Result.Failure("user.exists", "A user with this email already exists.");

        if (!await roleManager.RoleExistsAsync(AppRoles.Customer))
            return Result.Failure("role.missing", "Customer role is not available.");

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        var create = await userManager.CreateAsync(user, password);
        if (!create.Succeeded)
            return Result.Failure("user.create_failed", string.Join("; ", create.Errors.Select(e => e.Description)));

        var addRole = await userManager.AddToRoleAsync(user, AppRoles.Customer);
        if (!addRole.Succeeded)
        {
            logger.LogWarning("User created but customer role assignment failed: {Errors}", string.Join("; ", addRole.Errors.Select(e => e.Description)));
            return Result.Failure("user.role_failed", string.Join("; ", addRole.Errors.Select(e => e.Description)));
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return Result.Success();
    }

    public async Task<Result> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure("auth.invalid", "Invalid credentials.");

        var result = await signInManager.PasswordSignInAsync(user.UserName!, password, rememberMe, lockoutOnFailure: true);
        if (result.Succeeded)
            return Result.Success();

        if (result.IsLockedOut)
            return Result.Failure("auth.locked_out", "The account is temporarily locked.");

        return Result.Failure("auth.invalid", "Invalid credentials.");
    }

    public async Task SignOutAsync(CancellationToken ct)
    {
        await signInManager.SignOutAsync();
    }
}
