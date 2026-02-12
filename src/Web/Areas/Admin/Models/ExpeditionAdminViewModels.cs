using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class ExpeditionTypeFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(600)] public string ShortDescription { get; set; } = string.Empty;
    [StringLength(4000)] public string? Description { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class ExpeditionFormViewModel
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
    public string? BestSeason { get; set; }
    public string? Overview { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? HeroImageUrl { get; set; }
    public string? Permits { get; set; }
    [Range(1, 100)] public int MinGroupSize { get; set; } = 1;
    [Range(1, 100)] public int MaxGroupSize { get; set; } = 20;
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
    public int? ExpeditionTypeId { get; set; }

    [Display(Name = "Sections (one per line: sectionType|title|content|ordering)")]
    public string SectionsText { get; set; } = string.Empty;

    [Display(Name = "Itinerary (one per line: dayNumber|title|description|overnightLocation)")]
    public string ItineraryText { get; set; } = string.Empty;

    [Display(Name = "FAQs (one per line: question|answer|ordering)")]
    public string FaqsText { get; set; } = string.Empty;

    [Display(Name = "Media (one per line: url|caption|mediaType|ordering)")]
    public string MediaText { get; set; } = string.Empty;
}
