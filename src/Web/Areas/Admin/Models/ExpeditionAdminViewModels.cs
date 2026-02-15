using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class ExpeditionTypeImageInput
{
    public string? ExistingPath { get; set; }
    public IFormFile? File { get; set; }
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool Remove { get; set; }
}

public sealed class ExpeditionTypeFormViewModel : IValidatableObject
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(600)] public string ShortDescription { get; set; } = string.Empty;
    [StringLength(4000)] public string? Description { get; set; }
    public string? ExistingImagePath { get; set; }
    [Display(Name = "Image")] public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public List<ExpeditionTypeImageInput> Images { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var coverCount = Images.Count(x => !x.Remove && (x.IsCover && (x.File is not null || !string.IsNullOrWhiteSpace(x.ExistingPath))));
        var imageCount = Images.Count(x => !x.Remove && (x.File is not null || !string.IsNullOrWhiteSpace(x.ExistingPath)));
        if (imageCount > 0 && coverCount != 1)
        {
            yield return new ValidationResult("Exactly one cover image is required when images exist.", [nameof(Images)]);
        }
    }
}

public sealed class ItineraryDayInput
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? Meals { get; set; }
    public string? AccommodationType { get; set; }
}

public sealed class ItineraryInput
{
    public int Id { get; set; }
    [Required] public string SeasonTitle { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public List<ItineraryDayInput> Days { get; set; } = [];
}

public sealed class CostItemInput
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public bool IsActive { get; set; } = true;
    [Required] public string Type { get; set; } = "Inclusion";
    public int SortOrder { get; set; }
}

public sealed class FixedDepartureInput
{
    public int Id { get; set; }
    [DataType(DataType.Date)] public DateTime StartDate { get; set; }
    [DataType(DataType.Date)] public DateTime EndDate { get; set; }
    public int ForDays { get; set; }
    public string Status { get; set; } = "BookingOpen";
    public int? GroupSize { get; set; }
}

public sealed class GearListInput
{
    public int Id { get; set; }
    public string? ExistingPath { get; set; }
    public IFormFile? UploadFile { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? UploadImage { get; set; }
    public string? ShortDescription { get; set; }
}

public sealed class MapInput
{
    public int Id { get; set; }
    public string? ExistingPath { get; set; }
    public IFormFile? UploadFile { get; set; }
    public string? Title { get; set; }
    public string? Notes { get; set; }
}

public sealed class MediaInput
{
    public int Id { get; set; }
    public string? ExistingPath { get; set; }
    public IFormFile? PhotoFile { get; set; }
    public string? VideoUrl { get; set; }
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}

public sealed class ReviewInput
{
    public int Id { get; set; }
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string EmailAddress { get; set; } = string.Empty;
    public string? ExistingPhotoPath { get; set; }
    public IFormFile? UserPhoto { get; set; }
    public string? VideoUrl { get; set; }
    [Range(1, 5)] public int Rating { get; set; } = 5;
    [Required] public string ReviewText { get; set; } = string.Empty;
    public string ModerationStatus { get; set; } = "Pending";
}

public sealed class HighlightInput
{
    public int Id { get; set; }
    [Required] public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public sealed class ExpeditionFormViewModel : IValidatableObject
{
    public int? Id { get; set; }
    [Required, StringLength(200)] public string Name { get; set; } = string.Empty;
    [StringLength(220)] public string? Slug { get; set; }
    [Required, StringLength(600)] public string ShortDescription { get; set; } = string.Empty;
    [Required, StringLength(200)] public string Destination { get; set; } = string.Empty;
    [StringLength(150)] public string? Region { get; set; }
    [Range(1, 365)] public int DurationDays { get; set; } = 60;
    [Range(0, 12000)] public int MaxAltitudeMeters { get; set; } = 8849;
    [Required, StringLength(100)] public string Difficulty { get; set; } = "Hard";
    public string? DifficultyLevel { get; set; }
    public string? BestSeason { get; set; }
    public string? Overview { get; set; }
    public string? OverviewCountry { get; set; }
    public string? PeakName { get; set; }
    public string? OverviewDuration { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? WeatherReport { get; set; }
    public string? Range { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? HeroImageUrl { get; set; }
    public IFormFile? HeroImageFile { get; set; }
    public string? Permits { get; set; }
    [Range(1, 100)] public int MinGroupSize { get; set; } = 1;
    [Range(1, 100)] public int MaxGroupSize { get; set; } = 20;
    public string? GroupSizeText { get; set; }
    [Range(0, 999999)] public decimal Price { get; set; }
    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    [Required] public string Status { get; set; } = TravelStatus.Draft;
    public bool Featured { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public string? SummitRoute { get; set; }
    public bool RequiresClimbingPermit { get; set; }
    public string? ExpeditionStyle { get; set; }
    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }
    public decimal? SummitBonusUsd { get; set; }
    public string? WalkingPerDay { get; set; }
    public string? Accommodation { get; set; }
    [Required] public int? ExpeditionTypeId { get; set; }

    public List<ItineraryInput> Itineraries { get; set; } = [];
    public List<CostItemInput> CostItems { get; set; } = [];
    public List<FixedDepartureInput> FixedDepartures { get; set; } = [];
    public List<GearListInput> GearLists { get; set; } = [];
    public List<MapInput> Maps { get; set; } = [];
    public List<MediaInput> Media { get; set; } = [];
    public List<ReviewInput> Reviews { get; set; } = [];
    public List<HighlightInput> Highlights { get; set; } = [];

    public string SectionsText { get; set; } = string.Empty;
    public string ItineraryText { get; set; } = string.Empty;
    public string FaqsText { get; set; } = string.Empty;
    public string ReviewsText { get; set; } = string.Empty;
    public string MediaText { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(HeroImageUrl) && HeroImageFile is null)
        {
            yield return new ValidationResult("Provide at least one primary media source: URL (image/video) or uploaded image.", [nameof(HeroImageUrl), nameof(HeroImageFile)]);
        }

        foreach (var fd in FixedDepartures)
        {
            if (fd.EndDate < fd.StartDate)
            {
                yield return new ValidationResult("Departure EndDate must be greater than or equal to StartDate.", [nameof(FixedDepartures)]);
            }
        }

        foreach (var itinerary in Itineraries)
        {
            var duplicateDay = itinerary.Days.GroupBy(x => x.DayNumber).FirstOrDefault(g => g.Key < 1 || g.Count() > 1);
            if (duplicateDay is not null)
            {
                yield return new ValidationResult("Itinerary day numbers must be unique per itinerary and greater than or equal to 1.", [nameof(Itineraries)]);
            }
        }
    }
}
