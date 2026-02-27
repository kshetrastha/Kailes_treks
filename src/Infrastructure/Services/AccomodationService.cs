using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class AccomodationService(AppDbContext dbContext) : GenericRepository<Accomodation>(dbContext), IAccomodationService
{
    public async Task<AccomodationPagedResult> ListOrderedAsync(int page, int pageSize, string? accomodationName, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedAccomodationName = string.IsNullOrWhiteSpace(accomodationName) ? null : accomodationName.Trim();

        var query = Query()
            .AsNoTracking();

        if (trimmedAccomodationName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedAccomodationName}%"));
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
        return new AccomodationPagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
