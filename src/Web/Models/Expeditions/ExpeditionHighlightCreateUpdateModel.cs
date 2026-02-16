using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class ExpeditionHighlightCreateUpdateModel
{
    public int? Id { get; set; }
    public int? ExpeditionId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}