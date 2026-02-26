using System;
using System.Collections.Generic;
using System.Text;
using TravelCleanArch.Domain.Entities.Expeditions;

namespace TravelCleanArch.Domain.Entities.Master;

public sealed class ExpeditionType : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public int Ordering { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<Expedition> Expeditions { get; set; } = [];
    public List<ExpeditionTypeImage> Images { get; set; } = [];
}
