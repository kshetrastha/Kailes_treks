using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class DifficultyLevelService(AppDbContext dbContext) : GenericRepository<DifficultyLevel>(dbContext), IDifficultyLevelService
{
    public async Task<DifficultyLevelPagedResult> ListOrderedAsync(int page, int pageSize, string? levelName, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedLevelName = string.IsNullOrWhiteSpace(levelName) ? null : levelName.Trim();

        var query = Query()
            .AsNoTracking();

        if (trimmedLevelName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedLevelName}%"));
        }

        var totalCount = await query.CountAsync(ct);
        var effectivePageSize = pageSize <= 0 ? Math.Max(1, totalCount) : Math.Max(1, pageSize);

        var orderedQuery = query
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Id);

        var items = pageSize <= 0
            ? await orderedQuery.ToListAsync(ct)
            : await orderedQuery.Skip((page - 1) * effectivePageSize).Take(effectivePageSize).ToListAsync(ct);

        var effectivePage = pageSize <= 0 ? 1 : page;
        return new DifficultyLevelPagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
