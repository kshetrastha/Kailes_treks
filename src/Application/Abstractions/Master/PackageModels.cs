using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record PackagePagedResult(IReadOnlyCollection<Package> Items, int Page, int PageSize, int TotalCount);
