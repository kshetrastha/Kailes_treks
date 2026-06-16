namespace TravelCleanArch.Domain.Entities;

public enum KailashBookingStatus { Pending = 0, Confirmed = 1, Cancelled = 2 }

public sealed class KailashBooking
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public KailashYatraPackage Package { get; set; } = null!;
    public int SelectedMonth { get; set; }  // 1-12
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public decimal BookingAmount { get; set; }

    // Personal
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

    // Passport
    public string PassportNumber { get; set; } = string.Empty;
    public string PlaceOfIssue { get; set; } = string.Empty;
    public DateOnly DateOfIssue { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? Insurance { get; set; }

    // Emergency contact
    public string EmergencyName { get; set; } = string.Empty;
    public string EmergencyRelationship { get; set; } = string.Empty;
    public string? EmergencyTelCountryCode { get; set; }
    public string? EmergencyTelAreaCode { get; set; }
    public string? EmergencyTelNumber { get; set; }
    public string? EmergencyMobile { get; set; }

    // Health declaration (JSON: {"HighBP":false,"LowBP":false,...})
    public string HealthDeclaration { get; set; } = "{}";
    public string? OtherHealthConcerns { get; set; }

    // Booking meta
    public string? SpecialRequests { get; set; }
    public int NumberOfTravellers { get; set; } = 1;
    public KailashBookingStatus Status { get; set; } = KailashBookingStatus.Pending;
    public DateTime SubmittedAtUtc { get; set; }
    public string? IpAddress { get; set; }
}
