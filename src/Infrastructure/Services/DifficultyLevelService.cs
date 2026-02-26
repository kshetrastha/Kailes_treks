using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class DifficultyLevelService(AppDbContext dbContext) : GenericRepository<DifficultyLevel>(dbContext), IDifficultyLevelService
{
    public async Task<IReadOnlyList<DifficultyLevel>> ListOrderedAsync(CancellationToken ct)
    {
        var list = await Query()
        .AsNoTracking()
        .OrderBy(x => x.Name)
        .ThenBy(x => x.Id)
        .ToListAsync(ct);
        return list;
    }
}
