using System;
using System.Collections.Generic;
using System.Text;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities.Expeditions
{
    public sealed class ExpeditionMedia : BaseEntity
    {
        public int ExpeditionId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public string MediaType { get; set; } = "image";
        public int Ordering { get; set; }
        public string? FilePath { get; set; }
        public string? VideoUrl { get; set; }
        public ExpeditionMediaType MediaKind { get; set; } = ExpeditionMediaType.Photo;

        public Expedition Expedition { get; set; } = default!;
    }
}
