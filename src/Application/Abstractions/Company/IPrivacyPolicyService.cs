using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IPrivacyPolicyService : IGenericRepository<PrivacyPolicySection>
{
    Task<IReadOnlyList<PrivacyPolicySection>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
