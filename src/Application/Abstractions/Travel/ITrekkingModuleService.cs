namespace TravelCleanArch.Application.Abstractions.Travel;

public interface ITrekkingModuleService
{
    Task<IReadOnlyCollection<TrekkingBasicInfoListItemDto>> ListBasicInfosAsync(CancellationToken ct);
    Task<TrekkingModuleDetailsDto?> GetDetailsAsync(int id, CancellationToken ct);
    Task<TrekkingModuleDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct);
    Task<int> CreateBasicInfoAsync(TrekkingBasicInfoUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateBasicInfoAsync(int id, TrekkingBasicInfoUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteBasicInfoAsync(int id, CancellationToken ct);

    Task<bool> UpsertOverviewAsync(int trekkingId, TrekkingOverviewUpsertDto request, int? userId, CancellationToken ct);

    Task<bool> ReplaceItinerariesAsync(int trekkingId, IReadOnlyCollection<TrekkingItineraryUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceInclusionExclusionsAsync(int trekkingId, IReadOnlyCollection<TrekkingInclusionExclusionUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceFixedDeparturesAsync(int trekkingId, IReadOnlyCollection<TrekkingFixedDepartureUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceGearsAsync(int trekkingId, IReadOnlyCollection<TrekkingGearUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceReviewsAsync(int trekkingId, IReadOnlyCollection<TrekkingReviewUpsertDto> items, int? userId, CancellationToken ct);
    Task<bool> ReplaceFaqsAsync(int trekkingId, IReadOnlyCollection<TrekkingFaqUpsertDto> items, int? userId, CancellationToken ct);
}
