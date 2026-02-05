using System.Text.RegularExpressions;

namespace TravelCleanArch.Infrastructure.Services;

internal static partial class TravelSlug
{
    [GeneratedRegex("[^a-z0-9]+", RegexOptions.Compiled)]
    private static partial Regex InvalidSlugRegex();

    public static string Generate(string value)
    {
        var slug = InvalidSlugRegex().Replace(value.ToLowerInvariant().Trim(), "-").Trim('-');
        return string.IsNullOrWhiteSpace(slug) ? "item" : slug;
    }
}
