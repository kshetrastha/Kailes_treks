using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TravelCleanArch.Web.Models.Package;

public sealed class TrekkingReviewFormViewModel
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

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    public IFormFile? ProfileImage { get; set; }
}
