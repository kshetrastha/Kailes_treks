namespace TravelCleanArch.Web.Models.Home;

public sealed class WhoWeAreItemViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? ImagePath { get; init; }
    public string? ImageCaption { get; init; }
}
