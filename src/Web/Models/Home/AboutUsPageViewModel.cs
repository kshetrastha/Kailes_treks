namespace TravelCleanArch.Web.Models.Home;

public sealed class AboutUsPageViewModel
{
    public string Subtitle { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ContentHtml { get; init; }
    public string? PrimaryImagePath { get; init; }
    public string? SecondaryImagePath { get; init; }
    public string? BadgeText { get; init; }
    public string? ContactPhone { get; init; }
    public string? ButtonText { get; init; }
    public string? ButtonUrl { get; init; }
    public IReadOnlyList<string> Highlights { get; init; } = [];
}
