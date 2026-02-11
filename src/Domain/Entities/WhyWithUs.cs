namespace TravelCleanArch.Domain.Entities;

public sealed class WhyWithUs : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconCssClass { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class WhyWithUsHero : BaseEntity
{
    public string Header { get; set; } = "Because we are the best";
    public string Title { get; set; } = "Why with us?";
    public string Description { get; set; } = string.Empty;
    public string? BackgroundImagePath { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
