using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class WhoWeAreService(AppDbContext dbContext) : GenericRepository<WhoWeAre>(dbContext), IWhoWeAreService
{
    public async Task<IReadOnlyList<WhoWeAre>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly)
        {
            query = query.Where(x => x.IsPublished);
        }

        return await query
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<string?> GetImagePathByIdAsync(int id, CancellationToken ct)
        => Query().AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.ImagePath)
            .FirstOrDefaultAsync(ct);
}
