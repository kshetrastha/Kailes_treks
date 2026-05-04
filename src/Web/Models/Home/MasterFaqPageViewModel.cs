namespace TravelCleanArch.Web.Models.Home;

public sealed class MasterFaqPageViewModel
{
    public IReadOnlyList<MasterFaqItemViewModel> Faqs { get; init; } = [];
}

public sealed class MasterFaqItemViewModel
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
}
