namespace TravelCleanArch.Domain.Entities;

public sealed class WhoWeAre : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? ImageCaption { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class WhoWeAreHero : BaseEntity
{
    public string Header { get; set; } = "Leading Expedition Operator";
    public string Title { get; set; } = "Who we are?";
    public string Description { get; set; } = string.Empty;
    public string? BackgroundImagePath { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
