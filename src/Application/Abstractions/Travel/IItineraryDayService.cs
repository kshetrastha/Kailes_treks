namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record ItineraryDaySummaryDto(int Id, int ItineraryId, string ItineraryTitle, int ExpeditionId, string ExpeditionName, int DayNumber, string? ShortDescription, string? Meals, string? AccommodationType);
public sealed record ItineraryDayUpsertItemDto(int Id, int DayNumber, string? ShortDescription, string? Description, string? Meals, string? AccommodationType);

public interface IItineraryDayService
{
    Task<IReadOnlyList<ItineraryDaySummaryDto>> ListAsync(int? itineraryId, CancellationToken ct);
    Task<IReadOnlyList<ItineraryDayUpsertItemDto>> ListForItineraryAsync(int itineraryId, CancellationToken ct);
    Task UpsertForItineraryAsync(int itineraryId, IReadOnlyCollection<ItineraryDayUpsertItemDto> items, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
