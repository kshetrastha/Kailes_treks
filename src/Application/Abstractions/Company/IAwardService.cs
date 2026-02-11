using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IAwardService : IGenericRepository<Award>
{
    Task<IReadOnlyList<Award>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
