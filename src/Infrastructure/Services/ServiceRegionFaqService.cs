using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ServiceRegionFaqService(AppDbContext dbContext) : GenericRepository<ServiceRegionFaq>(dbContext), IServiceRegionFaqService
{
    public async Task<ServiceRegionFaqPagedResult> ListOrderedAsync(int page, int pageSize, int? serviceRegionId, string? question, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedQuestion = string.IsNullOrWhiteSpace(question) ? null : question.Trim();

        var query = Query()
            .AsNoTracking()
            .Include(x => x.ServiceRegion)
            .AsQueryable();

        if (serviceRegionId.HasValue)
        {
            query = query.Where(x => x.ServiceRegionId == serviceRegionId.Value);
        }

        if (trimmedQuestion is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Question, $"%{trimmedQuestion}%"));
        }

        var totalCount = await query.CountAsync(ct);
        var effectivePageSize = pageSize <= 0 ? Math.Max(1, totalCount) : Math.Max(1, pageSize);

        var orderedQuery = query
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id);

        var items = pageSize <= 0
            ? await orderedQuery.ToListAsync(ct)
            : await orderedQuery.Skip((page - 1) * effectivePageSize).Take(effectivePageSize).ToListAsync(ct);

        var effectivePage = pageSize <= 0 ? 1 : page;
        return new ServiceRegionFaqPagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
