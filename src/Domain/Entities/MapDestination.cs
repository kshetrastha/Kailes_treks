namespace TravelCleanArch.Domain.Entities;

public sealed class MapDestination : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? HeroImagePath { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<MapDestinationImage> Images { get; set; } = [];
}

public sealed class MapDestinationImage : BaseEntity
{
    public int MapDestinationId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }

    public MapDestination? MapDestination { get; set; }
}
