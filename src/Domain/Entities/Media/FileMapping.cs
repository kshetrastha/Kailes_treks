using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCleanArch.Domain.Entities.Media;

[Table("FileMapping", Schema = "Media")]
public sealed class FileMapping : BaseEntity
{
    public int FileDetailId { get; set; }
    public int TargetId { get; set; }

    [MaxLength(100)]
    public string TargetType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string MediaType { get; set; } = string.Empty;

    [ForeignKey(nameof(FileDetailId))]
    public FileDetail FileDetail { get; set; } = default!;
}

public static class FileMappingTargetTypes
{
    public const string ServiceRegion = "ServiceRegion";
}

public static class FileMappingMediaTypes
{
    public const string BannerImage = "BannerImage";
    public const string DashboardImage = "DashboardImage";
}
