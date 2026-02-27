using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCleanArch.Domain.Entities.Master;

[Table("ServiceRegions", Schema = "Service")]
public sealed class ServiceRegion : BaseEntity
{
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int ServiceTypeId { get; set; }

    [ForeignKey(nameof(ServiceTypeId))]
    public ServiceType ServiceType { get; set; } = default!;

    [MaxLength(100)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SlugURL { get; set; }

    public int Ordering { get; set; }
}
