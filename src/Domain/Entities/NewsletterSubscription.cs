namespace TravelCleanArch.Domain.Entities;

public sealed class NewsletterSubscription : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public DateTime SubscribedAtUtc { get; set; }
    public string? SourcePage { get; set; }
    public string? IpAddress { get; set; }
}
