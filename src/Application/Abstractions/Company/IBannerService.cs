using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IBannerService : IGenericRepository<Banner>
{
    Task<IReadOnlyList<Banner>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
