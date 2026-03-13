using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class PackageService(AppDbContext dbContext) : GenericRepository<Package>(dbContext), IPackageService
{
    public async Task<PackagePagedResult> ListOrderedAsync(int page, int pageSize, string? name, int? categoryId, int? serviceRegionId, int? difficultyLevelId, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();

        var query = Query()
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.ServiceRegion)
            .Include(x => x.DifficultyLevel)
            .AsQueryable();

        if (trimmedName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedName}%"));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        if (serviceRegionId.HasValue)
        {
            query = query.Where(x => x.ServiceRegionId == serviceRegionId.Value);
        }

        if (difficultyLevelId.HasValue)
        {
            query = query.Where(x => x.DifficultyLevelId == difficultyLevelId.Value);
        }

        var totalCount = await query.CountAsync(ct);
        var effectivePageSize = pageSize <= 0 ? Math.Max(1, totalCount) : Math.Max(1, pageSize);

        var orderedQuery = query
            .OrderBy(x => x.PopularityRank)
            .ThenByDescending(x => x.BestSeller)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Id);

        var items = pageSize <= 0
            ? await orderedQuery.ToListAsync(ct)
            : await orderedQuery.Skip((page - 1) * effectivePageSize).Take(effectivePageSize).ToListAsync(ct);

        var effectivePage = pageSize <= 0 ? 1 : page;
        return new PackagePagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
