using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ServiceRegionService(AppDbContext dbContext) : GenericRepository<ServiceRegion>(dbContext), IServiceRegionService
{
    public async Task<ServiceRegionPagedResult> ListOrderedAsync(int page, int pageSize, string? name, int? serviceTypeId, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();

        var query = Query()
            .AsNoTracking()
            .Include(x => x.ServiceType)
            .AsQueryable();

        if (trimmedName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedName}%"));
        }

        if (serviceTypeId.HasValue)
        {
            query = query.Where(x => x.ServiceTypeId == serviceTypeId.Value);
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
        return new ServiceRegionPagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
