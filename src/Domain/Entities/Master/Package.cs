using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCleanArch.Domain.Entities.Master;

[Table("PackageDetail", Schema = "Master")]
public sealed class Package : BaseEntity
{
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    public int ServiceRegionId { get; set; }

    [ForeignKey(nameof(ServiceRegionId))]
    public ServiceRegion ServiceRegion { get; set; } = null!;

    public int DifficultyLevelId { get; set; }

    [ForeignKey(nameof(DifficultyLevelId))]
    public DifficultyLevel DifficultyLevel { get; set; } = null!;

    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsDiscounted { get; set; }

    public decimal? DiscountedPrice { get; set; }

    public decimal Duration { get; set; }

    [MaxLength(50)]
    public string DurationType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string WalkingPerDay { get; set; } = string.Empty;

    public int MaxGroupSize { get; set; }

    [MaxLength(200)]
    public string StartingPoint { get; set; } = string.Empty;

    [MaxLength(200)]
    public string EndingPoint { get; set; } = string.Empty;

    public bool BestSeller { get; set; }

    public int PopularityRank { get; set; }

    [MaxLength(200)]
    public string? Availability { get; set; }

    public decimal TotalDistance { get; set; }

    [MaxLength(100)]
    public string? MaxElevation { get; set; }

    [MaxLength(250)]
    public string? SlugURL { get; set; }
}
