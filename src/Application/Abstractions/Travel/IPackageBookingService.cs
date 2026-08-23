using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record PackageBookingFilter(
    string? Search,
    PackageBookingStatus? Status,
    int Page,
    int PageSize);

public sealed record PackageBookingPagedResult(
    IReadOnlyList<PackageBooking> Items,
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyDictionary<PackageBookingStatus, int> StatusCounts);

public interface IPackageBookingService : IGenericRepository<PackageBooking>
{
    Task<PackageBookingPagedResult> ListAsync(PackageBookingFilter filter, CancellationToken ct);
    Task<PackageBooking?> GetDetailAsync(int id, CancellationToken ct);
    Task<PackageBooking?> GetByReferenceAsync(string reference, CancellationToken ct);
    Task<string> GenerateReferenceAsync(DateTime submittedAtUtc, CancellationToken ct);
}
