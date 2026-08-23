using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Application.Abstractions.Travel;

namespace TravelCleanArch.Web.Models.Package;

/// <summary>Read-only package facts shown in the booking form's Tour Summary / Price Summary.</summary>
public sealed class PackageBookingSummaryViewModel
{
    public int TrekkingId { get; init; }
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Destination { get; init; }
    public string? Region { get; init; }
    public string? TrekkingType { get; init; }
    public string? Difficulty { get; init; }
    public int DurationDays { get; init; }
    public int MaxAltitudeMeters { get; init; }
    public string? BestSeason { get; init; }
    public bool PriceOnRequest { get; init; }
    public decimal? Price { get; init; }
    public string CurrencyCode { get; init; } = "USD";
    public string? HeroImageUrl { get; init; }
    public int MinGroupSize { get; init; }
    public int MaxGroupSize { get; init; }
    public IReadOnlyList<TrekkingFixedDepartureDto> FixedDepartures { get; init; } = [];
}

public sealed class PackageBookingFormViewModel
{
    public PackageBookingSummaryViewModel Package { get; set; } = new();

    [Required(ErrorMessage = "First name is required."), StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required."), StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required."), EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required."), StringLength(50)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(50)] public string? AlternatePhone { get; set; }
    [DataType(DataType.Date)] public DateOnly? DateOfBirth { get; set; }
    [StringLength(20)] public string? Gender { get; set; }
    [StringLength(100)] public string? Nationality { get; set; }
    [StringLength(50)] public string? PassportNumber { get; set; }

    [StringLength(300)] public string? Address { get; set; }
    [StringLength(120)] public string? City { get; set; }
    [StringLength(30)] public string? PostalCode { get; set; }
    [StringLength(120)] public string? Country { get; set; }

    [StringLength(150)] public string? EmergencyContactName { get; set; }
    [StringLength(50)] public string? EmergencyContactPhone { get; set; }

    [Range(1, 100, ErrorMessage = "Enter between 1 and 100 travellers.")]
    public int NumberOfTravellers { get; set; } = 1;

    [DataType(DataType.Date)] public DateOnly? PreferredStartDate { get; set; }
    public int? FixedDepartureId { get; set; }

    [StringLength(4000)] public string? SpecialRequests { get; set; }
}

public sealed class PackageBookingConfirmationViewModel
{
    public string Reference { get; init; } = string.Empty;
    public string PackageName { get; init; } = string.Empty;
    public string PackageSlug { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int NumberOfTravellers { get; init; }
    public DateOnly? PreferredStartDate { get; init; }
    public decimal? TotalAmount { get; init; }
    public string? CurrencyCode { get; init; }
    public bool PriceOnRequest { get; init; }
}
