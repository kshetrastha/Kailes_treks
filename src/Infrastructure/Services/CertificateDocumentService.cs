using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class CertificateDocumentService(AppDbContext dbContext) : GenericRepository<CertificateDocument>(dbContext), ICertificateDocumentService
{
    public async Task<IReadOnlyList<CertificateDocument>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);
        return await query.OrderBy(x => x.Ordering).ThenBy(x => x.Id).ToListAsync(ct);
    }
}
