using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Master;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class AccomodationService(AppDbContext dbContext) : GenericRepository<Accomodation>(dbContext), IAccomodationService
{
    public Task<IReadOnlyList<Accomodation>> ListOrderedAsync(CancellationToken ct)
        => Query().AsNoTracking().OrderBy(x => x.Name).ThenBy(x => x.Id).ToListAsync(ct);
}
