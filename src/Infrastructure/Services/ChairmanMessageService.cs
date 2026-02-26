using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ChairmanMessageService(AppDbContext dbContext) : GenericRepository<ChairmanMessage>(dbContext), IChairmanMessageService
{
    public async Task<IReadOnlyList<ChairmanMessage>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);
        return await query.OrderByDescending(x => x.UpdatedAtUtc).ThenByDescending(x => x.Id).ToListAsync(ct);
    }
}
