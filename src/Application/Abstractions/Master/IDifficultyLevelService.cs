using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IDifficultyLevelService : IGenericRepository<DifficultyLevel>
{
    Task<IReadOnlyList<DifficultyLevel>> ListOrderedAsync(CancellationToken ct);
}
