using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Application.Abstractions.Master;
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
    ITermsAndConditionService TermsAndConditionService { get; }
    IExpeditionService ExpeditionService { get; }
    ITrekkingService TrekkingService { get; }
    ITrekkingTypeService TrekkingTypeService { get; }
    IAccomodationService AccomodationService { get; }
    ICategoryService CategoryService { get; }
    IDifficultyLevelService DifficultyLevelService { get; }
    IServiceTypeService ServiceTypeService { get; }
    IServiceRegionService ServiceRegionService { get; }
    IServiceRegionFaqService ServiceRegionFaqService { get; }
    IPackageService PackageService { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
