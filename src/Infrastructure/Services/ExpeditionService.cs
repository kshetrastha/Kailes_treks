using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Domain.Entities.Expeditions;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ExpeditionService(AppDbContext db) : IExpeditionService
{
    public async Task<ExpeditionPagedResult> ListAsync(string? search, string? status, string? destination, bool? featured, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Expeditions.AsNoTracking().Include(x => x.ExpeditionType).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TravelStatus>(status, true, out var parsedStatus))
            query = query.Where(x => x.Status == parsedStatus);
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
                x.Price ?? 0m,
                x.Status.ToString(),
                x.Featured,
                x.Ordering,
                x.ExpeditionTypeId,
                x.ExpeditionType != null ? x.ExpeditionType.Title : null,
                x.DifficultyLevel.HasValue ? x.DifficultyLevel.Value.ToString() : x.Difficulty,
                x.OverviewCountry.ToString()))
            .ToListAsync(ct);

        return new ExpeditionPagedResult(items, page, pageSize, total);
    }

    public async Task<ExpeditionDetailsDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await db.Expeditions.AsNoTracking()
            .Include(x => x.ExpeditionType)
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
            .Include(x => x.Faqs).Include(x => x.MediaItems)
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
            .Include(x => x.Faqs.OrderBy(f => f.Ordering))
            .Include(x => x.MediaItems.OrderBy(m => m.Ordering))
            .Where(x => x.Slug == slug && x.Status == TravelStatus.published)
            .FirstOrDefaultAsync(ct);

        return entity is null ? null : MapDetails(entity);
    }

    private static ExpeditionDetailsDto MapDetails(Expedition x)
        => new(
            x.Id, x.Name, x.Slug, x.ShortDescription, x.Destination, x.Region, x.DurationDays, x.MaxAltitudeMeters, x.MaxAltitudeFeet,
            x.Difficulty, x.BestSeason?.ToString(), x.Overview, x.Inclusions, x.Exclusions, x.HeroImageUrl, x.HeroVideoUrl, x.Permits,
            x.MinGroupSize, x.MaxGroupSize, x.PriceOnRequest, x.Price, x.CurrencyCode, x.PriceNotesUrl, x.TripPdfUrl, x.AvailableDates, x.BookingCtaUrl, x.SeoTitle, x.SeoDescription,
            x.Status.ToString(), x.Featured, x.Ordering, x.SummitRoute, x.RequiresClimbingPermit, x.ExpeditionStyle, x.OxygenSupport,
            x.SherpaSupport, x.SummitBonusUsd, x.ExpeditionTypeId, x.ExpeditionType?.Title,
            [],
            [],
            x.Faqs.Select(f => new ExpeditionFaqDto(f.Id, f.Question, f.Answer, f.Ordering)).ToList(),
            x.MediaItems.Select(m => new ExpeditionMediaDto(m.Id, m.Url, m.Caption, m.MediaType.ToString(), m.Ordering, m.FilePath, m.VideoUrl)).ToList(),
            x.OverviewCountry.ToString(), x.PeakName, x.OverviewDuration, x.Route, x.Rank, x.Latitude, x.Longitude, x.CoordinatesText, x.WeatherReport,
            x.Range, x.WalkingPerDay, x.Accommodation, x.GroupSizeText,
            x.DifficultyLevel?.ToString(), x.BoardBasis, x.AverageRating, x.RatingLabel, x.ReviewCount,
            x.Itineraries.Select(i => new ItineraryDto(i.Id, i.SeasonTitle, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(d.Id, d.ItineraryId, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
            x.Maps.Select(m => new ExpeditionMapDto(m.Id, m.FilePath, m.Title, m.Notes)).ToList(),
            x.CostItems.Select(c => new CostItemDto(c.Id, c.Title, c.ShortDescription, c.IsActive, c.Type.ToString(), c.SortOrder)).ToList(),
            x.FixedDepartures.Select(f => new FixedDepartureDto(f.Id, f.StartDate, f.EndDate, f.ForDays, f.Status, f.GroupSize)).ToList(),
            x.GearLists.Select(g => new GearListDto(g.Id, g.ShortDescription, g.FilePath, g.ImagePath ?? string.Empty)).ToList(),
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
        e.MaxAltitudeFeet = r.MaxAltitudeFeet;
        e.Difficulty = r.Difficulty;
        e.BestSeason = Enum.TryParse<Season>(r.BestSeason, true, out var bestSeason) ? bestSeason : null;
        e.Overview = r.Overview;
        e.Inclusions = r.Inclusions;
        e.Exclusions = r.Exclusions;
        e.HeroImageUrl = r.HeroImageUrl;
        e.HeroVideoUrl = r.HeroVideoUrl;
        e.Permits = r.Permits;
        e.MinGroupSize = r.MinGroupSize;
        e.MaxGroupSize = r.MaxGroupSize;
        e.PriceOnRequest = r.PriceOnRequest;
        e.Price = r.Price;
        e.CurrencyCode = r.CurrencyCode;
        e.PriceNotesUrl = r.PriceNotesUrl;
        e.TripPdfUrl = r.TripPdfUrl;
        e.AvailableDates = r.AvailableDates;
        e.BookingCtaUrl = r.BookingCtaUrl;
        e.SeoTitle = r.SeoTitle;
        e.SeoDescription = r.SeoDescription;
        e.Status = Enum.TryParse<TravelStatus>(r.Status, true, out var status) ? status : TravelStatus.Draft;
        e.Featured = r.Featured;
        e.Ordering = r.Ordering;
        e.SummitRoute = r.SummitRoute;
        e.RequiresClimbingPermit = r.RequiresClimbingPermit;
        e.ExpeditionStyle = r.ExpeditionStyle;
        e.BoardBasis = r.BoardBasis;
        e.OxygenSupport = r.OxygenSupport;
        e.SherpaSupport = r.SherpaSupport;
        e.SummitBonusUsd = r.SummitBonusUsd;
        e.AverageRating = r.AverageRating;
        e.RatingLabel = r.RatingLabel;
        e.ReviewCount = r.ReviewCount;
        e.ExpeditionTypeId = r.ExpeditionTypeId;
        e.OverviewCountry = Enum.TryParse<Country>(r.OverviewCountry, true, out var country) ? country : Country.Nepal;
        e.PeakName = r.PeakName;
        e.OverviewDuration = r.OverviewDuration;
        e.Route = r.Route;
        e.Rank = r.Rank;
        e.Latitude = r.Latitude;
        e.Longitude = r.Longitude;
        e.CoordinatesText = r.CoordinatesText;
        e.WeatherReport = r.WeatherReport;
        e.Range = r.Range;
        e.WalkingPerDay = r.WalkingPerDay;
        e.Accommodation = r.Accommodation;
        e.GroupSizeText = r.GroupSizeText;
        e.DifficultyLevel = Enum.TryParse<DifficultyLevel>(r.DifficultyLevel, true, out var diff) ? diff : null;
        e.UpdatedAtUtc = DateTime.UtcNow;
        e.UpdatedBy = userId;
        if (isCreate) { e.CreatedAtUtc = DateTime.UtcNow; e.CreatedBy = userId; }

        e.Faqs.Clear(); foreach (var f in r.Faqs) e.Faqs.Add(new ExpeditionFaq { Question = f.Question, Answer = f.Answer, Ordering = f.Ordering });
        e.MediaItems.Clear(); foreach (var m in r.MediaItems) e.MediaItems.Add(new ExpeditionMedia { Url = m.Url, Caption = m.Caption, MediaType = m.MediaType, Ordering = m.Ordering, FilePath = m.FilePath, VideoUrl = m.VideoUrl });

        SyncById(e.Itineraries, r.Itineraries ?? [], x => x.Id,
            () => new Itinerary(),
            (entity, dto) =>
            {
                entity.SeasonTitle = dto.SeasonName;
                entity.SortOrder = dto.SortOrder;
                SyncById(entity.Days, dto.Days, d => d.Id,
                    () => new ItineraryDay(),
                    (dayEntity, dayDto) =>
                    {
                        dayEntity.DayNumber = dayDto.DayNumber;
                        dayEntity.ShortDescription = dayDto.ShortDescription;
                        dayEntity.Description = dayDto.Description;
                        dayEntity.Meals = dayDto.Meals;
                        dayEntity.AccommodationType = dayDto.AccommodationType;
                    });
            });

        SyncById(e.Maps, r.Maps ?? [], x => x.Id,
            () => new ExpeditionMap(),
            (entity, dto) =>
            {
                entity.FilePath = dto.FilePath;
                entity.Title = dto.Title;
                entity.Notes = dto.Notes;
            });

        SyncById(e.CostItems, r.CostItems ?? [], x => x.Id,
            () => new CostItem(),
            (entity, dto) =>
            {
                entity.Title = dto.Title;
                entity.ShortDescription = dto.ShortDescription;
                entity.IsActive = dto.IsActive;
                entity.Type = Enum.TryParse<CostItemType>(dto.Type, out var type) ? type : CostItemType.Inclusion;
                entity.SortOrder = dto.SortOrder;
            });

        SyncById(e.FixedDepartures, r.FixedDepartures ?? [], x => x.Id,
            () => new FixedDeparture(),
            (entity, dto) =>
            {
                entity.StartDate = NormalizeDateTimeToUtc(dto.StartDate);
                entity.EndDate = NormalizeDateTimeToUtc(dto.EndDate);
                entity.ForDays = dto.ForDays;
                entity.Status = dto.Status;
                entity.GroupSize = dto.GroupSize;
            });

        SyncById(e.GearLists, r.GearLists ?? [], x => x.Id,
            () => new GearList(),
            (entity, dto) =>
            {
                entity.ShortDescription = dto.ShortDescription;
                entity.FilePath = dto.FilePath;
                entity.ImagePath = dto.ImagePath;
            });

        SyncById(e.Highlights, r.Highlights ?? [], x => x.Id,
            () => new ExpeditionHighlight(),
            (entity, dto) =>
            {
                entity.Text = dto.Text;
                entity.SortOrder = dto.SortOrder;
            });

        SyncById(e.Reviews, r.Reviews ?? [], x => x.Id,
            () => new ExpeditionReview(),
            (entity, dto) =>
            {
                entity.FullName = dto.FullName;
                entity.EmailAddress = dto.EmailAddress;
                entity.UserPhotoPath = dto.UserPhotoPath;
                entity.VideoUrl = dto.VideoUrl;
                entity.Rating = dto.Rating;
                entity.ReviewText = dto.ReviewText;
                entity.ModerationStatus = Enum.TryParse<ReviewModerationStatus>(dto.ModerationStatus, out var ms) ? ms : ReviewModerationStatus.Pending;
            });
    }


    private static void SyncById<TEntity, TDto>(
        IList<TEntity> entities,
        IReadOnlyCollection<TDto> dtos,
        Func<TEntity, int> entityId,
        Func<TEntity> createEntity,
        Action<TEntity, TDto> map)
        where TEntity : class
    {
        var dtoById = dtos
            .Select(dto => (dto, id: GetId(dto)))
            .Where(x => x.id > 0)
            .ToDictionary(x => x.id, x => x.dto);

        for (var i = entities.Count - 1; i >= 0; i--)
        {
            var existing = entities[i];
            var id = entityId(existing);
            if (id > 0 && !dtoById.ContainsKey(id))
            {
                entities.RemoveAt(i);
            }
        }

        foreach (var dto in dtos)
        {
            var id = GetId(dto);
            var existing = id > 0 ? entities.FirstOrDefault(x => entityId(x) == id) : null;
            if (existing is null)
            {
                existing = createEntity();
                entities.Add(existing);
            }

            map(existing, dto);
        }
    }

    private static int GetId<TDto>(TDto dto)
    {
        var prop = typeof(TDto).GetProperty("Id");
        if (prop?.PropertyType != typeof(int)) return 0;
        return (int)(prop.GetValue(dto) ?? 0);
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
