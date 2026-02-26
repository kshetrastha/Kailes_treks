using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class WhoWeAreHeroFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Header { get; set; } = "Leading Expedition Operator";

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = "Who we are?";

    [Required]
    [StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    public string? ExistingBackgroundImagePath { get; set; }

    [Display(Name = "Background Image")]
    public IFormFile? BackgroundImage { get; set; }
}
