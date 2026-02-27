using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record CategoryPagedResult(IReadOnlyCollection<Category> Items, int Page, int PageSize, int TotalCount);
