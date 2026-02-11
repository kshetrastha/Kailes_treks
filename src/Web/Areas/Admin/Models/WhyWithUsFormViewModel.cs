using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class WhyWithUsFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Icon CSS Class")]
    [StringLength(80)]
    public string? IconCssClass { get; set; }

    [Range(0, 999)]
    public int Ordering { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;
}
