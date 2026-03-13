using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IPackageService : IGenericRepository<Package>
{
    Task<PackagePagedResult> ListOrderedAsync(int page, int pageSize, string? name, int? categoryId, int? serviceRegionId, int? difficultyLevelId, CancellationToken ct);
}
