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
    India = 3,
    Bhutan = 4,
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

public enum PilgrimageDestination
{
    // Nepal side
    Kathmandu = 1,
    Pokhara = 2,
    Muktinath = 3,
    Simikot = 4,
    Hilsa = 5,

    // Tibet side
    Lhasa = 6,
    Kerung = 7,
    Sigatshe = 8,
    Saga = 9,
    Manasarovar = 10,
    [Display(Name = "Rakshyas Taal")]
    RakshyasTaal = 11,
    Darchen = 12,
    [Display(Name = "YamaDwar")]
    YamaDwar = 13,
    Zuthulphuk = 14,
    Dheraphuk = 15,
    [Display(Name = "Dolma La")]
    DolmaLa = 16,
    Gaurikunda = 17,
    Ali = 18,
    [Display(Name = "Mount Kailesh")]
    MountKailesh=19,
    Bhutan =20
}
