namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class ExpeditionFaq : BaseEntity
{
    public int ExpeditionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Ordering { get; set; }

    public Expedition Expedition { get; set; } = default!;
}
