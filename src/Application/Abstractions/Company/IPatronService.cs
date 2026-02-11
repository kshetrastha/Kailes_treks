using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IPatronService : IGenericRepository<Patron>
{
    Task<IReadOnlyList<Patron>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
