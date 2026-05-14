using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Package;

public sealed class TrekkingInquiryFormViewModel
{
    [Required]
    [StringLength(220)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Comment { get; set; } = string.Empty;
}
