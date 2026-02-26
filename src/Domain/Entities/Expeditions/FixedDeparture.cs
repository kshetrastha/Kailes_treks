using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class FixedDeparture : BaseEntity
{
    public int ExpeditionId { get; set; }
    public Expedition Expedition { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ForDays { get; set; }
    public DepartureStatus Status { get; set; } = DepartureStatus.BookingCreated;
    public int? GroupSize { get; set; }

}
