using System.ComponentModel.DataAnnotations;

namespace TravelCleanArch.Domain.Enumerations;

internal class Enumeration
{
}

public enum TravelStatus
{
    Draft = 1,  
    published = 2,  
    Arrived = 3,
    Deleted = 4
}
public enum Country
{
    Nepal = 1,
    Tibet = 2,
    India = 3
}
public enum Season
{
    Spring = 1,
    Summer = 2,
    Autumn = 3,
    Winter = 4
}
public enum DifficultyLevel
{
    Easy = 1,
    Moderate = 2,
    Difficult = 3,
    [Display(Name = "Hard Difficult")]
    HardDifficult = 4,
    [Display(Name = "Very Hard")]
    VeryHard = 5
}
public enum CostItemType
{
    Inclusion = 1,
    Exclusion = 2
}

public enum DepartureStatus
{
    BookingCreated = 1,
    BookingOpen = 2,
    BookingClosed = 3,
    BookingHold = 4
}

public enum ReviewModerationStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum ExpeditionMediaType
{
    Photo = 1,
    Video = 2
}
