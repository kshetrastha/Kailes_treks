namespace TravelCleanArch.Application.Abstractions.Travel;

/// <summary>Geolocation details resolved for a visitor IP address. Every field is best-effort.</summary>
public sealed record IpGeolocationResult(
    string? Country,
    string? CountryCode,
    string? Region,
    string? City,
    string? PostalCode,
    string? TimeZone,
    string? Organisation,
    double? Latitude,
    double? Longitude);

public interface IIpGeolocationService
{
    /// <summary>
    /// Resolves an IP address to a location. Never throws and never blocks a caller indefinitely:
    /// returns null when lookup is disabled, the address is local/private, or the provider fails.
    /// </summary>
    Task<IpGeolocationResult?> LookupAsync(string? ipAddress, CancellationToken ct);
}
