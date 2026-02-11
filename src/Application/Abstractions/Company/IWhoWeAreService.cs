using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IWhoWeAreService : IGenericRepository<WhoWeAre>
{
    Task<IReadOnlyList<WhoWeAre>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
    Task<string?> GetImagePathByIdAsync(int id, CancellationToken ct);
}
