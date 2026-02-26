namespace TravelCleanArch.Web.Models.Home;

public sealed class ExpeditionModuleDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int? MaxElevation { get; set; }
    public string? BestSeason { get; set; }
    public string? Accommodation { get; set; }
    public string? WalkingHoursPerDay { get; set; }
    public string? GroupSize { get; set; }
    public string? BannerImagePath { get; set; }
    public string? ThumbnailImagePath { get; set; }
    public string? Country { get; set; }
    public string? PeakName { get; set; }
    public string? Route { get; set; }
    public string? Rank { get; set; }
    public string? Range { get; set; }
    public string? Coordinates { get; set; }
    public string? WeatherInformation { get; set; }
    public string? FullDescription { get; set; }
    public string? MapEmbedCode { get; set; }

    public IReadOnlyDictionary<string, List<ExpeditionItineraryRowViewModel>> ItinerariesBySeason { get; set; } = new Dictionary<string, List<ExpeditionItineraryRowViewModel>>();
    public List<string> Inclusions { get; set; } = [];
    public List<string> Exclusions { get; set; } = [];
    public List<ExpeditionDepartureRowViewModel> FixedDepartures { get; set; } = [];
    public IReadOnlyDictionary<string, List<ExpeditionGearRowViewModel>> GearByCategory { get; set; } = new Dictionary<string, List<ExpeditionGearRowViewModel>>();
    public List<ExpeditionReviewRowViewModel> Reviews { get; set; } = [];
    public List<ExpeditionFaqRowViewModel> Faqs { get; set; } = [];
}

public sealed class ExpeditionItineraryRowViewModel
{
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Accommodation { get; set; }
    public string? Meals { get; set; }
    public string? Elevation { get; set; }
}

public sealed class ExpeditionDepartureRowViewModel
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalSeats { get; set; }
    public int BookedSeats { get; set; }
    public int RemainingSeats { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = string.Empty;
    public bool IsGuaranteed { get; set; }
}

public sealed class ExpeditionGearRowViewModel
{
    public string ItemName { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
}

public sealed class ExpeditionReviewRowViewModel
{
    public string ClientName { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
}

public sealed class ExpeditionFaqRowViewModel
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
