using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface ITermsAndConditionService : IGenericRepository<TermsAndCondition>
{
    Task<IReadOnlyList<TermsAndCondition>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
    Task<TermsAndCondition?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken ct);
}
