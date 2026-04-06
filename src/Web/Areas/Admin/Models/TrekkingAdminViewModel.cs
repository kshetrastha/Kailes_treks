using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class TrekkingAdminViewModel
{
    [Display(Name = "Id")]
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Trekking Type")]
    public int? TrekkingTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Package Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Display(Name = "Short Description")]
    public string ShortDescription { get; set; } = string.Empty;

    [Display(Name = "Featured")]
    public bool Featured { get; set; }

    [Display(Name = "Display Order")]
    public int Ordering { get; set; }

    [Required]
    [Display(Name = "Destination")]
    public string Destination { get; set; } = string.Empty;

    [Display(Name = "Region")]
    public string? Region { get; set; }

    [Display(Name = "Duration (Days)")]
    public int DurationDays { get; set; }

    [Display(Name = "Max Altitude (Meters)")]
    public int MaxAltitudeMeters { get; set; }

    [Display(Name = "Max Altitude (Feet)")]
    public int? MaxAltitudeFeet { get; set; }

    [Display(Name = "Difficulty Level")]
    public DifficultyLevel? DifficultyLevel { get; set; }

    [Display(Name = "Best Season")]
    public Season? BestSeason { get; set; }

    [Display(Name = "Walking Per Day")]
    public string? WalkingPerDay { get; set; }

    [Display(Name = "Accommodation")]
    public string? Accommodation { get; set; }

    [Display(Name = "Overview")]
    public string? Overview { get; set; }

    [Display(Name = "Country")]
    public Country OverviewCountry { get; set; } = Country.Nepal;

    [Display(Name = "Peak Name")]
    public string? PeakName { get; set; }

    [Display(Name = "Route")]
    public string? Route { get; set; }

    [Display(Name = "Rank")]
    public string? Rank { get; set; }

    [Display(Name = "Latitude")]
    public decimal? Latitude { get; set; }

    [Display(Name = "Longitude")]
    public decimal? Longitude { get; set; }

    [Display(Name = "Coordinates")]
    public string? CoordinatesText { get; set; }

    [Display(Name = "Weather Report URL")]
    public string? WeatherReportUrl { get; set; }

    [Display(Name = "Mountain Range")]
    public string? Range { get; set; }

    [Display(Name = "Hero Image URL")]
    public string? HeroImageUrl { get; set; }

    [Display(Name = "Hero Image")]
    public IFormFile? HeroImageFile { get; set; }

    [Display(Name = "Hero Video URL")]
    public string? HeroVideoUrl { get; set; }

    [Display(Name = "Min Group Size")]
    public int MinGroupSize { get; set; }

    [Display(Name = "Max Group Size")]
    public int MaxGroupSize { get; set; }

    [Display(Name = "Group Size Description")]
    public string? GroupSizeText { get; set; }

    [Display(Name = "Price On Request")]
    public bool PriceOnRequest { get; set; }

    [Display(Name = "Price")]
    public decimal? Price { get; set; }

    [Display(Name = "Currency Code")]
    public string? CurrencyCode { get; set; }

    [Display(Name = "Price Notes URL")]
    public string? PriceNotesUrl { get; set; }

    [Display(Name = "Trip PDF URL")]
    public string? TripPdfUrl { get; set; }

    [Display(Name = "SEO Title")]
    public string? SeoTitle { get; set; }

    [Display(Name = "SEO Description")]
    public string? SeoDescription { get; set; }

    [Display(Name = "Average Rating")]
    public decimal? AverageRating { get; set; }

    [Display(Name = "Rating Label")]
    public string? RatingLabel { get; set; }

    [Display(Name = "Review Count")]
    public int? ReviewCount { get; set; }

    [Display(Name = "Status")]
    public TravelStatus Status { get; set; } = TravelStatus.Draft;

    [Display(Name = "Expedition Style")]
    public string? ExpeditionStyle { get; set; }

    [Display(Name = "Board Basis")]
    public string? BoardBasis { get; set; }

    [Display(Name = "Oxygen Support")]
    public bool OxygenSupport { get; set; }

    [Display(Name = "Sherpa Support")]
    public bool SherpaSupport { get; set; }

    [Display(Name = "Summit Bonus (USD)")]
    public decimal? SummitBonusUsd { get; set; }

    [Display(Name = "Permits")]
    public string? Permits { get; set; }

    [Display(Name = "Requires Climbing Permit")]
    public bool RequiresClimbingPermit { get; set; }

    [Display(Name = "Inclusions")]
    public string? Inclusions { get; set; }

    [Display(Name = "Exclusions")]
    public string? Exclusions { get; set; }

    [Display(Name = "Available Dates")]
    public string? AvailableDates { get; set; }

    [Display(Name = "Booking URL")]
    public string? BookingCtaUrl { get; set; }

    [Display(Name = "Summit Route")]
    public string? SummitRoute { get; set; }

    [Display(Name = "Overview Duration")]
    public string? OverviewDuration { get; set; }
}