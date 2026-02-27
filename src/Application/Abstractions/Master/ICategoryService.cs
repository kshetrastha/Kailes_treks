using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface ICategoryService : IGenericRepository<Category>
{
    Task<CategoryPagedResult> ListOrderedAsync(int page, int pageSize, string? categoryName, CancellationToken ct);
}
