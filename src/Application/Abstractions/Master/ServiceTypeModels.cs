using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record ServiceTypePagedResult(IReadOnlyCollection<ServiceType> Items, int Page, int PageSize, int TotalCount);
