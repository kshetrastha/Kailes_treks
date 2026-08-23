namespace TravelCleanArch.Domain.Entities;

public sealed class AboutUsPage : BaseEntity
{
    public string Subtitle { get; set; } = "About Our Company";
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ContentHtml { get; set; }
    public string? PrimaryImagePath { get; set; }
    public string? SecondaryImagePath { get; set; }
    public string? BadgeText { get; set; }
    public string? ContactPhone { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public bool IsPublished { get; set; } = true;
    public ICollection<AboutUsHighlight> Highlights { get; set; } = new List<AboutUsHighlight>();
}

public sealed class AboutUsHighlight : BaseEntity
{
    public int AboutUsPageId { get; set; }
    public AboutUsPage AboutUsPage { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}
