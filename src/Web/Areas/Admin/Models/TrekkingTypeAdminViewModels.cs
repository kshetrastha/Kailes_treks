using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class TrekkingTypeImageInput
{
    public string? ExistingPath { get; set; }
    public IFormFile? File { get; set; }
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool Remove { get; set; }
}

public sealed class TrekkingTypeFormViewModel : IValidatableObject
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(600)] public string ShortDescription { get; set; } = string.Empty;
    [StringLength(4000)] public string? Description { get; set; }
    public string? ExistingImagePath { get; set; }
    [Display(Name = "Image")] public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public List<TrekkingTypeImageInput> Images { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var coverCount = Images.Count(x => !x.Remove && (x.IsCover && (x.File is not null || !string.IsNullOrWhiteSpace(x.ExistingPath))));
        var imageCount = Images.Count(x => !x.Remove && (x.File is not null || !string.IsNullOrWhiteSpace(x.ExistingPath)));
        if (imageCount > 0 && coverCount != 1)
        {
            yield return new ValidationResult("Exactly one cover image is required when images exist.", [nameof(Images)]);
        }
    }
}
