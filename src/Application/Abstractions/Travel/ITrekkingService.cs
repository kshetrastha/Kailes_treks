namespace TravelCleanArch.Application.Abstractions.Travel;

public interface ITrekkingService
{
    Task<TrekkingPagedResult> ListAsync(string? search, string? status, string? destination, bool? featured, int page, int pageSize, CancellationToken ct);
    Task<int> CreateAsync(TrekkingUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, TrekkingUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<object?> GetPublicBySlugAsync(string slug, CancellationToken ct);
}
