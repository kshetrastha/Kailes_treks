using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record TrekkingBasicInfoListItemDto(int Id, string Name, string DifficultyLevel, string Duration, bool IsFeatured);

public sealed record TrekkingBasicInfoUpsertDto(
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

public sealed record TrekkingOverviewUpsertDto(
    string? Country,
    string? PeakName,
    string? Route,
    string? Rank,
    string? Range,
    string? Coordinates,
    string? WeatherInformation,
    string? FullDescription,
    string? MapEmbedCode);

public sealed record TrekkingItineraryUpsertDto(string SeasonTitle, int DayNumber, string Title, string? ShortDescription, string? FullDescription, string? Accommodation, string? Meals, string? Elevation);
public sealed record TrekkingInclusionExclusionUpsertDto(string Type, string Description, int DisplayOrder);
public sealed record TrekkingFixedDepartureUpsertDto(DateOnly StartDate, DateOnly EndDate, int TotalSeats, int BookedSeats, decimal Price, string Currency, string Status, bool IsGuaranteed);
public sealed record TrekkingGearUpsertDto(string Category, string ItemName, bool IsMandatory, int DisplayOrder);
public sealed record TrekkingReviewUpsertDto(string ClientName, string? Country, [Range(1,5)] int Rating, string? Title, string Comment, string? ImagePath, bool IsApproved);
public sealed record TrekkingFaqUpsertDto(string Question, string Answer, int DisplayOrder);

public sealed record TrekkingModuleDetailsDto(
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
    TrekkingOverviewUpsertDto? Overview,
    IReadOnlyCollection<TrekkingItineraryUpsertDto> Itineraries,
    IReadOnlyCollection<TrekkingInclusionExclusionUpsertDto> InclusionExclusions,
    IReadOnlyCollection<TrekkingFixedDepartureUpsertDto> FixedDepartures,
    IReadOnlyCollection<TrekkingGearUpsertDto> Gears,
    IReadOnlyCollection<TrekkingReviewUpsertDto> Reviews,
    IReadOnlyCollection<TrekkingFaqUpsertDto> Faqs);
