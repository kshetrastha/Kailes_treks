namespace TravelCleanArch.Domain.Entities;

public sealed class Expedition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? Region { get; set; }
    public int DurationDays { get; set; }
    public int MaxAltitudeMeters { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string? BestSeason { get; set; }
    public string? Overview { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? HeroImageUrl { get; set; }
    public string? Permits { get; set; }
    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }
    public decimal Price { get; set; }
    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string Status { get; set; } = TravelStatus.Draft;
    public bool Featured { get; set; }
    public int Ordering { get; set; }
    public string? SummitRoute { get; set; }
    public bool RequiresClimbingPermit { get; set; }
    public string? ExpeditionStyle { get; set; }
    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }
    public decimal? SummitBonusUsd { get; set; }   

    public List<ExpeditionItineraryDay> ItineraryDays { get; set; } = [];
    public List<ExpeditionFaq> Faqs { get; set; } = [];
    public List<ExpeditionMedia> MediaItems { get; set; } = [];
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

    public Expedition Expedition { get; set; } = default!;
}
