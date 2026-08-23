using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class AboutUsService(AppDbContext dbContext) : GenericRepository<AboutUsPage>(dbContext), IAboutUsService
{
    public Task<AboutUsPage?> GetPageAsync(bool asNoTracking, bool publishedOnly, CancellationToken ct)
    {
        var query = Query().Include(x => x.Highlights).AsQueryable();
        if (asNoTracking) query = query.AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);

        return query.OrderBy(x => x.Id).FirstOrDefaultAsync(ct);
    }
}
