namespace TravelCleanArch.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "TravelCleanArch";
    public string Audience { get; set; } = "TravelCleanArch";
    public string SigningKey { get; set; } = "CHANGE_ME_32+_CHARS_MIN_SIGNING_KEY";
    public int ExpMinutes { get; set; } = 120;
}
