using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Web.Models.Expeditions
{
    public class ExpeditionMapCreateUpdateModel
    {
        public int? Id { get; set; }
        public int? ExpeditionId { get; set; }

        // If you upload files, you can keep FilePath + IFormFile together
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public IFormFile? File { get; set; }         // optional: only if you upload maps from UI

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }
    }

}
