using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class ItineraryRowInput
{
    public int Id { get; set; }

    [Required, StringLength(250)]
    public string SeasonTitle { get; set; } = string.Empty;

    [Range(0, 999)]
    public int SortOrder { get; set; }
}

public sealed class ItineraryBulkFormViewModel
{
    [Required]
    public int ExpeditionId { get; set; }

    public List<SelectListItem> ExpeditionOptions { get; set; } = [];
    public List<ItineraryRowInput> Rows { get; set; } = [new()];
}

public sealed class ItineraryDayRowInput
{
    public int Id { get; set; }

    [Range(1, 365)]
    public int DayNumber { get; set; } = 1;

    [StringLength(500)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    [StringLength(120)]
    public string? Meals { get; set; }

    [StringLength(120)]
    public string? AccommodationType { get; set; }
}

public sealed class ItineraryDayBulkFormViewModel
{
    [Required]
    public int ItineraryId { get; set; }

    public List<SelectListItem> ItineraryOptions { get; set; } = [];
    public List<ItineraryDayRowInput> Rows { get; set; } = [new()];
}
