namespace TravelCleanArch.Web.Models.Home;

public sealed class HomeIndexViewModel
{
    public IReadOnlyCollection<WhyWithUsItemViewModel> WhyWithUsItems { get; init; } = [];
}
