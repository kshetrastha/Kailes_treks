namespace TravelCleanArch.Web.Models.Home;

public sealed class ExpeditionsByTypeViewModel
{
    public int TypeId { get; init; }
    public string TypeTitle { get; init; } = string.Empty;
    public string? ShortDescription { get; init; }
    public string? TypeDescription { get; init; }
    public string? ImagePath { get; init; }
    public IReadOnlyList<ExpeditionTypeCardViewModel> Expeditions { get; init; } = [];
}

public sealed class ExpeditionTypeCardViewModel
{
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public int DurationDays { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
    public string? ImagePath { get; init; }
}
