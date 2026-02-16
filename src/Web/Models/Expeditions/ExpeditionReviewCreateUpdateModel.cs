using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Models.Expeditions
{
    public class ExpeditionReviewCreateUpdateModel
    {
        public int? Id { get; set; }
        public int? ExpeditionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string EmailAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? UserPhotoPath { get; set; }

        public IFormFile? UserPhotoFile { get; set; }   // optional upload

        [MaxLength(1000)]
        public string? VideoUrl { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string ReviewText { get; set; } = string.Empty;

        public ReviewModerationStatus ModerationStatus { get; set; } = ReviewModerationStatus.Pending;
    }

}
