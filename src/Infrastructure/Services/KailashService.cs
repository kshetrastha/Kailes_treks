using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class KailashService(AppDbContext db) : IKailashService
{
    private static readonly JsonSerializerOptions _json = new() { WriteIndented = false };

    // ── helpers ─────────────────────────────────────────────────────────────

    private static List<int> ParseMonths(string raw) =>
        string.IsNullOrWhiteSpace(raw)
            ? []
            : raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                 .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
                 .Where(n => n > 0)
                 .ToList();

    private static string SerializeMonths(List<int> months) =>
        string.Join(",", months.Distinct().OrderBy(m => m));

    private static KailashYatraPackageDto ToPackageDto(KailashYatraPackage p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        AvailableMonths = ParseMonths(p.AvailableMonths),
        FullMoonMonths = ParseMonths(p.FullMoonMonths),
        Price = p.Price,
        Ordering = p.Ordering,
        IsActive = p.IsActive,
    };

    // ── Packages ─────────────────────────────────────────────────────────────

    public async Task<List<KailashYatraPackageDto>> GetAllPackagesAsync(CancellationToken ct = default) =>
        (await db.KailashYatraPackages.OrderBy(p => p.Ordering).ThenBy(p => p.Id).ToListAsync(ct))
            .Select(ToPackageDto).ToList();

    public async Task<List<KailashYatraPackageDto>> GetActivePackagesAsync(CancellationToken ct = default) =>
        (await db.KailashYatraPackages.Where(p => p.IsActive).OrderBy(p => p.Ordering).ThenBy(p => p.Id).ToListAsync(ct))
            .Select(ToPackageDto).ToList();

    public async Task<KailashYatraPackageDto?> GetPackageByIdAsync(int id, CancellationToken ct = default)
    {
        var p = await db.KailashYatraPackages.FindAsync([id], ct);
        return p is null ? null : ToPackageDto(p);
    }

    public async Task<int> CreatePackageAsync(KailashYatraPackageUpsertDto dto, CancellationToken ct = default)
    {
        var p = new KailashYatraPackage
        {
            Name = dto.Name,
            AvailableMonths = SerializeMonths(dto.AvailableMonths),
            FullMoonMonths = SerializeMonths(dto.FullMoonMonths),
            Price = dto.Price,
            Ordering = dto.Ordering,
            IsActive = dto.IsActive,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };
        db.KailashYatraPackages.Add(p);
        await db.SaveChangesAsync(ct);
        return p.Id;
    }

    public async Task<bool> UpdatePackageAsync(int id, KailashYatraPackageUpsertDto dto, CancellationToken ct = default)
    {
        var p = await db.KailashYatraPackages.FindAsync([id], ct);
        if (p is null) return false;
        p.Name = dto.Name;
        p.AvailableMonths = SerializeMonths(dto.AvailableMonths);
        p.FullMoonMonths = SerializeMonths(dto.FullMoonMonths);
        p.Price = dto.Price;
        p.Ordering = dto.Ordering;
        p.IsActive = dto.IsActive;
        p.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeletePackageAsync(int id, CancellationToken ct = default)
    {
        var p = await db.KailashYatraPackages.FindAsync([id], ct);
        if (p is null) return false;
        db.KailashYatraPackages.Remove(p);
        await db.SaveChangesAsync(ct);
        return true;
    }

    // ── Bookings ─────────────────────────────────────────────────────────────

    public async Task<int> CreateBookingAsync(KailashBookingCreateDto dto, string? ipAddress, CancellationToken ct = default)
    {
        var b = new KailashBooking
        {
            PackageId = dto.PackageId,
            SelectedMonth = dto.SelectedMonth,
            ArrivalDate = dto.ArrivalDate,
            DepartureDate = dto.DepartureDate,
            BookingAmount = dto.BookingAmount,
            LastName = dto.LastName,
            GivenName = dto.GivenName,
            MiddleName = dto.MiddleName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Email = dto.Email,
            Nationality = dto.Nationality,
            TelCountryCode = dto.TelCountryCode,
            TelAreaCode = dto.TelAreaCode,
            TelNumber = dto.TelNumber,
            MobileNumber = dto.MobileNumber,
            Occupation = dto.Occupation,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            PassportNumber = dto.PassportNumber,
            PlaceOfIssue = dto.PlaceOfIssue,
            DateOfIssue = dto.DateOfIssue,
            ExpiryDate = dto.ExpiryDate,
            Insurance = dto.Insurance,
            EmergencyName = dto.EmergencyName,
            EmergencyRelationship = dto.EmergencyRelationship,
            EmergencyTelCountryCode = dto.EmergencyTelCountryCode,
            EmergencyTelAreaCode = dto.EmergencyTelAreaCode,
            EmergencyTelNumber = dto.EmergencyTelNumber,
            EmergencyMobile = dto.EmergencyMobile,
            HealthDeclaration = JsonSerializer.Serialize(dto.HealthDeclaration, _json),
            OtherHealthConcerns = dto.OtherHealthConcerns,
            SpecialRequests = dto.SpecialRequests,
            NumberOfTravellers = dto.NumberOfTravellers,
            Status = KailashBookingStatus.Pending,
            SubmittedAtUtc = DateTime.UtcNow,
            IpAddress = ipAddress,
        };
        db.KailashBookings.Add(b);
        await db.SaveChangesAsync(ct);
        return b.Id;
    }

    public async Task<KailashBookingPagedResult> ListBookingsAsync(string? search, string? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = db.KailashBookings.Include(b => b.Package).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(b => b.LastName.ToLower().Contains(s) || b.GivenName.ToLower().Contains(s) || b.Email.ToLower().Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<KailashBookingStatus>(status, true, out var statusEnum))
            q = q.Where(b => b.Status == statusEnum);

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(b => b.SubmittedAtUtc)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(b => new KailashBookingListItemDto
            {
                Id = b.Id,
                PackageName = b.Package.Name,
                SelectedMonth = b.SelectedMonth,
                FullName = b.GivenName + " " + b.LastName,
                Email = b.Email,
                NumberOfTravellers = b.NumberOfTravellers,
                BookingAmount = b.BookingAmount,
                Status = b.Status.ToString(),
                SubmittedAtUtc = b.SubmittedAtUtc,
            }).ToListAsync(ct);

        return new KailashBookingPagedResult { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<KailashBookingDetailsDto?> GetBookingByIdAsync(int id, CancellationToken ct = default)
    {
        var b = await db.KailashBookings.Include(b => b.Package).FirstOrDefaultAsync(b => b.Id == id, ct);
        if (b is null) return null;

        Dictionary<string, bool> health;
        try { health = JsonSerializer.Deserialize<Dictionary<string, bool>>(b.HealthDeclaration) ?? []; }
        catch { health = []; }

        return new KailashBookingDetailsDto
        {
            Id = b.Id,
            PackageId = b.PackageId,
            PackageName = b.Package.Name,
            SelectedMonth = b.SelectedMonth,
            ArrivalDate = b.ArrivalDate,
            DepartureDate = b.DepartureDate,
            BookingAmount = b.BookingAmount,
            LastName = b.LastName,
            GivenName = b.GivenName,
            MiddleName = b.MiddleName,
            DateOfBirth = b.DateOfBirth,
            Gender = b.Gender,
            Email = b.Email,
            Nationality = b.Nationality,
            TelCountryCode = b.TelCountryCode,
            TelAreaCode = b.TelAreaCode,
            TelNumber = b.TelNumber,
            MobileNumber = b.MobileNumber,
            Occupation = b.Occupation,
            Address = b.Address,
            City = b.City,
            PostalCode = b.PostalCode,
            Country = b.Country,
            PassportNumber = b.PassportNumber,
            PlaceOfIssue = b.PlaceOfIssue,
            DateOfIssue = b.DateOfIssue,
            ExpiryDate = b.ExpiryDate,
            Insurance = b.Insurance,
            EmergencyName = b.EmergencyName,
            EmergencyRelationship = b.EmergencyRelationship,
            EmergencyTelCountryCode = b.EmergencyTelCountryCode,
            EmergencyTelAreaCode = b.EmergencyTelAreaCode,
            EmergencyTelNumber = b.EmergencyTelNumber,
            EmergencyMobile = b.EmergencyMobile,
            HealthDeclaration = health,
            OtherHealthConcerns = b.OtherHealthConcerns,
            SpecialRequests = b.SpecialRequests,
            NumberOfTravellers = b.NumberOfTravellers,
            Status = b.Status.ToString(),
            SubmittedAtUtc = b.SubmittedAtUtc,
        };
    }

    public async Task<bool> UpdateBookingStatusAsync(int id, string status, CancellationToken ct = default)
    {
        if (!Enum.TryParse<KailashBookingStatus>(status, true, out var s)) return false;
        var b = await db.KailashBookings.FindAsync([id], ct);
        if (b is null) return false;
        b.Status = s;
        await db.SaveChangesAsync(ct);
        return true;
    }
}
