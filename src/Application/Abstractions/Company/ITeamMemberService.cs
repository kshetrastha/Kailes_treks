using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface ITeamMemberService : IGenericRepository<TeamMember>
{
    Task<IReadOnlyList<TeamMember>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
