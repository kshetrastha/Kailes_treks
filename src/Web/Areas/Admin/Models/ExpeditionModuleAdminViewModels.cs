using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class ExpeditionModuleFormViewModel : IValidatableObject
{
    public int? Id { get; set; }
    [Required] public int ExpeditionTypeId { get; set; }
    [Required, StringLength(220)] public string Name { get; set; } = string.Empty;
    [Required, StringLength(800)] public string ShortDescription { get; set; } = string.Empty;
    [Required] public string DifficultyLevel { get; set; } = "Moderate";
    public int? MaxElevation { get; set; }
    [Required, StringLength(120)] public string Duration { get; set; } = string.Empty;
    [StringLength(120)] public string? WalkingHoursPerDay { get; set; }
    [StringLength(200)] public string? Accommodation { get; set; }
    [StringLength(200)] public string? BestSeason { get; set; }
    [StringLength(80)] public string? GroupSize { get; set; }
    public bool IsFeatured { get; set; }
    [StringLength(500)] public string? BannerImagePath { get; set; }
    [StringLength(500)] public string? ThumbnailImagePath { get; set; }

    public ExpeditionOverviewFormViewModel Overview { get; set; } = new();
    public List<ExpeditionItineraryFormRow> Itineraries { get; set; } = [];
    public List<ExpeditionInclusionExclusionFormRow> InclusionExclusions { get; set; } = [];
    public List<ExpeditionFixedDepartureFormRow> FixedDepartures { get; set; } = [];
    public List<ExpeditionGearFormRow> Gears { get; set; } = [];
    public List<ExpeditionReviewFormRow> Reviews { get; set; } = [];
    public List<ExpeditionFaqFormRow> Faqs { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var row in FixedDepartures)
        {
            if (row.EndDate < row.StartDate)
            {
                yield return new ValidationResult("End date must be greater than or equal to start date.", [nameof(FixedDepartures)]);
            }
            if (row.BookedSeats > row.TotalSeats)
            {
                yield return new ValidationResult("Booked seats cannot exceed total seats.", [nameof(FixedDepartures)]);
            }
        }
    }
}

public sealed class ExpeditionOverviewFormViewModel
{
    public string? Country { get; set; }
    public string? PeakName { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }
    public string? Range { get; set; }
    public string? Coordinates { get; set; }
    public string? WeatherInformation { get; set; }
    public string? FullDescription { get; set; }
    public string? MapEmbedCode { get; set; }
}

public sealed class ExpeditionItineraryFormRow
{
    [Required] public string SeasonTitle { get; set; } = string.Empty;
    [Range(1, 100)] public int DayNumber { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Accommodation { get; set; }
    public string? Meals { get; set; }
    public string? Elevation { get; set; }
}

public sealed class ExpeditionInclusionExclusionFormRow
{
    [Required] public string Type { get; set; } = "Inclusion";
    [Required] public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public sealed class ExpeditionFixedDepartureFormRow
{
    [DataType(DataType.Date)] public DateOnly StartDate { get; set; }
    [DataType(DataType.Date)] public DateOnly EndDate { get; set; }
    [Range(0, int.MaxValue)] public int TotalSeats { get; set; }
    [Range(0, int.MaxValue)] public int BookedSeats { get; set; }
    [Range(0, 999999999)] public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = "BookingOpen";
    public bool IsGuaranteed { get; set; }
}

public sealed class ExpeditionGearFormRow
{
    [Required] public string Category { get; set; } = "Equipment";
    [Required] public string ItemName { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public int DisplayOrder { get; set; }
}

public sealed class ExpeditionReviewFormRow
{
    [Required] public string ClientName { get; set; } = string.Empty;
    public string? Country { get; set; }
    [Range(1, 5)] public int Rating { get; set; } = 5;
    public string? Title { get; set; }
    [Required] public string Comment { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public bool IsApproved { get; set; }
}

public sealed class ExpeditionFaqFormRow
{
    [Required] public string Question { get; set; } = string.Empty;
    [Required] public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
