using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record ExpeditionBasicInfoListItemDto(int Id, string Name, string DifficultyLevel, string Duration, bool IsFeatured);

public sealed record ExpeditionBasicInfoUpsertDto(
    int ExpeditionTypeId,
    string Name,
    string ShortDescription,
    string DifficultyLevel,
    int? MaxElevation,
    string Duration,
    string? WalkingHoursPerDay,
    string? Accommodation,
    string? BestSeason,
    string? GroupSize,
    bool IsFeatured,
    string? BannerImagePath,
    string? ThumbnailImagePath);

public sealed record ExpeditionOverviewUpsertDto(
    string? Country,
    string? PeakName,
    string? Route,
    string? Rank,
    string? Range,
    string? Coordinates,
    string? WeatherInformation,
    string? FullDescription,
    string? MapEmbedCode);

public sealed record ExpeditionItineraryUpsertDto(string SeasonTitle, int DayNumber, string Title, string? ShortDescription, string? FullDescription, string? Accommodation, string? Meals, string? Elevation);
public sealed record ExpeditionInclusionExclusionUpsertDto(string Type, string Description, int DisplayOrder);
public sealed record ExpeditionFixedDepartureUpsertDto(DateOnly StartDate, DateOnly EndDate, int TotalSeats, int BookedSeats, decimal Price, string Currency, string Status, bool IsGuaranteed);
public sealed record ExpeditionGearUpsertDto(string Category, string ItemName, bool IsMandatory, int DisplayOrder);
public sealed record ExpeditionReviewUpsertDto(string ClientName, string? Country, [Range(1,5)] int Rating, string? Title, string Comment, string? ImagePath, bool IsApproved);
public sealed record ExpeditionFaqUpsertDto(string Question, string Answer, int DisplayOrder);

public sealed record ExpeditionModuleDetailsDto(
    int Id,
    int ExpeditionTypeId,
    string Name,
    string ShortDescription,
    string DifficultyLevel,
    int? MaxElevation,
    string Duration,
    string? WalkingHoursPerDay,
    string? Accommodation,
    string? BestSeason,
    string? GroupSize,
    bool IsFeatured,
    string? BannerImagePath,
    string? ThumbnailImagePath,
    ExpeditionOverviewUpsertDto? Overview,
    IReadOnlyCollection<ExpeditionItineraryUpsertDto> Itineraries,
    IReadOnlyCollection<ExpeditionInclusionExclusionUpsertDto> InclusionExclusions,
    IReadOnlyCollection<ExpeditionFixedDepartureUpsertDto> FixedDepartures,
    IReadOnlyCollection<ExpeditionGearUpsertDto> Gears,
    IReadOnlyCollection<ExpeditionReviewUpsertDto> Reviews,
    IReadOnlyCollection<ExpeditionFaqUpsertDto> Faqs);
