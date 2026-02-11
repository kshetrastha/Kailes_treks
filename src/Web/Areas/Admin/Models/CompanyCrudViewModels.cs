using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Areas.Admin.Models;

public sealed class AwardFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [StringLength(220)] public string? Issuer { get; set; }
    [DataType(DataType.Date)] public DateTime? AwardedOnUtc { get; set; }
    [StringLength(4000)] public string? Description { get; set; }
    [Url, StringLength(500)] public string? ReferenceUrl { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class PatronFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Name { get; set; } = string.Empty;
    [Required, StringLength(220)] public string Role { get; set; } = string.Empty;
    [StringLength(4000)] public string? Biography { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class ChairmanMessageFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Heading { get; set; } = "Chairman's Message";
    [Required, StringLength(220)] public string ChairmanName { get; set; } = string.Empty;
    [StringLength(220)] public string? Designation { get; set; }
    [Required, StringLength(12000)] public string MessageHtml { get; set; } = string.Empty;
    [Url, StringLength(500)] public string? VideoUrl { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class TeamMemberFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string FullName { get; set; } = string.Empty;
    [Required, StringLength(220)] public string Role { get; set; } = string.Empty;
    [StringLength(4000)] public string? Biography { get; set; }
    [EmailAddress, StringLength(220)] public string? Email { get; set; }
    [Url, StringLength(500)] public string? LinkedInUrl { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class CertificateDocumentFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [StringLength(160)] public string? Category { get; set; }
    [StringLength(2000)] public string? Description { get; set; }
    [DataType(DataType.Date)] public DateTime? IssuedOnUtc { get; set; }
    public string? ExistingFilePath { get; set; }
    public IFormFile? DocumentFile { get; set; }
    public string? ExistingThumbnailPath { get; set; }
    public IFormFile? ThumbnailImage { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class ReviewFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string ReviewerName { get; set; } = string.Empty;
    [StringLength(220)] public string? ReviewerRole { get; set; }
    [Required, StringLength(6000)] public string ReviewText { get; set; } = string.Empty;
    [Range(1, 5)] public int Rating { get; set; } = 5;
    [StringLength(220)] public string? SourceName { get; set; }
    [Url, StringLength(500)] public string? SourceUrl { get; set; }
    [DataType(DataType.Date)] public DateTime? ReviewedOnUtc { get; set; }
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}
