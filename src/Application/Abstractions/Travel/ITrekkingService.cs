using TravelCleanArch.Application.Common;

namespace TravelCleanArch.Application.Abstractions.Travel;

public interface ITrekkingService
{
    Task<TrekkingPagedResult> ListAsync(string? search, string? status, string? destination, string? trekkingType, bool? featured, int page, int pageSize, CancellationToken ct);
    Task<TrekkingDetailsDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(TrekkingUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, TrekkingUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<TrekkingDetailsDto?> GetPublicBySlugAsync(string slug, CancellationToken ct);
    Task<IReadOnlyCollection<TrekkingTourCardDto>> GetRecentPublicToursAsync(int excludeTrekkingId, int count, CancellationToken ct);
    Task<IReadOnlyCollection<TrekkingTourCardDto>> GetRelatedPublicToursAsync(int trekkingId, int? trekkingTypeId, string? destination, int count, CancellationToken ct);
    Task<List<TrekkingCountryGroupDto>> GetPublicTrekkingHierarchyAsync(CancellationToken ct);
    Task<List<TrekkingCountryPackageCountDto>> GetPublicTrekkingPackageCountByCountryAsync(CancellationToken ct);

    Task<List<SelectOptionDto>> GetAllOptionsAsync(
        string? selectedTrekkingType = null,
        CancellationToken ct = default);
    Task<List<SelectOptionDto>> GetOptionsByDestinationAsync(
    string? destination,
    string? selectedTrekkingType = null,
    CancellationToken ct = default);
}
