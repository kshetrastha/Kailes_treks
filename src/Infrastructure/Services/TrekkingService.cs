using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class TrekkingService(AppDbContext db) : ITrekkingService
{
    public async Task<TrekkingPagedResult> ListAsync(string? search, string? status, string? destination, bool? featured, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Trekking.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        if (!string.IsNullOrWhiteSpace(destination)) query = query.Where(x => x.Destination == destination);
        if (featured.HasValue) query = query.Where(x => x.Featured == featured.Value);

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(x => x.Ordering).ThenBy(x => x.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new TrekkingListItemDto(x.Id, x.Name, x.Slug, x.Destination, x.DurationDays, x.Price, x.Status, x.Featured, x.Ordering))
            .ToListAsync(ct);

        return new TrekkingPagedResult(items, page, pageSize, total);
    }

    public async Task<int> CreateAsync(TrekkingUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = new Trekking();
        MapCommon(request, entity, userId, isCreate: true);
        entity.TrailGrade = request.TrailGrade;
        entity.TeaHouseAvailable = request.TeaHouseAvailable;
        entity.AccommodationType = request.AccommodationType;
        entity.Meals = request.Meals;
        entity.TransportMode = request.TransportMode;
        entity.TrekPermitType = request.TrekPermitType;

        db.Trekking.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, TrekkingUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = await db.Trekking
            .Include(x => x.ItineraryDays)
            .Include(x => x.Faqs)
            .Include(x => x.MediaItems)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        MapCommon(request, entity, userId, isCreate: false);
        entity.TrailGrade = request.TrailGrade;
        entity.TeaHouseAvailable = request.TeaHouseAvailable;
        entity.AccommodationType = request.AccommodationType;
        entity.Meals = request.Meals;
        entity.TransportMode = request.TransportMode;
        entity.TrekPermitType = request.TrekPermitType;

        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Trekking.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        db.Trekking.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<object?> GetPublicBySlugAsync(string slug, CancellationToken ct)
        => await db.Trekking.AsNoTracking()
            .Include(x => x.ItineraryDays.OrderBy(i => i.DayNumber))
            .Include(x => x.Faqs.OrderBy(f => f.Ordering))
            .Include(x => x.MediaItems.OrderBy(m => m.Ordering))
            .Where(x => x.Slug == slug && x.Status == TravelStatus.Published)
            .Select(x => new
            {
                x.Name,
                x.Slug,
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
                x.TrailGrade,
                x.TeaHouseAvailable,
                x.AccommodationType,
                x.Meals,
                x.TransportMode,
                x.TrekPermitType,
                Itinerary = x.ItineraryDays,
                Faqs = x.Faqs,
                Media = x.MediaItems
            })
            .FirstOrDefaultAsync(ct);

    private void MapCommon(TrekkingUpsertDto request, Trekking entity, int? userId, bool isCreate)
    {
        entity.Name = request.Name.Trim();
        entity.Slug = ResolveSlug(request.Slug, request.Name, entity.Id);
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
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = userId;
        if (isCreate)
        {
            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.CreatedBy = userId;
        }

        entity.ItineraryDays.Clear();
        foreach (var i in request.ItineraryDays)
        {
            entity.ItineraryDays.Add(new TrekkingItineraryDay
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
            entity.Faqs.Add(new TrekkingFaq
            {
                Question = f.Question,
                Answer = f.Answer,
                Ordering = f.Ordering
            });
        }

        entity.MediaItems.Clear();
        foreach (var m in request.MediaItems)
        {
            entity.MediaItems.Add(new TrekkingMedia
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
        while (db.Trekking.Any(x => x.Slug == slug && x.Id != currentId))
        {
            slug = $"{baseSlug}-{suffix++}";
        }

        return slug;
    }
}
