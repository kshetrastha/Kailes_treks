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

    public async Task<Result<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<LoginResponse>.Failure("auth.invalid", "Invalid credentials.");

        var ok = await userManager.CheckPasswordAsync(user, password);
        if (!ok)
            return Result<LoginResponse>.Failure("auth.invalid", "Invalid credentials.");

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user.Id, user.Email!, roles, Array.Empty<System.Security.Claims.Claim>());

        return Result<LoginResponse>.Success(new LoginResponse(user.Id, user.Email!, token));
    }
}
