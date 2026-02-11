namespace TravelCleanArch.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}
