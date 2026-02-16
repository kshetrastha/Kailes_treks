using System;
using System.Collections.Generic;
using System.Text;

namespace TravelCleanArch.Domain.Entities.Expeditions
{
    public sealed class ExpeditionHighlight : BaseEntity
    {
        public int ExpeditionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public Expedition Expedition { get; set; } = default!;
    }
}
