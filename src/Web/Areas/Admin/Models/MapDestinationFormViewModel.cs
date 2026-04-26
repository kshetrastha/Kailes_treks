using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class MapDestinationFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(600)]
    [Display(Name = "Short Description")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Hero Image")]
    public IFormFile? HeroImage { get; set; }
    public string? ExistingHeroImagePath { get; set; }

    public int Ordering { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;

    public List<MapDestinationImageInput> Images { get; set; } = [new()];
}

public sealed class MapDestinationImageInput
{
    public int Id { get; set; }
    public IFormFile? File { get; set; }
    public string? ExistingPath { get; set; }
    [StringLength(500)] public string? Caption { get; set; }
    public int SortOrder { get; set; }
}
