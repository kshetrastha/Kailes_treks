using TravelCleanArch.Web.Security;
using TravelCleanArch.Application;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Infrastructure;
using TravelCleanArch.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/account/login";
    opts.AccessDeniedPath = "/account/access-denied";
    opts.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole(AppRoles.Admin));
    options.AddPolicy("EditorOrAdmin", p => p.RequireRole(AppRoles.Admin, AppRoles.Editor));
    options.AddPolicy("CustomerOnly", p => p.RequireRole(AppRoles.Customer));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await seeder.MigrateAndSeedAsync(CancellationToken.None);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/home");
    return Task.CompletedTask;
});

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
