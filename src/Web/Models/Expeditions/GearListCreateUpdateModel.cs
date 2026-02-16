using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class GearListCreateUpdateModel
{
    public int? Id { get; set; }
    public int? ExpeditionId { get; set; }

    [MaxLength(1000)]
    public string? ShortDescription { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }          // cover image path

    public IFormFile? ImageFile { get; set; }       // optional upload for cover image

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;  // PDF/doc path

    public IFormFile? File { get; set; }            // optional upload for PDF/doc
}