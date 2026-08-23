using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class PackageBookingService(AppDbContext dbContext)
    : GenericRepository<PackageBooking>(dbContext), IPackageBookingService
{
    public async Task<PackageBookingPagedResult> ListAsync(PackageBookingFilter filter, CancellationToken ct)
    {
        var baseQuery = Query().AsNoTracking();

        var statusCounts = await baseQuery
            .GroupBy(x => x.Status)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count, ct);

        var query = baseQuery;

        if (filter.Status is { } status)
        {
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = $"%{filter.Search.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Reference, term) ||
                EF.Functions.ILike(x.FirstName, term) ||
                EF.Functions.ILike(x.LastName, term) ||
                EF.Functions.ILike(x.Email, term) ||
                EF.Functions.ILike(x.Phone, term) ||
                EF.Functions.ILike(x.PackageName, term));
        }

        var totalCount = await query.CountAsync(ct);

        var page = Math.Max(1, filter.Page);
        var pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;

        var items = await query
            .OrderByDescending(x => x.SubmittedAtUtc)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PackageBookingPagedResult(items, page, pageSize, totalCount, statusCounts);
    }

    public Task<PackageBooking?> GetDetailAsync(int id, CancellationToken ct)
        => Query().Include(x => x.Trekking).FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<PackageBooking?> GetByReferenceAsync(string reference, CancellationToken ct)
        => Query().AsNoTracking().FirstOrDefaultAsync(x => x.Reference == reference, ct);

    /// <summary>Builds a human-quotable reference such as VKT-2026-000042, unique per year.</summary>
    public async Task<string> GenerateReferenceAsync(DateTime submittedAtUtc, CancellationToken ct)
    {
        var year = submittedAtUtc.Year;
        var prefix = $"VKT-{year}-";

        var lastReference = await Query()
            .AsNoTracking()
            .Where(x => x.Reference.StartsWith(prefix))
            .OrderByDescending(x => x.Reference)
            .Select(x => x.Reference)
            .FirstOrDefaultAsync(ct);

        var next = 1;
        if (lastReference is not null &&
            int.TryParse(lastReference[prefix.Length..], out var lastSequence))
        {
            next = lastSequence + 1;
        }

        return $"{prefix}{next:D6}";
    }
}
