namespace TravelCleanArch.Domain.Entities;

public sealed class TrekkingInquiry : BaseEntity
{
    public int TrekkingId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime SubmittedAtUtc { get; set; }
    public string? SourcePage { get; set; }
    public string? IpAddress { get; set; }

    public Trekking Trekking { get; set; } = default!;
}
