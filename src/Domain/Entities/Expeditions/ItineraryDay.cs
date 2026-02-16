using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class ItineraryDay : BaseEntity
{
    public int ItineraryId { get; set; }
    public int DayNumber { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? Meals { get; set; }
    public string? AccommodationType { get; set; }

    public Itinerary Itinerary { get; set; } = default!;
}