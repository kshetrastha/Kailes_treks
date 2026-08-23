using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class PrivacyPolicyService(AppDbContext dbContext) : GenericRepository<PrivacyPolicySection>(dbContext), IPrivacyPolicyService
{
    public async Task<IReadOnlyList<PrivacyPolicySection>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);

        return await query
            .OrderBy(x => x.IsContactBlock)
            .ThenBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);
    }
}
