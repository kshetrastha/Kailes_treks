namespace TravelCleanArch.Web.Models.Home;

public sealed class WhoWeAreItemViewModel
{
    public string Title { get; init; } = string.Empty;
    public string? SubDescription { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? ImagePath { get; init; }
    public string? ImageCaption { get; init; }
    public IReadOnlyList<WhoWeAreImageItemViewModel> Images { get; init; } = [];
}

public sealed class WhoWeAreImageItemViewModel
{
    public string ImagePath { get; init; } = string.Empty;
    public string? Caption { get; init; }
}
