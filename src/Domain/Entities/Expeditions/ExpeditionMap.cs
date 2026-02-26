namespace TravelCleanArch.Domain.Entities.Expeditions
{
    public sealed class ExpeditionMap : BaseEntity
    {
        public int ExpeditionId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Notes { get; set; }

        public Expedition Expedition { get; set; } = default!;
    }
}
