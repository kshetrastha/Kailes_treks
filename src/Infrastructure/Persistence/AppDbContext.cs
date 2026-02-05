using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Infrastructure.Identity;

namespace TravelCleanArch.Infrastructure.Persistence;

public sealed class AppDbContext:
    IdentityDbContext<AppUser, AppRole, int>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(b =>
        {
            b.Property(x => x.FullName).HasMaxLength(200);
        });
    }
}
