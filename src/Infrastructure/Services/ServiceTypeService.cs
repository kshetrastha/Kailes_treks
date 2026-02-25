using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ServiceTypeService(AppDbContext dbContext) : GenericRepository<ServiceType>(dbContext), IServiceTypeService
{
    public async Task<IReadOnlyList<ServiceType>> ListOrderedAsync(CancellationToken ct)
    {
        var list = await Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return list;
    }
}
