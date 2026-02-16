using TravelCleanArch.Domain.Entities.Expeditions;

namespace TravelCleanArch.Domain.Entities.Master;

public sealed class Itinerary : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string SeasonTitle { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    // Navigation
    public Expedition Expedition { get; set; } = default!;
    public List<ItineraryDay> Days { get; set; } = [];
}