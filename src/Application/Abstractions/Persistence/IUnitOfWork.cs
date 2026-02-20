using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Application.Abstractions.Travel;

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
    IBlogPostService BlogPostService { get; }
    IExpeditionService ExpeditionService { get; }
    public ITrekkingService TrekkingService { get; }
    public ITrekkingTypeService TrekkingTypeService {  get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
