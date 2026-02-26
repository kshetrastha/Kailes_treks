namespace TravelCleanArch.Web.Models.Home;

public sealed class WhyWithUsItemViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? IconCssClass { get; init; }
    public string? ImagePath { get; init; }
}
