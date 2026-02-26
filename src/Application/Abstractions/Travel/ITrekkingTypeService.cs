namespace TravelCleanArch.Application.Abstractions.Travel;

public interface ITrekkingTypeService
{
    Task<IReadOnlyCollection<TrekkingTypeDto>> ListAsync(bool includeUnpublished, CancellationToken ct);
    Task<TrekkingTypeDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(TrekkingTypeUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, TrekkingTypeUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
