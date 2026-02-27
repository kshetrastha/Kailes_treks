using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record AccomodationPagedResult(IReadOnlyCollection<Accomodation> Items, int Page, int PageSize, int TotalCount);
