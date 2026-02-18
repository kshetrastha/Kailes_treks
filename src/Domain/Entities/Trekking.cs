using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities;

public sealed class Trekking : BaseEntity
{
    // FK
    public int? TrekkingTypeId { get; set; }
    public TrekkingType? TrekkingType { get; set; }

    // Core
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;

    // Trip facts
    public string Destination { get; set; } = string.Empty;
    public string? Region { get; set; }
    public int DurationDays { get; set; }
    public int MaxAltitudeMeters { get; set; }
    public int? MaxAltitudeFeet { get; set; }
    public DifficultyLevel? DifficultyLevel { get; set; }
    public Season? BestSeason { get; set; }
    public string? WalkingPerDay { get; set; }
    public string? Accommodation { get; set; }

    // Overview details
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

    // Hero / media
    public string? HeroImageUrl { get; set; }
    public string? HeroVideoUrl { get; set; }

    // Group
    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }
    public string? GroupSizeText { get; set; }

    // Price
    public bool PriceOnRequest { get; set; }
    public decimal? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PriceNotesUrl { get; set; }
    public string? TripPdfUrl { get; set; }

    // SEO
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

    // Rating
    public decimal? AverageRating { get; set; }
    public string? RatingLabel { get; set; }
    public int? ReviewCount { get; set; }

    // Flags/details
    public TravelStatus Status { get; set; } = TravelStatus.Draft;
    public bool Featured { get; set; }
    public int Ordering { get; set; }
    public string? ExpeditionStyle { get; set; }
    public string? BoardBasis { get; set; }
    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }
    public decimal? SummitBonusUsd { get; set; }
    public string? Permits { get; set; }
    public bool RequiresClimbingPermit { get; set; }

    public List<TrekkingFaq> Faqs { get; set; } = [];
    public List<TrekkingMedia> MediaItems { get; set; } = [];
    public List<TrekkingItinerary> Itineraries { get; set; } = [];
    public List<TrekkingItineraryDay> ItineraryDays { get; set; } = [];
    public List<TrekkingMap> Maps { get; set; } = [];
    public List<TrekkingCostItem> CostItems { get; set; } = [];
    public List<TrekkingFixedDeparture> FixedDepartures { get; set; } = [];
    public List<TrekkingGearList> GearLists { get; set; } = [];
    public List<TrekkingHighlight> Highlights { get; set; } = [];
    public List<TrekkingReview> Reviews { get; set; } = [];

    // Backward-compatible aliases.
    public string Difficulty
    {
        get => DifficultyLevel?.ToString() ?? string.Empty;
        set
        {
            if (Enum.TryParse<DifficultyLevel>(value, true, out var parsed))
            {
                DifficultyLevel = parsed;
            }
        }
    }

    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SummitRoute { get; set; }
    public string? OverviewDuration { get; set; }

    public string? WeatherReport
    {
        get => WeatherReportUrl;
        set => WeatherReportUrl = value;
    }
}

public sealed class TrekkingFaq : BaseEntity
{
    public int TrekkingId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Ordering { get; set; }

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingMedia : BaseEntity
{
    public int TrekkingId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public string MediaType { get; set; } = "image";
    public int Ordering { get; set; }
    public string? FilePath { get; set; }
    public string? VideoUrl { get; set; }
    public ExpeditionMediaType MediaKind { get; set; } = ExpeditionMediaType.Photo;

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingItinerary : BaseEntity
{
    public int TrekkingId { get; set; }
    public string SeasonTitle { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Trekking Trekking { get; set; } = default!;
    public List<TrekkingItineraryDay> Days { get; set; } = [];
}

public sealed class TrekkingItineraryDay : BaseEntity
{
    public int TrekkingItineraryId { get; set; }
    public int DayNumber { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? Meals { get; set; }
    public string? AccommodationType { get; set; }

    public TrekkingItinerary TrekkingItinerary { get; set; } = default!;
}

public sealed class TrekkingMap : BaseEntity
{
    public int TrekkingId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Notes { get; set; }

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingCostItem : BaseEntity
{
    public int TrekkingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public CostItemType Type { get; set; }
    public int SortOrder { get; set; }

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingFixedDeparture : BaseEntity
{
    public int TrekkingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ForDays { get; set; }
    public DepartureStatus Status { get; set; } = DepartureStatus.BookingCreated;
    public int? GroupSize { get; set; }

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingGearList : BaseEntity
{
    public int TrekkingId { get; set; }
    public string? ShortDescription { get; set; }
    public string? ImagePath { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingHighlight : BaseEntity
{
    public int TrekkingId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Trekking Trekking { get; set; } = default!;
}

public sealed class TrekkingReview : BaseEntity
{
    public int TrekkingId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? UserPhotoPath { get; set; }
    public string? VideoUrl { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public ReviewModerationStatus ModerationStatus { get; set; } = ReviewModerationStatus.Pending;

    public Trekking Trekking { get; set; } = default!;
}
