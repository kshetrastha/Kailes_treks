using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Entities.Expeditions;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities;

public enum ExpeditionInclusionType
{
    Inclusion = 1,
    Exclusion = 2
}

public enum ExpeditionDepartureStatus
{
    BookingOpen = 1,
    Limited = 2,
    SoldOut = 3,
    Closed = 4
}

public enum ExpeditionGearCategory
{
    Clothing = 1,
    Equipment = 2,
    Technical = 3,
    Personal = 4
}

public sealed class ExpeditionBasicInfo : BaseEntity
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
    public ExpeditionOverview? Overview { get; set; }
    public List<ExpeditionItinerary> Itineraries { get; set; } = [];
    public List<ExpeditionInclusionExclusion> InclusionExclusions { get; set; } = [];
    public List<ExpeditionFixedDeparture> FixedDepartures { get; set; } = [];
    public List<ExpeditionGear> GearItems { get; set; } = [];
    public List<ExpeditionReviewItem> Reviews { get; set; } = [];
    public List<ExpeditionFaqItem> Faqs { get; set; } = [];
}

public sealed class ExpeditionOverview : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string? Country { get; set; }
    public string? PeakName { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }
    public string? Range { get; set; }
    public string? Coordinates { get; set; }
    public string? WeatherInformation { get; set; }
    public string? FullDescription { get; set; }
    public string? MapEmbedCode { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionItinerary : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string SeasonTitle { get; set; } = string.Empty;
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Accommodation { get; set; }
    public string? Meals { get; set; }
    public string? Elevation { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionInclusionExclusion : BaseEntity
{
    public int ExpeditionId { get; set; }
    public ExpeditionInclusionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionFixedDeparture : BaseEntity
{
    public int ExpeditionId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalSeats { get; set; }
    public int BookedSeats { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public ExpeditionDepartureStatus Status { get; set; } = ExpeditionDepartureStatus.BookingOpen;
    public bool IsGuaranteed { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionGear : BaseEntity
{
    public int ExpeditionId { get; set; }
    public ExpeditionGearCategory Category { get; set; } = ExpeditionGearCategory.Equipment;
    public string ItemName { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public int DisplayOrder { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionReviewItem : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? Country { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public bool IsApproved { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}

public sealed class ExpeditionFaqItem : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public ExpeditionBasicInfo Expedition { get; set; } = default!;
}
