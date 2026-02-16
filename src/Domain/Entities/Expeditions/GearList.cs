namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class GearList : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string? ShortDescription { get; set; }
    public string? ImagePath { get; set; } // cover image for the gear list
    public string FilePath { get; set; } = string.Empty;
    public Expedition Expedition { get; set; } = default!;
}
