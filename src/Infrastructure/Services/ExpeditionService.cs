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
                x.HeroImageUrl,
                x.Slug,
                x.ShortDescription,
                x.Destination,
                x.DurationDays,
                x.Price,
                x.Status,
                x.Featured,
                x.Ordering,
                x.ExpeditionTypeId,
                x.ExpeditionType != null ? x.ExpeditionType.Title : null))
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
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);

        return entity is null ? null : MapDetails(entity);
    }

    public async Task<int> CreateAsync(ExpeditionUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = new Expedition();
        MapCommon(request, entity, userId, isCreate: true);

        db.Expeditions.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, ExpeditionUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = await db.Expeditions
            .Include(x => x.Sections)
            .Include(x => x.ItineraryDays)
            .Include(x => x.Faqs)
            .Include(x => x.MediaItems)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        MapCommon(request, entity, userId, isCreate: false);

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
        var entity = await db.Expeditions.AsNoTracking()
            .Include(x => x.ExpeditionType)
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
            x.Id,
            x.Name,
            x.Slug,
            x.ShortDescription,
            x.Destination,
            x.Region,
            x.DurationDays,
            x.MaxAltitudeMeters,
            x.Difficulty,
            x.BestSeason,
            x.Overview,
            x.Inclusions,
            x.Exclusions,
            x.HeroImageUrl,
            x.Permits,
            x.MinGroupSize,
            x.MaxGroupSize,
            x.Price,
            x.AvailableDates,
            x.BookingCtaUrl,
            x.SeoTitle,
            x.SeoDescription,
            x.Status,
            x.Featured,
            x.Ordering,
            x.SummitRoute,
            x.RequiresClimbingPermit,
            x.ExpeditionStyle,
            x.OxygenSupport,
            x.SherpaSupport,
            x.SummitBonusUsd,
            x.ExpeditionTypeId,
            x.ExpeditionType != null ? x.ExpeditionType.Title : null,
            x.Sections.Select(s => new ExpeditionSectionDto(s.SectionType, s.Title, s.Content, s.Ordering)).ToList(),
            x.ItineraryDays.Select(i => new ExpeditionItineraryDayDto(i.DayNumber, i.Title, i.Description, i.OvernightLocation)).ToList(),
            x.Faqs.Select(f => new ExpeditionFaqDto(f.Question, f.Answer, f.Ordering)).ToList(),
            x.MediaItems.Select(m => new ExpeditionMediaDto(m.Url, m.Caption, m.MediaType, m.Ordering)).ToList());

    private void MapCommon(ExpeditionUpsertDto request, Expedition entity, int? userId, bool isCreate)
    {
        entity.Name = request.Name.Trim();
        entity.Slug = ResolveSlug(request.Slug, request.Name, entity.Id);
        entity.ShortDescription = request.ShortDescription.Trim();
        entity.Destination = request.Destination.Trim();
        entity.Region = request.Region;
        entity.DurationDays = request.DurationDays;
        entity.MaxAltitudeMeters = request.MaxAltitudeMeters;
        entity.Difficulty = request.Difficulty;
        entity.BestSeason = request.BestSeason;
        entity.Overview = request.Overview;
        entity.Inclusions = request.Inclusions;
        entity.Exclusions = request.Exclusions;
        entity.HeroImageUrl = request.HeroImageUrl;
        entity.Permits = request.Permits;
        entity.MinGroupSize = request.MinGroupSize;
        entity.MaxGroupSize = request.MaxGroupSize;
        entity.Price = request.Price;
        entity.AvailableDates = request.AvailableDates;
        entity.BookingCtaUrl = request.BookingCtaUrl;
        entity.SeoTitle = request.SeoTitle;
        entity.SeoDescription = request.SeoDescription;
        entity.Status = request.Status;
        entity.Featured = request.Featured;
        entity.Ordering = request.Ordering;
        entity.SummitRoute = request.SummitRoute;
        entity.RequiresClimbingPermit = request.RequiresClimbingPermit;
        entity.ExpeditionStyle = request.ExpeditionStyle;
        entity.OxygenSupport = request.OxygenSupport;
        entity.SherpaSupport = request.SherpaSupport;
        entity.SummitBonusUsd = request.SummitBonusUsd;
        entity.ExpeditionTypeId = request.ExpeditionTypeId;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = userId;
        if (isCreate)
        {
            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.CreatedBy = userId;
        }

        entity.Sections.Clear();
        foreach (var s in request.Sections)
        {
            entity.Sections.Add(new ExpeditionSection
            {
                SectionType = s.SectionType,
                Title = s.Title,
                Content = s.Content,
                Ordering = s.Ordering
            });
        }

        entity.ItineraryDays.Clear();
        foreach (var i in request.ItineraryDays)
        {
            entity.ItineraryDays.Add(new ExpeditionItineraryDay
            {
                DayNumber = i.DayNumber,
                Title = i.Title,
                Description = i.Description,
                OvernightLocation = i.OvernightLocation
            });
        }

        entity.Faqs.Clear();
        foreach (var f in request.Faqs)
        {
            entity.Faqs.Add(new ExpeditionFaq
            {
                Question = f.Question,
                Answer = f.Answer,
                Ordering = f.Ordering
            });
        }

        entity.MediaItems.Clear();
        foreach (var m in request.MediaItems)
        {
            entity.MediaItems.Add(new ExpeditionMedia
            {
                Url = m.Url,
                Caption = m.Caption,
                MediaType = m.MediaType,
                Ordering = m.Ordering
            });
        }
    }

    private string ResolveSlug(string? customSlug, string name, int currentId)
    {
        var baseSlug = TravelSlug.Generate(string.IsNullOrWhiteSpace(customSlug) ? name : customSlug);
        var slug = baseSlug;
        var suffix = 1;
        while (db.Expeditions.Any(x => x.Slug == slug && x.Id != currentId))
        {
            slug = $"{baseSlug}-{suffix++}";
        }

        return slug;
    }
}
