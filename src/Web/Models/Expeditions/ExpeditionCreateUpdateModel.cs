using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class ExpeditionCreateUpdateModel
{
    public int? Id { get; set; }   // null = Create, value = Update

    // =========================
    // Basic Info
    // =========================
    [Required]
    public int? ExpeditionTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string ShortDescription { get; set; } = string.Empty;

    public bool Featured { get; set; }
    public int Ordering { get; set; }

    // =========================
    // Trip Facts
    // =========================
    [Required]
    public string Destination { get; set; } = string.Empty;

    public string? Region { get; set; }

    public int DurationDays { get; set; }

    public int MaxAltitudeMeters { get; set; }
    public int? MaxAltitudeFeet { get; set; }

    public DifficultyLevel? DifficultyLevel { get; set; }
    public Season? BestSeason { get; set; }

    public string? WalkingPerDay { get; set; }
    public string? Accommodation { get; set; }

    // =========================
    // Overview Section
    // =========================
    public string? Overview { get; set; }

    public Country OverviewCountry { get; set; } = Country.Nepal;

    public string? PeakName { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? CoordinatesText { get; set; }

    public string? WeatherReportUrl { get; set; }
    public string? Range { get; set; }

    // =========================
    // Hero Media
    // =========================
    public string? HeroImageUrl { get; set; }
    public IFormFile? HeroImageFile { get; set; }   // upload support

    public string? HeroVideoUrl { get; set; }

    // =========================
    // Group Size
    // =========================
    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }
    public string? GroupSizeText { get; set; }

    // =========================
    // Pricing
    // =========================
    public bool PriceOnRequest { get; set; }
    public decimal? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PriceNotesUrl { get; set; }
    public string? TripPdfUrl { get; set; }

    // =========================
    // SEO
    // =========================
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

    // =========================
    // Rating
    // =========================
    public decimal? AverageRating { get; set; }
    public string? RatingLabel { get; set; }
    public int? ReviewCount { get; set; }

    // =========================
    // Extra Details
    // =========================
    public TravelStatus Status { get; set; } = TravelStatus.Draft;

    public string? ExpeditionStyle { get; set; }
    public string? BoardBasis { get; set; }

    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }

    public decimal? SummitBonusUsd { get; set; }

    public string? Permits { get; set; }
    public bool RequiresClimbingPermit { get; set; }

    // =========================
    // Legacy / Extra Fields
    // =========================
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SummitRoute { get; set; }
    public string? OverviewDuration { get; set; }

    // =========================
    // Child Collections
    // =========================
    public List<ExpeditionFaqCreateUpdateModel> Faqs { get; set; } = new();
    public List<ExpeditionMediaCreateUpdateModel> Photos { get; set; } = new();
    public List<ItineraryCreateUpdateModel> Itineraries { get; set; } = new();
    public List<ItineraryDayCreateUpdateModel> ItineraryDays { get; set; } = new();
    public List<ExpeditionMapCreateUpdateModel> ExpeditionMaps { get; set; } = new();
    public List<CostItemCreateUpdateModel> CostItems { get; set; } = new();
    public List<FixedDepartureCreateUpdateModel> FixedDepartures { get; set; } = new();
    public List<GearListCreateUpdateModel> GearLists { get; set; } = new();
    public List<ExpeditionHighlightCreateUpdateModel> Highlights { get; set; } = new();
    public List<ExpeditionReviewCreateUpdateModel> Reviews { get; set; } = new();
}
