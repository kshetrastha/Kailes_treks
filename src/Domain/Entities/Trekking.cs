namespace TravelCleanArch.Domain.Entities;

public sealed class Trekking : BaseEntity
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
    public string Status { get; set; } = "draft";
    public bool Featured { get; set; }
    public int Ordering { get; set; }
    public string? TrailGrade { get; set; }
    public bool TeaHouseAvailable { get; set; }
    public string? AccommodationType { get; set; }
    public string? Meals { get; set; }
    public string? TransportMode { get; set; }
    public string? TrekPermitType { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }

    public List<TrekkingItineraryDay> ItineraryDays { get; set; } = [];
    public List<TrekkingFaq> Faqs { get; set; } = [];
    public List<TrekkingMedia> MediaItems { get; set; } = [];
}

public sealed class TrekkingItineraryDay : BaseEntity
{
    public int TrekkingId { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OvernightLocation { get; set; }

    public Trekking Trekking { get; set; } = default!;
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

    public Trekking Trekking { get; set; } = default!;
}
