using System.ComponentModel.DataAnnotations;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Domain.Enumerations;

namespace TravelCleanArch.Domain.Entities.Expeditions;

public sealed class Expedition : BaseEntity
{
    // FK
    public int ExpeditionTypeId { get; set; }
    public ExpeditionType? ExpeditionType { get; set; }
    // Core
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;

    // “Trip Facts” / Overview card
    public string Destination { get; set; } = string.Empty;          // e.g., Nepal
    public string? Region { get; set; }
    public int DurationDays { get; set; }
    public int MaxAltitudeMeters { get; set; }
    public int? MaxAltitudeFeet { get; set; }                        // optional, page shows ft too
    public DifficultyLevel? DifficultyLevel { get; set; }
    public Season? BestSeason { get; set; }                          // e.g., Spring
    public string? WalkingPerDay { get; set; }                       // e.g., "5 - 7 Hours"
    public string? Accommodation { get; set; }                       // e.g., "Hotel + Lodge + Tent"

    // Overview details (the icon row on the page)
    public string? Overview { get; set; }                            // Trip Overview text
    public Country OverviewCountry { get; set; } = Country.Nepal;                  // Nepal
    public string? PeakName { get; set; }                            // Mt. Everest
    public string? Route { get; set; }                               // S-Col; SE-Ridge
    public string? Rank { get; set; }                                // 1
    public decimal? Latitude { get; set; }                           // 27.988056 (example)
    public decimal? Longitude { get; set; }                          // 86.925278 (example)
    public string? CoordinatesText { get; set; }                     // "27°59'17\"N 86°55'31\"E"
    public string? WeatherReportUrl { get; set; }                    // "Live Weather Report" link
    public string? Range { get; set; }                               // Mahalangur Range

    // Hero / media
    public string? HeroImageUrl { get; set; }
    public string? HeroVideoUrl { get; set; }                    // optional, if we later add hero videos   

    // Group size
    public int MinGroupSize { get; set; }
    public int MaxGroupSize { get; set; }
    public string? GroupSizeText { get; set; }                       // "1 - 2 PAX" / "Individual"

    // Price modeling (SST sometimes hides website price)
    public bool PriceOnRequest { get; set; }                         // true when website doesn’t show it
    public decimal? Price { get; set; }                              // make nullable
    public string? CurrencyCode { get; set; }                        // "USD"
    public string? PriceNotesUrl { get; set; }                       // the “why we don’t publish price” PDF
    public string? TripPdfUrl { get; set; }                          // “Trip PDF” link

    // SEO
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

    // Rating summary (page shows “5 Excellent”)
    public decimal? AverageRating { get; set; }                      // 5.0
    public string? RatingLabel { get; set; }                         // "Excellent"
    public int? ReviewCount { get; set; }                            // if you later scrape/count

    // Flags / special service details
    public TravelStatus Status { get; set; } = TravelStatus.Draft;
    public bool Featured { get; set; }
    public int Ordering { get; set; }
    public string? ExpeditionStyle { get; set; }                     // e.g., "Ultimate - VVIP"
    public string? BoardBasis { get; set; }                          // e.g., "Full Board"
    public bool OxygenSupport { get; set; }
    public bool SherpaSupport { get; set; }
    public decimal? SummitBonusUsd { get; set; }
    public string? Permits { get; set; }
    public bool RequiresClimbingPermit { get; set; }
    public List<ExpeditionFaq> Faqs { get; set; } = [];
    public List<ExpeditionMedia> MediaItems { get; set; } = [];
    public List<Itinerary> Itineraries { get; set; } = [];
    public List<ExpeditionMap> Maps { get; set; } = [];
    public List<CostItem> CostItems { get; set; } = [];
    public List<FixedDeparture> FixedDepartures { get; set; } = [];
    public List<GearList> GearLists { get; set; } = [];
    public List<ExpeditionHighlight> Highlights { get; set; } = [];
    public List<ExpeditionReview> Reviews { get; set; } = [];
}










//public sealed class ExpeditionSection : BaseEntity
//{
//    public int ExpeditionId { get; set; }
//    public string SectionType { get; set; } = string.Empty;
//    public string Title { get; set; } = string.Empty;
//    public string? Content { get; set; }
//    public int Ordering { get; set; }

//    public Expedition Expedition { get; set; } = default!;
//}

//public sealed class ExpeditionItineraryDay : BaseEntity
//{
//    public int ExpeditionId { get; set; }
//    public int DayNumber { get; set; }
//    public string Title { get; set; } = string.Empty;
//    public string? Description { get; set; }
//    public string? OvernightLocation { get; set; }

//    public Expedition Expedition { get; set; } = default!;
//}








