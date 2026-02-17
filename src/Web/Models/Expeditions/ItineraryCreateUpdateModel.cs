using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class ItineraryCreateUpdateModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string SeasonTitle { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public List<ItineraryDayCreateUpdateModel> Days { get; set; } = [];
}
