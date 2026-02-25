using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Domain.Entities.Master;

[Table("Accomodation", Schema = "Master")]
public sealed class Accomodation : BaseEntity
{
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }
}
