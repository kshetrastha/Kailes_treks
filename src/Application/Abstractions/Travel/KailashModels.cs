using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Travel;

// ── Packages ────────────────────────────────────────────────────────────────

public sealed class KailashYatraPackageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<int> AvailableMonths { get; set; } = [];
    public List<int> FullMoonMonths { get; set; } = [];
    public decimal Price { get; set; }
    public int Ordering { get; set; }
    public bool IsActive { get; set; }
}

public sealed class KailashYatraPackageUpsertDto
{
    public string Name { get; set; } = string.Empty;
    public List<int> AvailableMonths { get; set; } = [];
    public List<int> FullMoonMonths { get; set; } = [];
    public decimal Price { get; set; }
    public int Ordering { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── Booking create ───────────────────────────────────────────────────────────

public sealed class KailashBookingCreateDto
{
    public int PackageId { get; set; }
    public int SelectedMonth { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public decimal BookingAmount { get; set; }

    public string LastName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string? TelCountryCode { get; set; }
    public string? TelAreaCode { get; set; }
    public string? TelNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public string PassportNumber { get; set; } = string.Empty;
    public string PlaceOfIssue { get; set; } = string.Empty;
    public DateOnly DateOfIssue { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? Insurance { get; set; }

    public string EmergencyName { get; set; } = string.Empty;
    public string EmergencyRelationship { get; set; } = string.Empty;
    public string? EmergencyTelCountryCode { get; set; }
    public string? EmergencyTelAreaCode { get; set; }
    public string? EmergencyTelNumber { get; set; }
    public string? EmergencyMobile { get; set; }

    public Dictionary<string, bool> HealthDeclaration { get; set; } = [];
    public string? OtherHealthConcerns { get; set; }

    public string? SpecialRequests { get; set; }
    public int NumberOfTravellers { get; set; } = 1;
}

// ── Booking list / detail ────────────────────────────────────────────────────

public sealed class KailashBookingListItemDto
{
    public int Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int SelectedMonth { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfTravellers { get; set; }
    public decimal BookingAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime SubmittedAtUtc { get; set; }
}

public sealed class KailashBookingDetailsDto
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int SelectedMonth { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public decimal BookingAmount { get; set; }

    public string LastName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string? TelCountryCode { get; set; }
    public string? TelAreaCode { get; set; }
    public string? TelNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public string PassportNumber { get; set; } = string.Empty;
    public string PlaceOfIssue { get; set; } = string.Empty;
    public DateOnly DateOfIssue { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? Insurance { get; set; }

    public string EmergencyName { get; set; } = string.Empty;
    public string EmergencyRelationship { get; set; } = string.Empty;
    public string? EmergencyTelCountryCode { get; set; }
    public string? EmergencyTelAreaCode { get; set; }
    public string? EmergencyTelNumber { get; set; }
    public string? EmergencyMobile { get; set; }

    public Dictionary<string, bool> HealthDeclaration { get; set; } = [];
    public string? OtherHealthConcerns { get; set; }

    public string? SpecialRequests { get; set; }
    public int NumberOfTravellers { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime SubmittedAtUtc { get; set; }
}

public sealed class KailashBookingPagedResult
{
    public List<KailashBookingListItemDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
