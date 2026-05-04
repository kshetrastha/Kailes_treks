using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IMasterFaqService : IGenericRepository<MasterFaq>
{
    Task<IReadOnlyList<MasterFaq>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
