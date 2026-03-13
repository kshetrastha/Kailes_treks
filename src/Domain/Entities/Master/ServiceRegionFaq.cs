using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCleanArch.Domain.Entities.Master;

[Table("ServiceRegionFAQ", Schema = "Service")]
public sealed class ServiceRegionFaq : BaseEntity
{
    public int ServiceRegionId { get; set; }

    [ForeignKey(nameof(ServiceRegionId))]
    public ServiceRegion ServiceRegion { get; set; } = default!;

    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Answer { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
