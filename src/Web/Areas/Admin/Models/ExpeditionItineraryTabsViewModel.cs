using Microsoft.AspNetCore.Mvc.Rendering;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class ExpeditionItineraryTabsViewModel
{
    public int ExpeditionId { get; set; }
    public string ExpeditionName { get; set; } = string.Empty;
    public string ActiveTab { get; set; } = "itineraries";

    public int? SelectedItineraryId { get; set; }
    public List<SelectListItem> ItineraryOptions { get; set; } = [];

    public List<ItineraryRowInput> Itineraries { get; set; } = [new()];
    public List<ItineraryDayRowInput> ItineraryDays { get; set; } = [new()];
    public List<MapInput> Maps { get; set; } = [new()];
    public List<CostItemInput> CostIncludes { get; set; } = [new()];
    public List<CostItemInput> CostExcludes { get; set; } = [new()];
    public List<FixedDepartureInput> FixedDepartures { get; set; } = [new()];
    public List<GearListInput> GearLists { get; set; } = [new()];
    public List<MediaInput> MediaItems { get; set; } = [new()];
    public List<ReviewInput> Reviews { get; set; } = [new()];
    public List<ExpeditionFaqInput> Faqs { get; set; } = [new()];
    public List<HighlightInput> Highlights { get; set; } = [new()];
}

public sealed class ExpeditionFaqInput
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Ordering { get; set; }
}
