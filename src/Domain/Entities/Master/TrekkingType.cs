using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Domain.Entities.Master;

public sealed class TrekkingType : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<Trekking> TrekkingItems { get; set; } = [];
    public List<TrekkingTypeImage> Images { get; set; } = [];
}

public sealed class TrekkingTypeImage : BaseEntity
{
    public int TrekkingTypeId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool IsActive { get; set; } = true;

    public TrekkingType TrekkingType { get; set; } = default!;
}
