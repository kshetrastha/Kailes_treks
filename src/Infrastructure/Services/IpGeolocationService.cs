using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TravelCleanArch.Application.Abstractions.Travel;

namespace TravelCleanArch.Infrastructure.Services;

/// <summary>
/// Resolves visitor IP addresses through an external geolocation provider (ip-api.com by default).
///
/// This performs an outbound call containing the visitor's IP address. It can be turned off with
/// "IpGeolocation:Enabled": false in configuration, in which case bookings are stored with the raw
/// IP only. Failures are swallowed by design so a provider outage can never block a booking.
/// </summary>
public sealed class IpGeolocationService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<IpGeolocationService> logger) : IIpGeolocationService
{
    private const string DefaultEndpointFormat =
        "http://ip-api.com/json/{0}?fields=status,country,countryCode,regionName,city,zip,timezone,org,lat,lon";

    public async Task<IpGeolocationResult?> LookupAsync(string? ipAddress, CancellationToken ct)
    {
        if (!configuration.GetValue("IpGeolocation:Enabled", true)) return null;
        if (!IsPubliclyRoutable(ipAddress)) return null;

        var endpointFormat = configuration["IpGeolocation:EndpointFormat"] ?? DefaultEndpointFormat;
        var timeoutSeconds = configuration.GetValue("IpGeolocation:TimeoutSeconds", 3);

        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeout.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            var url = string.Format(endpointFormat, Uri.EscapeDataString(ipAddress!));
            using var response = await httpClient.GetAsync(url, timeout.Token);
            if (!response.IsSuccessStatusCode) return null;

            await using var stream = await response.Content.ReadAsStreamAsync(timeout.Token);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: timeout.Token);
            var root = document.RootElement;

            if (root.TryGetProperty("status", out var status) &&
                !string.Equals(status.GetString(), "success", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new IpGeolocationResult(
                Country: ReadString(root, "country"),
                CountryCode: ReadString(root, "countryCode"),
                Region: ReadString(root, "regionName"),
                City: ReadString(root, "city"),
                PostalCode: ReadString(root, "zip"),
                TimeZone: ReadString(root, "timezone"),
                Organisation: ReadString(root, "org"),
                Latitude: ReadDouble(root, "lat"),
                Longitude: ReadDouble(root, "lon"));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or OperationCanceledException)
        {
            // A booking must never fail because the geolocation provider is slow or unreachable.
            logger.LogWarning(ex, "IP geolocation lookup failed for {IpAddress}.", ipAddress);
            return null;
        }
    }

    /// <summary>Skips loopback, link-local, and private ranges so local/dev traffic is not sent out.</summary>
    private static bool IsPubliclyRoutable(string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress)) return false;
        if (!IPAddress.TryParse(ipAddress, out var ip)) return false;

        if (IPAddress.IsLoopback(ip)) return false;

        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
        {
            if (ip.IsIPv6LinkLocal || ip.IsIPv6SiteLocal) return false;
            if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
            else return true;
        }

        var bytes = ip.GetAddressBytes();
        if (bytes.Length != 4) return true;

        return bytes[0] switch
        {
            10 => false,                                    // 10.0.0.0/8
            127 => false,                                   // 127.0.0.0/8
            169 when bytes[1] == 254 => false,              // 169.254.0.0/16
            172 when bytes[1] >= 16 && bytes[1] <= 31 => false, // 172.16.0.0/12
            192 when bytes[1] == 168 => false,              // 192.168.0.0/16
            0 => false,
            _ => true
        };
    }

    private static string? ReadString(JsonElement root, string name)
        => root.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static double? ReadDouble(JsonElement root, string name)
        => root.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.Number
            ? value.GetDouble()
            : null;
}
