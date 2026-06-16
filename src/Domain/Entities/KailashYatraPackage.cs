namespace TravelCleanArch.Domain.Entities;

public sealed class KailashYatraPackage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>Comma-separated month numbers (1=Jan,4=Apr…12=Dec). e.g. "4,5,6,7,8,9,10,11,12,1"</summary>
    public string AvailableMonths { get; set; } = string.Empty;
    /// <summary>Comma-separated month numbers that are Full Moon batches</summary>
    public string FullMoonMonths { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Ordering { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<KailashBooking> Bookings { get; set; } = new List<KailashBooking>();
}
