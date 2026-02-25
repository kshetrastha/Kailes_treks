using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IServiceTypeService : IGenericRepository<ServiceType>
{
    Task<IReadOnlyList<ServiceType>> ListOrderedAsync(CancellationToken ct);
}
