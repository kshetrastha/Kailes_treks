using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Infrastructure.Services;

namespace TravelCleanArch.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public IWhyWithUsService WhyWithUsService => new WhyWithUsService(dbContext);

    public IWhyWithUsHeroService WhyWithUsHeroService => new WhyWithUsHeroService(dbContext);

    public IWhoWeAreService WhoWeAreService => new WhoWeAreService(dbContext);

    public IWhoWeAreHeroService WhoWeAreHeroService => new WhoWeAreHeroService(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
}
