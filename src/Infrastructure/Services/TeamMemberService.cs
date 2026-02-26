using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class TeamMemberService(AppDbContext dbContext) : GenericRepository<TeamMember>(dbContext), ITeamMemberService
{
    public async Task<IReadOnlyList<TeamMember>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);
        return await query.OrderBy(x => x.Ordering).ThenBy(x => x.Id).ToListAsync(ct);
    }
}
