using System;
using System.Collections.Generic;

namespace TravelCleanArch.Domain.Constants;

public static class ExpeditionSectionTypes
{
    public const string Overview = "overview";
    public const string Itinerary = "itinerary";
    public const string Inclusion = "inclusion";
    public const string Exclusion = "exclusion";
    public const string Review = "review";

    public static readonly IReadOnlySet<string> Allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Overview,
        Itinerary,
        Inclusion,
        Exclusion,
        Review
    };
}
