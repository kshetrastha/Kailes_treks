using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class TermsAndConditionService(AppDbContext dbContext) : GenericRepository<TermsAndCondition>(dbContext), ITermsAndConditionService
{
    public async Task<IReadOnlyList<TermsAndCondition>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);

        return await query
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Country)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<TermsAndCondition?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking().Where(x => x.Slug == slug);
        if (publishedOnly) query = query.Where(x => x.IsPublished);
        return query.FirstOrDefaultAsync(ct);
    }
}
