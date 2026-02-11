using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Contracts.Travel;

public sealed record ExpeditionItineraryDayRequest(int DayNumber, string Title, string? Description, string? OvernightLocation);
public sealed record ExpeditionFaqRequest(string Question, string Answer, int Ordering);
public sealed record ExpeditionMediaRequest(string Url, string? Caption, string MediaType, int Ordering);

public sealed record TrekkingItineraryDayRequest(int DayNumber, string Title, string? Description, string? OvernightLocation);
public sealed record TrekkingFaqRequest(string Question, string Answer, int Ordering);
public sealed record TrekkingMediaRequest(string Url, string? Caption, string MediaType, int Ordering);

public sealed record ExpeditionUpsertRequest(
    [Required] string Name,
    string? Slug,
    [Required] string Destination,
    string? Region,
    [Range(1, 365)] int DurationDays,
    int MaxAltitudeMeters,
    [Required] string Difficulty,
    string? BestSeason,
    string? Overview,
    string? Inclusions,
    string? Exclusions,
    string? HeroImageUrl,
    string? Permits,
    int MinGroupSize,
    int MaxGroupSize,
    decimal Price,
    string? AvailableDates,
    string? BookingCtaUrl,
    string? SeoTitle,
    string? SeoDescription,
    string Status,
    bool Featured,
    int Ordering,
    string? SummitRoute,
    bool RequiresClimbingPermit,
    string? ExpeditionStyle,
    bool OxygenSupport,
    bool SherpaSupport,
    decimal? SummitBonusUsd,
    IReadOnlyCollection<ExpeditionItineraryDayRequest>? ItineraryDays,
    IReadOnlyCollection<ExpeditionFaqRequest>? Faqs,
    IReadOnlyCollection<ExpeditionMediaRequest>? MediaItems)
{
    public string NormalizedStatus => string.IsNullOrWhiteSpace(Status) ? TravelStatus.Draft : Status.ToLowerInvariant();
}

public sealed record TrekkingUpsertRequest(
    [Required] string Name,
    string? Slug,
    [Required] string Destination,
    string? Region,
    [Range(1, 365)] int DurationDays,
    int MaxAltitudeMeters,
    [Required] string Difficulty,
    string? BestSeason,
    string? Overview,
    string? Inclusions,
    string? Exclusions,
    string? HeroImageUrl,
    string? Permits,
    int MinGroupSize,
    int MaxGroupSize,
    decimal Price,
    string? AvailableDates,
    string? BookingCtaUrl,
    string? SeoTitle,
    string? SeoDescription,
    string Status,
    bool Featured,
    int Ordering,
    string? TrailGrade,
    bool TeaHouseAvailable,
    string? AccommodationType,
    string? Meals,
    string? TransportMode,
    string? TrekPermitType,
    IReadOnlyCollection<TrekkingItineraryDayRequest>? ItineraryDays,
    IReadOnlyCollection<TrekkingFaqRequest>? Faqs,
    IReadOnlyCollection<TrekkingMediaRequest>? MediaItems)
{
    public string NormalizedStatus => string.IsNullOrWhiteSpace(Status) ? TravelStatus.Draft : Status.ToLowerInvariant();
}
