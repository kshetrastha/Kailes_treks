namespace TravelCleanArch.Web.Models.Home;

public sealed class MapDestinationDetailsViewModel
{
    public string Name { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? HeroImagePath { get; init; }
    public IReadOnlyList<MapDestinationDetailsImageViewModel> Images { get; init; } = [];
}

public sealed class MapDestinationDetailsImageViewModel
{
    public string ImagePath { get; init; } = string.Empty;
    public string? Caption { get; init; }
}
