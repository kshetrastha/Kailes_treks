using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IAboutUsService : IGenericRepository<AboutUsPage>
{
    Task<AboutUsPage?> GetPageAsync(bool asNoTracking, bool publishedOnly, CancellationToken ct);
}
