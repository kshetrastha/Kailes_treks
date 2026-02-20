using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities;

public enum TrekkingInclusionType
{
    Inclusion = 1,
    Exclusion = 2
}

public enum TrekkingDepartureStatus
{
    BookingOpen = 1,
    Limited = 2,
    SoldOut = 3,
    Closed = 4
}

public enum TrekkingGearCategory
{
    Clothing = 1,
    Equipment = 2,
    Technical = 3,
    Personal = 4
}

public sealed class TrekkingBasicInfo : BaseEntity
{
    public int ExpeditionTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Moderate;
    public int? MaxElevation { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string? WalkingHoursPerDay { get; set; }
    public string? Accommodation { get; set; }
    public string? BestSeason { get; set; }
    public string? GroupSize { get; set; }
    public bool IsFeatured { get; set; }
    public string? BannerImagePath { get; set; }
    public string? ThumbnailImagePath { get; set; }

    public ExpeditionType ExpeditionType { get; set; } = default!;
    public TrekkingOverview? Overview { get; set; }
    public List<TrekkingItinerary> Itineraries { get; set; } = [];
    public List<TrekkingInclusionExclusion> InclusionExclusions { get; set; } = [];
    public List<TrekkingFixedDeparture> FixedDepartures { get; set; } = [];
    public List<TrekkingGear> GearItems { get; set; } = [];
    public List<TrekkingReviewItem> Reviews { get; set; } = [];
    public List<TrekkingFaqItem> Faqs { get; set; } = [];
}

public sealed class TrekkingOverview : BaseEntity
{
    public int TrekkingId { get; set; }
    public string? Country { get; set; }
    public string? PeakName { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }
    public string? Range { get; set; }
    public string? Coordinates { get; set; }
    public string? WeatherInformation { get; set; }
    public string? FullDescription { get; set; }
    public string? MapEmbedCode { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingItinerary : BaseEntity
{
    public int TrekkingId { get; set; }
    public string SeasonTitle { get; set; } = string.Empty;
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Accommodation { get; set; }
    public string? Meals { get; set; }
    public string? Elevation { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingInclusionExclusion : BaseEntity
{
    public int TrekkingId { get; set; }
    public TrekkingInclusionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingFixedDeparture : BaseEntity
{
    public int TrekkingId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalSeats { get; set; }
    public int BookedSeats { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public TrekkingDepartureStatus Status { get; set; } = TrekkingDepartureStatus.BookingOpen;
    public bool IsGuaranteed { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingGear : BaseEntity
{
    public int TrekkingId { get; set; }
    public TrekkingGearCategory Category { get; set; } = TrekkingGearCategory.Equipment;
    public string ItemName { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public int DisplayOrder { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingReviewItem : BaseEntity
{
    public int TrekkingId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? Country { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public bool IsApproved { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}

public sealed class TrekkingFaqItem : BaseEntity
{
    public int TrekkingId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public TrekkingBasicInfo Trekking { get; set; } = default!;
}
