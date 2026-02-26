using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class CostItemCreateUpdateModel
{
    public int? Id { get; set; }                 // null for new rows (create)
    public int? ExpeditionId { get; set; }       // optional in UI; you can set it server-side

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? ShortDescription { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public CostItemType Type { get; set; }       // Included / Excluded etc.

    public int SortOrder { get; set; }
}