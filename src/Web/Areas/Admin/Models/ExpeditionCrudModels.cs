using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed record DropdownOptionDto(string Value, string Label);

public sealed record ExpeditionDropdownsDto(
    IReadOnlyCollection<DropdownOptionDto> DifficultyLevels,
    IReadOnlyCollection<DropdownOptionDto> Countries,
    IReadOnlyCollection<DropdownOptionDto> TravelStatuses);

public sealed class ExpeditionCrudRequest
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(220)]
    public string? Slug { get; set; }

    [Required, StringLength(600)]
    public string ShortDescription { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Destination { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Region { get; set; }

    [Range(1, 365)]
    public int DurationDays { get; set; }

    [Range(0, 12000)]
    public int MaxAltitudeMeters { get; set; }

    [Required]
    public DifficultyLevel DifficultyLevel { get; set; }

    [Required]
    public Country Country { get; set; }

    [Required]
    public TravelStatus TravelStatus { get; set; }

    [Range(1, 100)]
    public int MinGroupSize { get; set; }

    [Range(1, 100)]
    public int MaxGroupSize { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    public bool Featured { get; set; }

    [Range(0, 999)]
    public int Ordering { get; set; }

    [Required]
    public int ExpeditionTypeId { get; set; }
}

public sealed record ExpeditionCrudResponse(
    int Id,
    string Name,
    string Slug,
    string ShortDescription,
    string Destination,
    string? Region,
    int DurationDays,
    int MaxAltitudeMeters,
    DifficultyLevel DifficultyLevel,
    Country Country,
    TravelStatus TravelStatus,
    int MinGroupSize,
    int MaxGroupSize,
    decimal Price,
    bool Featured,
    int Ordering,
    int? ExpeditionTypeId,
    string? ExpeditionTypeTitle);
