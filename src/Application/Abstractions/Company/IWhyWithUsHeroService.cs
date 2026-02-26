using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IWhyWithUsHeroService : IGenericRepository<WhyWithUsHero>
{
    Task<WhyWithUsHero?> GetFirstAsync(bool asNoTracking, CancellationToken ct);
}
