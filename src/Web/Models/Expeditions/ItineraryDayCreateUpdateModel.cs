using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class ItineraryDayCreateUpdateModel
{
    public int? Id { get; set; }
    public int? ItineraryId { get; set; }

    [Range(1, 200)]
    public int DayNumber { get; set; }

    [MaxLength(1000)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Meals { get; set; }

    [MaxLength(200)]
    public string? AccommodationType { get; set; }
}
