using TravelCleanArch.Application.Abstractions.Company;

namespace TravelCleanArch.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IWhyWithUsService WhyWithUsService { get; }
    IWhyWithUsHeroService WhyWithUsHeroService { get; }
    IWhoWeAreService WhoWeAreService { get; }
    IWhoWeAreHeroService WhoWeAreHeroService { get; }
    IAwardService AwardService { get; }
    IPatronService PatronService { get; }
    IChairmanMessageService ChairmanMessageService { get; }
    ITeamMemberService TeamMemberService { get; }
    ICertificateDocumentService CertificateDocumentService { get; }
    IReviewService ReviewService { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
