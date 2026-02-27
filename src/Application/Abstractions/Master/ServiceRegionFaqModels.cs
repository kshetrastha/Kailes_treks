using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record ServiceRegionFaqPagedResult(IReadOnlyCollection<ServiceRegionFaq> Items, int Page, int PageSize, int TotalCount);
