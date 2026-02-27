using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IServiceRegionFaqService : IGenericRepository<ServiceRegionFaq>
{
    Task<ServiceRegionFaqPagedResult> ListOrderedAsync(int page, int pageSize, int? serviceRegionId, string? question, CancellationToken ct);
}
