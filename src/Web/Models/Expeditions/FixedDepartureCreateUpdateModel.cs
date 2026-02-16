using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Web.Models.Expeditions;

public class FixedDepartureCreateUpdateModel
{
    public int? Id { get; set; }
    public int? ExpeditionId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public int ForDays { get; set; }

    public DepartureStatus Status { get; set; } = DepartureStatus.BookingCreated;

    public int? GroupSize { get; set; }
}