namespace TravelCleanArch.Domain.Entities.Master;
public sealed class ExpeditionTypeImage : BaseEntity
{
    public int ExpeditionTypeId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool IsActive { get; set; } = true;
    public ExpeditionType ExpeditionType { get; set; } = default!;
}
