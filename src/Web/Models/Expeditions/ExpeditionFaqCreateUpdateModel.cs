using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class ExpeditionFaqCreateUpdateModel
{
    public int? Id { get; set; }
    public int? ExpeditionId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [Required]
    public string Answer { get; set; } = string.Empty;

    public int Ordering { get; set; }
}
