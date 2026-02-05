using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TravelCleanArch.API.Security;
using TravelCleanArch.API.Swagger;
using TravelCleanArch.Application;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Infrastructure;
using TravelCleanArch.Infrastructure.Authentication;
using TravelCleanArch.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
          ?? throw new InvalidOperationException("Jwt options are not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole(AppRoles.Admin));
    options.AddPolicy("EditorOrAdmin", p => p.RequireRole(AppRoles.Admin, AppRoles.Editor));
    options.AddPolicy("CustomerOnly", p => p.RequireRole(AppRoles.Customer));
});

builder.Services.AddSwaggerWithJwtAndGroups();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await seeder.MigrateAndSeedAsync(CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
    app.UseGroupedSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
