namespace TravelCleanArch.Domain.Entities;

public enum PackageBookingStatus
{
    Pending = 0,
    Contacted = 1,
    Booked = 2,
    Completed = 3,
    Cancelled = 4
}

/// <summary>
/// A booking request raised from a package detail page. Package fields are snapshotted at
/// submission time so the record stays accurate even after the package is edited or removed.
/// </summary>
public sealed class PackageBooking : BaseEntity
{
    public string Reference { get; set; } = string.Empty;

    // --- Package snapshot -------------------------------------------------
    public int TrekkingId { get; set; }
    public Trekking? Trekking { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public string PackageSlug { get; set; } = string.Empty;
    public string? PackageDestination { get; set; }
    public string? PackageRegion { get; set; }
    public string? PackageTrekkingType { get; set; }
    public string? PackageDifficulty { get; set; }
    public int PackageDurationDays { get; set; }
    public int PackageMaxAltitudeMeters { get; set; }
    public bool PriceOnRequest { get; set; }
    public decimal? PricePerPerson { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? TotalAmount { get; set; }

    // --- Trip selection ---------------------------------------------------
    public DateOnly? PreferredStartDate { get; set; }
    public int? FixedDepartureId { get; set; }
    public int NumberOfTravellers { get; set; } = 1;

    // --- Traveller details ------------------------------------------------
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? AlternatePhone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? PassportNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? SpecialRequests { get; set; }

    // --- Request / network metadata captured at submission -----------------
    public string? IpAddress { get; set; }
    public string? IpCountry { get; set; }
    public string? IpCountryCode { get; set; }
    public string? IpRegion { get; set; }
    public string? IpCity { get; set; }
    public string? IpPostalCode { get; set; }
    public string? IpTimeZone { get; set; }
    public string? IpOrganisation { get; set; }
    public double? IpLatitude { get; set; }
    public double? IpLongitude { get; set; }
    public string? UserAgent { get; set; }
    public string? BrowserLanguage { get; set; }
    public string? Referrer { get; set; }
    public string? SourcePage { get; set; }

    // --- Workflow ---------------------------------------------------------
    public PackageBookingStatus Status { get; set; } = PackageBookingStatus.Pending;
    public string? AdminNotes { get; set; }
    public DateTime SubmittedAtUtc { get; set; }
    public DateTime? StatusChangedAtUtc { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
