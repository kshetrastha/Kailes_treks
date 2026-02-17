using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TravelCleanArch.Application.Abstractions.Authentication;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Queries;
using TravelCleanArch.Infrastructure.Authentication;
using TravelCleanArch.Infrastructure.Identity;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;
using TravelCleanArch.Infrastructure.Queries;
using TravelCleanArch.Infrastructure.Seeding;
using TravelCleanArch.Infrastructure.Services;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Application.Abstractions.Company;

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

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IInteractiveAuthService, InteractiveAuthService>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUserProfileReadRepository, UserProfileReadRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpeditionService, ExpeditionService>();
        services.AddScoped<IExpeditionModuleService, ExpeditionModuleService>();
        services.AddScoped<IExpeditionTypeService, ExpeditionTypeService>();
        services.AddScoped<IItineraryService, ItineraryService>();
        services.AddScoped<IItineraryDayService, ItineraryDayService>();
        services.AddScoped<ITrekkingService, TrekkingService>();
        services.AddScoped<IWhyWithUsService, WhyWithUsService>();
        services.AddScoped<IWhyWithUsHeroService, WhyWithUsHeroService>();
        services.AddScoped<IWhoWeAreService, WhoWeAreService>();
        services.AddScoped<IWhoWeAreHeroService, WhoWeAreHeroService>();
        services.AddScoped<IAwardService, AwardService>();
        services.AddScoped<IPatronService, PatronService>();
        services.AddScoped<IChairmanMessageService, ChairmanMessageService>();
        services.AddScoped<ITeamMemberService, TeamMemberService>();
        services.AddScoped<ICertificateDocumentService, CertificateDocumentService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
