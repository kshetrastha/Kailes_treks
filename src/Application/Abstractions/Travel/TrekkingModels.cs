namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record TrekkingListItemDto(int Id, string Name, string Slug, string Destination, int DurationDays, decimal Price, string Status, bool Featured, int Ordering);
public sealed record TrekkingItineraryDayDto(int DayNumber, string Title, string? Description, string? OvernightLocation);
public sealed record TrekkingFaqDto(string Question, string Answer, int Ordering);
public sealed record TrekkingMediaDto(string Url, string? Caption, string MediaType, int Ordering);

public sealed record TrekkingUpsertDto(
    string Name,
    string? Slug,
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
    string? TrailGrade,
    bool TeaHouseAvailable,
    string? AccommodationType,
    string? Meals,
    string? TransportMode,
    string? TrekPermitType,
    IReadOnlyCollection<TrekkingItineraryDayDto> ItineraryDays,
    IReadOnlyCollection<TrekkingFaqDto> Faqs,
    IReadOnlyCollection<TrekkingMediaDto> MediaItems);

public sealed record TrekkingPagedResult(IReadOnlyCollection<TrekkingListItemDto> Items, int Page, int PageSize, int TotalCount);
