namespace TravelCleanArch.Application.Abstractions.Travel;

public interface IExpeditionModuleService
{
    Task<IReadOnlyCollection<ExpeditionBasicInfoListItemDto>> ListBasicInfosAsync(CancellationToken ct);
    Task<ExpeditionModuleDetailsDto?> GetDetailsAsync(int id, CancellationToken ct);
    Task<int> CreateBasicInfoAsync(ExpeditionBasicInfoUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateBasicInfoAsync(int id, ExpeditionBasicInfoUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteBasicInfoAsync(int id, CancellationToken ct);

    Task<bool> UpsertOverviewAsync(int expeditionId, ExpeditionOverviewUpsertDto request, int? userId, CancellationToken ct);

    Task<bool> ReplaceItinerariesAsync(int expeditionId, IReadOnlyCollection<ExpeditionItineraryUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceInclusionExclusionsAsync(int expeditionId, IReadOnlyCollection<ExpeditionInclusionExclusionUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceFixedDeparturesAsync(int expeditionId, IReadOnlyCollection<ExpeditionFixedDepartureUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceGearsAsync(int expeditionId, IReadOnlyCollection<ExpeditionGearUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceReviewsAsync(int expeditionId, IReadOnlyCollection<ExpeditionReviewUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceFaqsAsync(int expeditionId, IReadOnlyCollection<ExpeditionFaqUpsertDto> items, int? userId, CancellationToken ct);
}
