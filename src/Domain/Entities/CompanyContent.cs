namespace TravelCleanArch.Domain.Entities;

public sealed class Award : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public DateTime? AwardedOnUtc { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string? ReferenceUrl { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class Patron : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? Biography { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class ChairmanMessage : BaseEntity
{
    public string Heading { get; set; } = "Chairman's Message";
    public string ChairmanName { get; set; } = string.Empty;
    public string? Designation { get; set; }
    public string MessageHtml { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class TeamMember : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public string? ImagePath { get; set; }
    public string? Email { get; set; }
    public string? LinkedInUrl { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class CertificateDocument : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? FilePath { get; set; }
    public string? ThumbnailImagePath { get; set; }
    public DateTime? IssuedOnUtc { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class Review : BaseEntity
{
    public string ReviewerName { get; set; } = string.Empty;
    public string? ReviewerRole { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public string? ReviewerImagePath { get; set; }
    public string? SourceName { get; set; }
    public string? SourceUrl { get; set; }
    public DateTime? ReviewedOnUtc { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class BlogPost : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string ContentHtml { get; set; } = string.Empty;
    public string? HeroImagePath { get; set; }
    public string? ThumbnailImagePath { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public int Ordering { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
