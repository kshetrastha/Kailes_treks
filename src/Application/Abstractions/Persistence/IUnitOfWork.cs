using TravelCleanArch.Application.Abstractions.Company;

namespace TravelCleanArch.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IWhyWithUsService WhyWithUsService { get; }
    IWhyWithUsHeroService WhyWithUsHeroService { get; }
    IWhoWeAreService WhoWeAreService { get; }
    IWhoWeAreHeroService WhoWeAreHeroService { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
