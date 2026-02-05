using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TravelCleanArch.Application.Abstractions.Authentication;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.Application.Abstractions.Queries;
using TravelCleanArch.Infrastructure.Authentication;
using TravelCleanArch.Infrastructure.Identity;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Queries;
using TravelCleanArch.Infrastructure.Seeding;

namespace TravelCleanArch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));

        services.AddDbContext<AppDbContext>(opts =>
        {
            var cs = config.GetConnectionString("DefaultConnection");
            opts.UseNpgsql(cs);
        });

        services.AddIdentityCore<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
            .AddRoles<AppRole>()                         // if you use roles
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<AppUser>>() // ✅ THIS is the missing line
            .AddDefaultTokenProviders();    

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUserProfileReadRepository, UserProfileReadRepository>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
