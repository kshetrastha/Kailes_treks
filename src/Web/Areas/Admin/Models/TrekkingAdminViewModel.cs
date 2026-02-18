using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class TrekkingAdminViewModel
{
    public int? Id { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(220)]
    public string? Slug { get; set; }

    [MaxLength(600)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Destination { get; set; } = string.Empty;

    public string? Region { get; set; }
    public int DurationDays { get; set; }
    public int MaxAltitudeMeters { get; set; }
    public int? MaxAltitudeFeet { get; set; }

    public DifficultyLevel? DifficultyLevel { get; set; }
    public Season? BestSeason { get; set; }

    public string? Overview { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? HeroImageUrl { get; set; }
    public string? HeroVideoUrl { get; set; }
    public string? Permits { get; set; }

    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }

    public bool PriceOnRequest { get; set; }
    public decimal? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PriceNotesUrl { get; set; }
    public string? TripPdfUrl { get; set; }

    public string? AvailableDates { get; set; }
    public string? BookingCtaUrl { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

    public TravelStatus Status { get; set; } = TravelStatus.Draft;
    public bool Featured { get; set; }
    public int Ordering { get; set; }

    public int? TrekkingTypeId { get; set; }
}
