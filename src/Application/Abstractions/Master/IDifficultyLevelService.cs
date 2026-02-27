using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IDifficultyLevelService : IGenericRepository<DifficultyLevel>
{
    Task<DifficultyLevelPagedResult> ListOrderedAsync(int page, int pageSize, string? levelName, CancellationToken ct);
}
