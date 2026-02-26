namespace TravelCleanArch.Application.Abstractions.Travel;

public sealed record ItinerarySummaryDto(int Id, int ExpeditionId, string ExpeditionName, string SeasonTitle, int SortOrder, int DayCount);
public sealed record ItineraryUpsertItemDto(int Id, string SeasonTitle, int SortOrder);

public interface IItineraryService
{
    Task<IReadOnlyList<ItinerarySummaryDto>> ListAsync(int? expeditionId, CancellationToken ct);
    Task<IReadOnlyList<ItineraryUpsertItemDto>> ListForExpeditionAsync(int expeditionId, CancellationToken ct);
    Task UpsertForExpeditionAsync(int expeditionId, IReadOnlyCollection<ItineraryUpsertItemDto> items, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
