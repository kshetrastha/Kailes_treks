namespace TravelCleanArch.Web.Models.Expeditions
{
    public class ItineraryCreateUpdateModel
    {
        public int? Id { get; set; }

        public string SeasonName { get; set; } = string.Empty; // Spring 2026
        public int Day { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string? Accommodation { get; set; }
    }

}