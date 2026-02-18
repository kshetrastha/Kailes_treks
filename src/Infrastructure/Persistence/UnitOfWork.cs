using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Infrastructure.Services;

namespace TravelCleanArch.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public IWhyWithUsService WhyWithUsService => new WhyWithUsService(dbContext);

    public IWhyWithUsHeroService WhyWithUsHeroService => new WhyWithUsHeroService(dbContext);

    public IWhoWeAreService WhoWeAreService => new WhoWeAreService(dbContext);

    public IWhoWeAreHeroService WhoWeAreHeroService => new WhoWeAreHeroService(dbContext);

    public IAwardService AwardService => new AwardService(dbContext);

    public IPatronService PatronService => new PatronService(dbContext);

    public IChairmanMessageService ChairmanMessageService => new ChairmanMessageService(dbContext);

    public ITeamMemberService TeamMemberService => new TeamMemberService(dbContext);

    public ICertificateDocumentService CertificateDocumentService => new CertificateDocumentService(dbContext);

    public IReviewService ReviewService => new ReviewService(dbContext);

    public IBlogPostService BlogPostService => new BlogPostService(dbContext);

    public IExpeditionService ExpeditionService => new ExpeditionService(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
}
