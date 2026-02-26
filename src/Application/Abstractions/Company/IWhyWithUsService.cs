using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Company;

public interface IWhyWithUsService : IGenericRepository<WhyWithUs>
{
    Task<IReadOnlyList<WhyWithUs>> ListOrderedAsync(bool publishedOnly, CancellationToken ct);
    Task<string?> GetImagePathByIdAsync(int id, CancellationToken ct);
}
