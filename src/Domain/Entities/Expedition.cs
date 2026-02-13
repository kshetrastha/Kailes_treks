using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Domain.Entities;

public enum DifficultyLevel
{
    Easy = 1,
    Moderate = 2,
    Difficult = 3,
    [Display(Name = "Hard Difficult")]
    HardDifficult = 4,
    [Display(Name = "Very Hard")]
    VeryHard = 5
}

public enum CostItemType
{
    Inclusion = 1,
    Exclusion = 2
}

public enum DepartureStatus
{
    BookingOpen = 1,
    BookingClosed = 2,
    BookingHold = 3
}

public enum ReviewModerationStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum ExpeditionMediaType
{
    Photo = 1,
    Video = 2
}

public sealed class Expedition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? Region { get; set; }
    public int DurationDays { get; set; }
    public int MaxAltitudeMeters { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public DifficultyLevel? DifficultyLevel { get; set; }
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
    public string? Permits { get; set; }
    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }
    public string? GroupSizeText { get; set; }
    public decimal Price { get; set; }
    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string Status { get; set; } = TravelStatus.Published;
    public bool Featured { get; set; }
    public int Ordering { get; set; }
    public string? SummitRoute { get; set; }
    public bool RequiresClimbingPermit { get; set; }
    public string? ExpeditionStyle { get; set; }
    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }
    public decimal? SummitBonusUsd { get; set; }
    public string? WalkingPerDay { get; set; }
    public string? Accommodation { get; set; }
    public int? ExpeditionTypeId { get; set; }

    public ExpeditionType? ExpeditionType { get; set; }
    public List<ExpeditionSection> Sections { get; set; } = [];
    public List<ExpeditionItineraryDay> ItineraryDays { get; set; } = [];
    public List<ExpeditionFaq> Faqs { get; set; } = [];
    public List<ExpeditionMedia> MediaItems { get; set; } = [];

    public List<Itinerary> Itineraries { get; set; } = [];
    public List<ExpeditionMap> Maps { get; set; } = [];
    public List<CostItem> CostItems { get; set; } = [];
    public List<FixedDeparture> FixedDepartures { get; set; } = [];
    public List<GearList> GearLists { get; set; } = [];
    public List<ExpeditionHighlight> Highlights { get; set; } = [];
    public List<ExpeditionReview> Reviews { get; set; } = [];
}

public sealed class ExpeditionType : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<Expedition> Expeditions { get; set; } = [];
    public List<ExpeditionTypeImage> Images { get; set; } = [];
}

public sealed class ExpeditionTypeImage : BaseEntity
{
    public int ExpeditionTypeId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }

    public ExpeditionType ExpeditionType { get; set; } = default!;
}

public sealed class Itinerary : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string SeasonTitle { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Expedition Expedition { get; set; } = default!;
    public List<ItineraryDay> Days { get; set; } = [];
}

public sealed class ItineraryDay : BaseEntity
{
    public int ItineraryId { get; set; }
    public int DayNumber { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? Meals { get; set; }
    public string? AccommodationType { get; set; }

    public Itinerary Itinerary { get; set; } = default!;
}

public sealed class ExpeditionMap : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Notes { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class CostItem : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public CostItemType Type { get; set; }
    public int SortOrder { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class FixedDeparture : BaseEntity
{
    public int ExpeditionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ForDays { get; set; }
    public DepartureStatus Status { get; set; } = DepartureStatus.BookingOpen;
    public int? GroupSize { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class GearList : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string? ShortDescription { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionSection : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string SectionType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public int Ordering { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionItineraryDay : BaseEntity
{
    public int ExpeditionId { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OvernightLocation { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionFaq : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Ordering { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionMedia : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public string MediaType { get; set; } = "image";
    public int Ordering { get; set; }
    public string? FilePath { get; set; }
    public string? VideoUrl { get; set; }
    public ExpeditionMediaType MediaKind { get; set; } = ExpeditionMediaType.Photo;

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionHighlight : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Expedition Expedition { get; set; } = default!;
}

public sealed class ExpeditionReview : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? UserPhotoPath { get; set; }
    public string? VideoUrl { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public ReviewModerationStatus ModerationStatus { get; set; } = ReviewModerationStatus.Pending;

    public Expedition Expedition { get; set; } = default!;
}
