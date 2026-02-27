using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class CategoryService(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryService
{
    public async Task<CategoryPagedResult> ListOrderedAsync(int page, int pageSize, string? categoryName, CancellationToken ct)
    {
        page = Math.Max(1, page);
        var trimmedCategoryName = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName.Trim();

        var query = Query()
            .AsNoTracking();

        if (trimmedCategoryName is not null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmedCategoryName}%"));
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
        return new CategoryPagedResult(items, effectivePage, effectivePageSize, totalCount);
    }
}
