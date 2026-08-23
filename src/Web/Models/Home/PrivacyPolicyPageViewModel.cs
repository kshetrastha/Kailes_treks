namespace TravelCleanArch.Web.Models.Home;

public sealed class PrivacyPolicyPageViewModel
{
    public IReadOnlyList<PrivacyPolicySectionViewModel> Sections { get; init; } = [];
    public IReadOnlyList<PrivacyPolicySectionViewModel> ContactBlocks { get; init; } = [];
    public DateTime? LastUpdatedUtc { get; init; }
}

public sealed class PrivacyPolicySectionViewModel
{
    public string Title { get; init; } = string.Empty;
    public string ContentHtml { get; init; } = string.Empty;
}
