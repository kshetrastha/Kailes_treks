namespace TravelCleanArch.Web.Models.Home;

public sealed class HomeIndexViewModel
{
    public string WhyWithUsHeader { get; init; } = "Because we are the best";
    public string WhyWithUsTitle { get; init; } = "Why with us?";
    public string WhyWithUsDescription { get; init; } = string.Empty;
    public string? WhyWithUsBackgroundImagePath { get; init; }
    public IReadOnlyCollection<WhyWithUsItemViewModel> WhyWithUsItems { get; init; } = [];
    //public IReadOnlyCollection<WhyWithUsItemViewModel> WhyWithUsItems { get; init; } = [];
    public IReadOnlyList<WhyWithUsItemViewModel> WhyWithUsItems { get; init; }

}
