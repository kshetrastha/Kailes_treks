using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        services.AddIdentityCore<AppUser>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireDigit = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = true;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUserProfileReadRepository, UserProfileReadRepository>();

        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
