using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IBlogPostService : IGenericRepository<BlogPost>
{
    Task<IReadOnlyList<BlogPost>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
    Task<BlogPost?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken ct);
}
