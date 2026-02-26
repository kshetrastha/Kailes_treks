using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities.Expeditions
{
    public sealed class ExpeditionReview : BaseEntity
    {
        public int ExpeditionId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string? UserPhotoPath { get; set; }
        public string? VideoUrl { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public ReviewModerationStatus ModerationStatus { get; set; } = ReviewModerationStatus.Pending;
        public Expedition Expedition { get; set; } = default!;
    }
}
