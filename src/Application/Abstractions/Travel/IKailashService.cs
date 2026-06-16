namespace TravelCleanArch.Application.Abstractions.Travel;

public interface IKailashService
{
    // Packages (admin)
    Task<List<KailashYatraPackageDto>> GetAllPackagesAsync(CancellationToken ct = default);
    Task<KailashYatraPackageDto?> GetPackageByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreatePackageAsync(KailashYatraPackageUpsertDto dto, CancellationToken ct = default);
    Task<bool> UpdatePackageAsync(int id, KailashYatraPackageUpsertDto dto, CancellationToken ct = default);
    Task<bool> DeletePackageAsync(int id, CancellationToken ct = default);

    // Public
    Task<List<KailashYatraPackageDto>> GetActivePackagesAsync(CancellationToken ct = default);

    // Bookings
    Task<int> CreateBookingAsync(KailashBookingCreateDto dto, string? ipAddress, CancellationToken ct = default);
    Task<KailashBookingPagedResult> ListBookingsAsync(string? search, string? status, int page, int pageSize, CancellationToken ct = default);
    Task<KailashBookingDetailsDto?> GetBookingByIdAsync(int id, CancellationToken ct = default);
    Task<bool> UpdateBookingStatusAsync(int id, string status, CancellationToken ct = default);
}
