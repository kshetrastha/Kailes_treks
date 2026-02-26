namespace TravelCleanArch.Application.Abstractions.Travel;

public interface IExpeditionTypeService
{
    Task<IReadOnlyCollection<ExpeditionTypeDto>> ListAsync(bool includeUnpublished, CancellationToken ct);
    Task<ExpeditionTypeDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(ExpeditionTypeUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, ExpeditionTypeUpsertDto request, int? userId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
