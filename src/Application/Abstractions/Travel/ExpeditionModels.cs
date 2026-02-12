namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record ExpeditionTypeDto(int Id, string Title, string ShortDescription, string? Description, string? ImagePath, int Ordering, bool IsPublished);
public sealed record ExpeditionTypeUpsertDto(string Title, string ShortDescription, string? Description, string? ImagePath, int Ordering, bool IsPublished);

public sealed record ExpeditionListItemDto(
    int Id,
    string Name,
    string HeroImageUrl,
    string Slug,
    string ShortDescription,
    string Destination,
    int DurationDays,
    decimal Price,
    string Status,
    bool Featured,
    int Ordering,
    int? ExpeditionTypeId,
    string? ExpeditionTypeTitle);

public sealed record ExpeditionSectionDto(string SectionType, string Title, string? Content, int Ordering);
public sealed record ExpeditionItineraryDayDto(int DayNumber, string Title, string? Description, string? OvernightLocation);
public sealed record ExpeditionFaqDto(string Question, string Answer, int Ordering);
public sealed record ExpeditionMediaDto(string Url, string? Caption, string MediaType, int Ordering);

public sealed record ExpeditionUpsertDto(
    string Name,
    string? Slug,
    string ShortDescription,
    string Destination,
    string? Region,
    int DurationDays,
    int MaxAltitudeMeters,
    string Difficulty,
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
    int? ExpeditionTypeId,
    IReadOnlyCollection<ExpeditionSectionDto> Sections,
    IReadOnlyCollection<ExpeditionItineraryDayDto> ItineraryDays,
    IReadOnlyCollection<ExpeditionFaqDto> Faqs,
    IReadOnlyCollection<ExpeditionMediaDto> MediaItems);

public sealed record ExpeditionDetailsDto(
    int Id,
    string Name,
    string Slug,
    string ShortDescription,
    string Destination,
    string? Region,
    int DurationDays,
    int MaxAltitudeMeters,
    string Difficulty,
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
    int? ExpeditionTypeId,
    string? ExpeditionTypeTitle,
    IReadOnlyCollection<ExpeditionSectionDto> Sections,
    IReadOnlyCollection<ExpeditionItineraryDayDto> ItineraryDays,
    IReadOnlyCollection<ExpeditionFaqDto> Faqs,
    IReadOnlyCollection<ExpeditionMediaDto> MediaItems);

public sealed record ExpeditionPagedResult(IReadOnlyCollection<ExpeditionListItemDto> Items, int Page, int PageSize, int TotalCount);
