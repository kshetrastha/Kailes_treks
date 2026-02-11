using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IWhoWeAreHeroService : IGenericRepository<WhoWeAreHero>
{
    Task<WhoWeAreHero?> GetFirstAsync(bool asNoTracking, CancellationToken ct);
}
