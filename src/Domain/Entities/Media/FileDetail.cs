using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCleanArch.Domain.Entities.Media;

[Table("FileDetail", Schema = "Media")]
public sealed class FileDetail : BaseEntity
{
    [MaxLength(1000)]
    public required string FileURL { get; set; }

    [MaxLength(255)]
    public required string OriginalName { get; set; }

    [MaxLength(150)]
    public required string ContentType { get; set; }

    [MaxLength(255)]
    public required string FileName { get; set; }

    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
}
