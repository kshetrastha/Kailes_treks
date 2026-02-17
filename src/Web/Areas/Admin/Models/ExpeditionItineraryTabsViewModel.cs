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
}
