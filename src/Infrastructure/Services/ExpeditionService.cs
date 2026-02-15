using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ExpeditionService(AppDbContext db) : IExpeditionService
{
    public async Task<ExpeditionPagedResult> ListAsync(string? search, string? status, string? destination, bool? featured, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Expeditions.AsNoTracking().Include(x => x.ExpeditionType).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        if (!string.IsNullOrWhiteSpace(destination)) query = query.Where(x => x.Destination == destination);
        if (featured.HasValue) query = query.Where(x => x.Featured == featured.Value);

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(x => x.Ordering).ThenBy(x => x.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new ExpeditionListItemDto(
                x.Id,
                x.Name,
                x.HeroImageUrl ?? string.Empty,
                x.Slug,
                x.ShortDescription,
                x.Destination,
                x.DurationDays,
                x.Price,
                x.Status,
                x.Featured,
                x.Ordering,
                x.ExpeditionTypeId,
                x.ExpeditionType != null ? x.ExpeditionType.Title : null,
                x.DifficultyLevel.HasValue ? x.DifficultyLevel.Value.ToString() : x.Difficulty,
                x.OverviewCountry))
            .ToListAsync(ct);

        return new ExpeditionPagedResult(items, page, pageSize, total);
    }

    public async Task<ExpeditionDetailsDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await db.Expeditions.AsNoTracking()
            .Include(x => x.ExpeditionType)
            .Include(x => x.Sections.OrderBy(s => s.Ordering))
            .Include(x => x.ItineraryDays.OrderBy(i => i.DayNumber))
            .Include(x => x.Faqs.OrderBy(f => f.Ordering))
            .Include(x => x.MediaItems.OrderBy(m => m.Ordering))
            .Include(x => x.Itineraries.OrderBy(i => i.SortOrder)).ThenInclude(x => x.Days.OrderBy(d => d.DayNumber))
            .Include(x => x.Maps)
            .Include(x => x.CostItems.OrderBy(c => c.SortOrder))
            .Include(x => x.FixedDepartures.OrderBy(f => f.StartDate))
            .Include(x => x.GearLists)
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder))
            .Include(x => x.Reviews.OrderByDescending(r => r.CreatedAtUtc))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return entity is null ? null : MapDetails(entity);
    }

    public async Task<int> CreateAsync(ExpeditionUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = new Expedition();
        MapCommon(request, entity, userId, true);
        db.Expeditions.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, ExpeditionUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = await db.Expeditions
            .Include(x => x.Sections).Include(x => x.ItineraryDays).Include(x => x.Faqs).Include(x => x.MediaItems)
            .Include(x => x.Itineraries).ThenInclude(i => i.Days)
            .Include(x => x.Maps).Include(x => x.CostItems).Include(x => x.FixedDepartures).Include(x => x.GearLists)
            .Include(x => x.Highlights).Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        MapCommon(request, entity, userId, false);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Expeditions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        db.Expeditions.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<object?> GetPublicBySlugAsync(string slug, CancellationToken ct)
    {
        var entity = await db.Expeditions.AsNoTracking().Include(x => x.ExpeditionType)
            .Include(x => x.Sections.OrderBy(s => s.Ordering))
            .Include(x => x.ItineraryDays.OrderBy(i => i.DayNumber))
            .Include(x => x.Faqs.OrderBy(f => f.Ordering))
            .Include(x => x.MediaItems.OrderBy(m => m.Ordering))
            .Where(x => x.Slug == slug && x.Status == TravelStatus.Published)
            .FirstOrDefaultAsync(ct);

        return entity is null ? null : MapDetails(entity);
    }

    private static ExpeditionDetailsDto MapDetails(Expedition x)
        => new(
            x.Id, x.Name, x.Slug, x.ShortDescription, x.Destination, x.Region, x.DurationDays, x.MaxAltitudeMeters,
            x.Difficulty, x.BestSeason, x.Overview, x.Inclusions, x.Exclusions, x.HeroImageUrl, x.Permits,
            x.MinGroupSize, x.MaxGroupSize, x.Price, x.AvailableDates, x.BookingCtaUrl, x.SeoTitle, x.SeoDescription,
            x.Status, x.Featured, x.Ordering, x.SummitRoute, x.RequiresClimbingPermit, x.ExpeditionStyle, x.OxygenSupport,
            x.SherpaSupport, x.SummitBonusUsd, x.ExpeditionTypeId, x.ExpeditionType?.Title,
            x.Sections.Select(s => new ExpeditionSectionDto(s.SectionType, s.Title, s.Content, s.Ordering)).ToList(),
            x.ItineraryDays.Select(i => new ExpeditionItineraryDayDto(i.DayNumber, i.Title, i.Description, i.OvernightLocation)).ToList(),
            x.Faqs.Select(f => new ExpeditionFaqDto(f.Question, f.Answer, f.Ordering)).ToList(),
            x.MediaItems.Select(m => new ExpeditionMediaDto(m.Url, m.Caption, m.MediaType, m.Ordering, m.FilePath, m.VideoUrl)).ToList(),
            x.OverviewCountry, x.PeakName, x.OverviewDuration, x.Route, x.Rank, x.Latitude, x.Longitude, x.WeatherReport,
            x.Range, x.WalkingPerDay, x.Accommodation, x.GroupSizeText,
            x.DifficultyLevel?.ToString(),
            x.Itineraries.Select(i => new ItineraryDto(i.Id, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(d.Id, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
            x.Maps.Select(m => new ExpeditionMapDto(m.Id, m.FilePath, m.Title, m.Notes)).ToList(),
            x.CostItems.Select(c => new CostItemDto(c.Id, c.Title, c.ShortDescription, c.IsActive, c.Type.ToString(), c.SortOrder)).ToList(),
            x.FixedDepartures.Select(f => new FixedDepartureDto(f.Id, f.StartDate, f.EndDate, f.ForDays, f.Status.ToString(), f.GroupSize)).ToList(),
            x.GearLists.Select(g => new GearListDto(g.Id, g.ShortDescription, g.FilePath)).ToList(),
            x.Highlights.Select(h => new ExpeditionHighlightDto(h.Id, h.Text, h.SortOrder)).ToList(),
            x.Reviews.Select(r => new ExpeditionReviewDto(r.Id, r.FullName, r.EmailAddress, r.UserPhotoPath, r.VideoUrl, r.Rating, r.ReviewText, r.ModerationStatus.ToString())).ToList());

    private void MapCommon(ExpeditionUpsertDto r, Expedition e, int? userId, bool isCreate)
    {
        e.Name = r.Name.Trim();
        e.Slug = ResolveSlug(r.Slug, r.Name, e.Id);
        e.ShortDescription = r.ShortDescription.Trim();
        e.Destination = r.Destination.Trim();
        e.Region = r.Region;
        e.DurationDays = r.DurationDays;
        e.MaxAltitudeMeters = r.MaxAltitudeMeters;
        e.Difficulty = r.Difficulty;
        e.BestSeason = r.BestSeason;
        e.Overview = r.Overview;
        e.Inclusions = r.Inclusions;
        e.Exclusions = r.Exclusions;
        e.HeroImageUrl = r.HeroImageUrl;
        e.Permits = r.Permits;
        e.MinGroupSize = r.MinGroupSize;
        e.MaxGroupSize = r.MaxGroupSize;
        e.Price = r.Price;
        e.AvailableDates = r.AvailableDates;
        e.BookingCtaUrl = r.BookingCtaUrl;
        e.SeoTitle = r.SeoTitle;
        e.SeoDescription = r.SeoDescription;
        e.Status = r.Status;
        e.Featured = r.Featured;
        e.Ordering = r.Ordering;
        e.SummitRoute = r.SummitRoute;
        e.RequiresClimbingPermit = r.RequiresClimbingPermit;
        e.ExpeditionStyle = r.ExpeditionStyle;
        e.OxygenSupport = r.OxygenSupport;
        e.SherpaSupport = r.SherpaSupport;
        e.SummitBonusUsd = r.SummitBonusUsd;
        e.ExpeditionTypeId = r.ExpeditionTypeId;
        e.OverviewCountry = r.OverviewCountry;
        e.PeakName = r.PeakName;
        e.OverviewDuration = r.OverviewDuration;
        e.Route = r.Route;
        e.Rank = r.Rank;
        e.Latitude = r.Latitude;
        e.Longitude = r.Longitude;
        e.WeatherReport = r.WeatherReport;
        e.Range = r.Range;
        e.WalkingPerDay = r.WalkingPerDay;
        e.Accommodation = r.Accommodation;
        e.GroupSizeText = r.GroupSizeText;
        if (Enum.TryParse<DifficultyLevel>(r.DifficultyLevel, out var diff)) e.DifficultyLevel = diff;
        e.UpdatedAtUtc = DateTime.UtcNow;
        e.UpdatedBy = userId;
        if (isCreate) { e.CreatedAtUtc = DateTime.UtcNow; e.CreatedBy = userId; }

        e.Sections.Clear(); foreach (var s in r.Sections) e.Sections.Add(new ExpeditionSection { SectionType = s.SectionType, Title = s.Title, Content = s.Content, Ordering = s.Ordering });
        e.ItineraryDays.Clear(); foreach (var i in r.ItineraryDays) e.ItineraryDays.Add(new ExpeditionItineraryDay { DayNumber = i.DayNumber, Title = i.Title, Description = i.Description, OvernightLocation = i.OvernightLocation });
        e.Faqs.Clear(); foreach (var f in r.Faqs) e.Faqs.Add(new ExpeditionFaq { Question = f.Question, Answer = f.Answer, Ordering = f.Ordering });
        e.MediaItems.Clear(); foreach (var m in r.MediaItems) e.MediaItems.Add(new ExpeditionMedia { Url = m.Url, Caption = m.Caption, MediaType = m.MediaType, Ordering = m.Ordering, FilePath = m.FilePath, VideoUrl = m.VideoUrl });

        e.Itineraries.Clear();
        foreach (var it in r.Itineraries ?? [])
        {
            var itinerary = new Itinerary { SeasonTitle = it.SeasonTitle, SortOrder = it.SortOrder };
            foreach (var d in it.Days) itinerary.Days.Add(new ItineraryDay { DayNumber = d.DayNumber, ShortDescription = d.ShortDescription, Description = d.Description, Meals = d.Meals, AccommodationType = d.AccommodationType });
            e.Itineraries.Add(itinerary);
        }
        e.Maps.Clear(); foreach (var m in r.Maps ?? []) e.Maps.Add(new ExpeditionMap { FilePath = m.FilePath, Title = m.Title, Notes = m.Notes });
        e.CostItems.Clear(); foreach (var c in r.CostItems ?? []) e.CostItems.Add(new CostItem { Title = c.Title, ShortDescription = c.ShortDescription, IsActive = c.IsActive, Type = Enum.TryParse<CostItemType>(c.Type, out var type) ? type : CostItemType.Inclusion, SortOrder = c.SortOrder });
        e.FixedDepartures.Clear(); foreach (var f in r.FixedDepartures ?? []) e.FixedDepartures.Add(new FixedDeparture { StartDate = NormalizeDateTimeToUtc(f.StartDate), EndDate = NormalizeDateTimeToUtc(f.EndDate), ForDays = f.ForDays, Status = Enum.TryParse<DepartureStatus>(f.Status, out var st) ? st : DepartureStatus.BookingOpen, GroupSize = f.GroupSize });
        e.GearLists.Clear(); foreach (var g in r.GearLists ?? []) e.GearLists.Add(new GearList { ShortDescription = g.ShortDescription, FilePath = g.FilePath });
        e.Highlights.Clear(); foreach (var h in r.Highlights ?? []) e.Highlights.Add(new ExpeditionHighlight { Text = h.Text, SortOrder = h.SortOrder });
        e.Reviews.Clear(); foreach (var rv in r.Reviews ?? []) e.Reviews.Add(new ExpeditionReview { FullName = rv.FullName, EmailAddress = rv.EmailAddress, UserPhotoPath = rv.UserPhotoPath, VideoUrl = rv.VideoUrl, Rating = rv.Rating, ReviewText = rv.ReviewText, ModerationStatus = Enum.TryParse<ReviewModerationStatus>(rv.ModerationStatus, out var ms) ? ms : ReviewModerationStatus.Pending });
    }

    private string ResolveSlug(string? customSlug, string name, int currentId)
    {
        var baseSlug = TravelSlug.Generate(string.IsNullOrWhiteSpace(customSlug) ? name : customSlug);
        var slug = baseSlug;
        var suffix = 1;
        while (db.Expeditions.Any(x => x.Slug == slug && x.Id != currentId)) slug = $"{baseSlug}-{suffix++}";
        return slug;
    }

    private static DateTime NormalizeDateTimeToUtc(DateTime value)
        => value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}
