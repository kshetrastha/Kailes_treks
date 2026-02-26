using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Models.Expeditions
{
    public class ExpeditionMediaCreateUpdateModel
    {
        public int? Id { get; set; }
        public int? ExpeditionId { get; set; }

        [MaxLength(1000)]
        public string Url { get; set; } = string.Empty;          // external url (image/video)

        [MaxLength(500)]
        public string? Caption { get; set; }

        // If you want to keep your current string column ("image"/"video"), keep it in the model too
        [MaxLength(50)]
        public string MediaType { get; set; } = "image";

        public int Ordering { get; set; }

        // Local stored file path (after upload)
        [MaxLength(500)]
        public string? FilePath { get; set; }

        // For embedded/hosted video links
        [MaxLength(1000)]
        public string? VideoUrl { get; set; }

        public ExpeditionMediaType MediaKind { get; set; } = ExpeditionMediaType.Photo;

        // Optional upload (photo/video thumbnail)
        public IFormFile? File { get; set; }
    }

}
