using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public interface IAccomodationService : IGenericRepository<Accomodation>
{
    Task<AccomodationPagedResult> ListOrderedAsync(int page, int pageSize, string? accomodationName, CancellationToken ct);
}
