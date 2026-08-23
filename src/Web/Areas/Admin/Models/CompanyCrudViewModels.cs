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


public sealed class BlogPostFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(280)] public string Title { get; set; } = string.Empty;
    [StringLength(320)] public string? Slug { get; set; }
    [StringLength(2000)] public string? Summary { get; set; }
    [Required, StringLength(40000)] public string ContentHtml { get; set; } = string.Empty;
    [DataType(DataType.Date)] public DateTime? PublishedOnUtc { get; set; }
    public string? ExistingHeroImagePath { get; set; }
    public IFormFile? HeroImage { get; set; }
    public string? ExistingThumbnailImagePath { get; set; }
    public IFormFile? ThumbnailImage { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; } = true;
}


public sealed class TermsAndConditionFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(120)] public string Country { get; set; } = string.Empty;
    [StringLength(180)] public string? Slug { get; set; }
    [Required, StringLength(280)] public string Title { get; set; } = string.Empty;
    [StringLength(2000)] public string? Summary { get; set; }
    [Required, StringLength(50000)] public string ContentHtml { get; set; } = string.Empty;
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}


public sealed class PrivacyPolicySectionFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(280)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(50000)] public string ContentHtml { get; set; } = string.Empty;
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsContactBlock { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class MasterFaqFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(500)] public string Question { get; set; } = string.Empty;
    [Required, StringLength(4000)] public string Answer { get; set; } = string.Empty;
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
}

public sealed class ServiceTypeFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; }
}


public sealed class ServiceRegionBannerImageInput
{
    public int? ExistingFileDetailId { get; set; }
    public string? ExistingPath { get; set; }
    [StringLength(500)] public string? ShortDescription { get; set; }
    [StringLength(2000)] public string? Description { get; set; }
    public IFormFile? File { get; set; }
    public bool Remove { get; set; }
}

public sealed class ServiceRegionFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(1000)] public string? Description { get; set; }
    [Range(1, int.MaxValue)] public int ServiceTypeId { get; set; }
    [Required, StringLength(100)] public string Reason { get; set; } = string.Empty;
    [StringLength(100)] public string? SlugURL { get; set; }
    [Range(0, int.MaxValue)] public int Ordering { get; set; }
    public IFormFile? BannerImage { get; set; }
    [StringLength(500)] public string? PrimaryBannerShortDescription { get; set; }
    [StringLength(2000)] public string? PrimaryBannerDescription { get; set; }
    public List<ServiceRegionBannerImageInput> BannerImages { get; set; } = [];
    public IFormFile? DashboardImage { get; set; }
    public string? ExistingBannerImagePath { get; set; }
    public string? ExistingDashboardImagePath { get; set; }
}

public sealed class ServiceRegionFaqFormViewModel
{
    public int? Id { get; set; }
    [Range(1, int.MaxValue)] public int ServiceRegionId { get; set; }
    [Required, StringLength(500)] public string Question { get; set; } = string.Empty;
    [Required, StringLength(4000)] public string Answer { get; set; } = string.Empty;
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
}

public sealed class CategoryFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(500)] public string? ShortDescription { get; set; }
    [StringLength(1000)] public string? Description { get; set; }
}

public sealed class DifficultyLevelFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; }
    [Range(0, int.MaxValue)] public int Ordering { get; set; }
}

public sealed class PackageFormViewModel
{
    public int? Id { get; set; }
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    [Range(1, int.MaxValue)] public int ServiceRegionId { get; set; }
    [Range(1, int.MaxValue)] public int DifficultyLevelId { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(500)] public string? ShortDescription { get; set; }
    [StringLength(1000)] public string? Description { get; set; }
    [Range(typeof(decimal), "0", "999999999")] public decimal Price { get; set; }
    public bool IsDiscounted { get; set; }
    [Range(typeof(decimal), "0", "999999999")] public decimal? DiscountedPrice { get; set; }
    [Range(typeof(decimal), "0", "9999")] public decimal Duration { get; set; }
    [Required, StringLength(50)] public string DurationType { get; set; } = string.Empty;
    [StringLength(100)] public string WalkingPerDay { get; set; } = string.Empty;
    [Range(1, int.MaxValue)] public int MaxGroupSize { get; set; }
    [Required, StringLength(200)] public string StartingPoint { get; set; } = string.Empty;
    [Required, StringLength(200)] public string EndingPoint { get; set; } = string.Empty;
    public bool BestSeller { get; set; }
    [Range(0, int.MaxValue)] public int PopularityRank { get; set; }
    [StringLength(200)] public string? Availability { get; set; }
    [Range(typeof(decimal), "0", "999999")] public decimal TotalDistance { get; set; }
    [StringLength(100)] public string? MaxElevation { get; set; }
    [StringLength(250)] public string? SlugURL { get; set; }
}

public sealed class AccomodationFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(250)] public string Name { get; set; } = string.Empty;
    [StringLength(500)] public string? ShortDescription { get; set; }
    [StringLength(1000)] public string? Description { get; set; }
}


public sealed class BannerImageInput
{
    public int? Id { get; set; }
    [StringLength(1000)] public string? SubDescription { get; set; }
    public string? Description { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public string? ExistingImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public bool Remove { get; set; }
}

public sealed class BannerFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(220)] public string Title { get; set; } = string.Empty;
    [StringLength(1000)] public string? SubDescription { get; set; }
    public string? Description { get; set; }
    [Range(0, 999)] public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;
    public List<BannerImageInput> Images { get; set; } = [];
}
