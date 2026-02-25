using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ServiceTypeService(AppDbContext dbContext) : GenericRepository<ServiceType>(dbContext), IServiceTypeService
{
    public async Task<ServiceTypePagedResult> ListOrderedAsync(int page, int pageSize, string? serviceName, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedServiceName = string.IsNullOrWhiteSpace(serviceName) ? null : serviceName.Trim();

        var query = Query()
            .AsNoTracking();

        if (trimmedServiceName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedServiceName}%"));
        }

        var totalCount = await query.CountAsync(ct);
        var effectivePageSize = pageSize <= 0 ? Math.Max(1, totalCount) : Math.Max(1, pageSize);

        var orderedQuery = query
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id);

        var items = pageSize <= 0
            ? await orderedQuery.ToListAsync(ct)
            : await orderedQuery.Skip((page - 1) * effectivePageSize).Take(effectivePageSize).ToListAsync(ct);

        var effectivePage = pageSize <= 0 ? 1 : page;
        return new ServiceTypePagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
