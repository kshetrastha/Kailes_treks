using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class WhyWithUsHeroService(AppDbContext dbContext) : GenericRepository<WhyWithUsHero>(dbContext), IWhyWithUsHeroService
{
    public Task<WhyWithUsHero?> GetFirstAsync(bool asNoTracking, CancellationToken ct)
    {
        var query = Query();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(ct);
    }
}
