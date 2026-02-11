using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class WhoWeAreFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Image Caption")]
    [StringLength(300)]
    public string? ImageCaption { get; set; }

    public string? ExistingImagePath { get; set; }

    [Display(Name = "Image")]
    public IFormFile? Image { get; set; }

    [Range(0, 999)]
    public int Ordering { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;
}
