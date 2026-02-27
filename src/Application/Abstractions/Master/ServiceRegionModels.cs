using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record ServiceRegionPagedResult(IReadOnlyCollection<ServiceRegion> Items, int Page, int PageSize, int TotalCount);
