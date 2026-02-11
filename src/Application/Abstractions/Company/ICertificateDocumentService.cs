using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface ICertificateDocumentService : IGenericRepository<CertificateDocument>
{
    Task<IReadOnlyList<CertificateDocument>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
}
