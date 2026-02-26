namespace TravelCleanArch.Application.Abstractions.Travel;

public interface IExpeditionService
{
    Task<ExpeditionPagedResult> ListAsync(string? search, string? status, string? destination, bool? featured, int page, int pageSize, CancellationToken ct);
    Task<ExpeditionDetailsDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(ExpeditionUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, ExpeditionUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<ExpeditionDetailsDto> GetPublicBySlugAsync(string slug, CancellationToken ct);
}
