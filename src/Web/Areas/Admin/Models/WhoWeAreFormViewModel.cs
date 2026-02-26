using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class WhoWeAreFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Short Description")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Sub Description")]
    [StringLength(500)]
    public string? SubDescription { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Image Caption")]
    [StringLength(300)]
    public string? ImageCaption { get; set; }

    public string? ExistingImagePath { get; set; }

    [Display(Name = "Primary Image")]
    public IFormFile? Image { get; set; }

    [Display(Name = "Additional Images")]
    public List<IFormFile> AdditionalImages { get; set; } = [];

    public List<string?> AdditionalImageCaptions { get; set; } = [];

    public List<WhoWeAreAdditionalImageViewModel> ExistingAdditionalImages { get; set; } = [];

    [Range(0, 999)]
    public int Ordering { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;
}

public sealed class WhoWeAreAdditionalImageViewModel
{
    public string ImagePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int Ordering { get; set; }
}
