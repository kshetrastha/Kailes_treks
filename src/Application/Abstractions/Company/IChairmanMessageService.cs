using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IChairmanMessageService : IGenericRepository<ChairmanMessage>
{
    Task<IReadOnlyList<ChairmanMessage>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
