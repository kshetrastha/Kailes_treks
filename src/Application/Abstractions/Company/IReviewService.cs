using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IReviewService : IGenericRepository<Review>
{
    Task<IReadOnlyList<Review>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
