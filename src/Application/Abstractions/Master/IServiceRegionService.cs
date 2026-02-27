using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IServiceRegionService : IGenericRepository<ServiceRegion>
{
    Task<ServiceRegionPagedResult> ListOrderedAsync(int page, int pageSize, string? name, int? serviceTypeId, CancellationToken ct);
}
