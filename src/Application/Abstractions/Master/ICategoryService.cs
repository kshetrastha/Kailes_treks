using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface ICategoryService : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> ListOrderedAsync(CancellationToken ct);
}
