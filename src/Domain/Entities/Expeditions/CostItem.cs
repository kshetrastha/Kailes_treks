using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class CostItem : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public CostItemType Type { get; set; }
    public int SortOrder { get; set; }

    public Expedition Expedition { get; set; } = default!;
}
