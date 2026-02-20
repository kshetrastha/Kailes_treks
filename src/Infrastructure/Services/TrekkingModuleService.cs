using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class TrekkingModuleService(AppDbContext db) : ITrekkingModuleService
{
    public async Task<IReadOnlyCollection<TrekkingBasicInfoListItemDto>> ListBasicInfosAsync(CancellationToken ct)
        => await db.TrekkingBasicInfos.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new TrekkingBasicInfoListItemDto(x.Id, x.Name, x.DifficultyLevel.ToString(), x.Duration, x.IsFeatured))
            .ToListAsync(ct);

    public async Task<TrekkingModuleDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(slug)) return null;

        var slugLookup = await db.TrekkingBasicInfos.AsNoTracking()
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(ct);

        var matched = slugLookup.FirstOrDefault(x =>
            string.Equals(TravelSlug.Generate(x.Name), slug, StringComparison.OrdinalIgnoreCase));
        return matched is null ? null : await GetDetailsAsync(matched.Id, ct);
    }

    public async Task<TrekkingModuleDetailsDto?> GetDetailsAsync(int id, CancellationToken ct)
    {
        var x = await db.TrekkingBasicInfos.AsNoTracking()
            .Include(e => e.Overview)
            .Include(e => e.Itineraries.OrderBy(i => i.SeasonTitle).ThenBy(i => i.DayNumber))
            .Include(e => e.InclusionExclusions.OrderBy(i => i.Type).ThenBy(i => i.DisplayOrder))
            .Include(e => e.FixedDepartures.OrderBy(i => i.StartDate))
            .Include(e => e.GearItems.OrderBy(i => i.Category).ThenBy(i => i.DisplayOrder))
            .Include(e => e.Reviews.OrderByDescending(i => i.CreatedAtUtc))
            .Include(e => e.Faqs.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        return x is null ? null : new TrekkingModuleDetailsDto(
            x.Id, x.ExpeditionTypeId, x.Name, x.ShortDescription, x.DifficultyLevel.ToString(), x.MaxElevation, x.Duration, x.WalkingHoursPerDay, x.Accommodation, x.BestSeason, x.GroupSize,
            x.IsFeatured, x.BannerImagePath, x.ThumbnailImagePath,
            x.Overview is null ? null : new TrekkingOverviewUpsertDto(x.Overview.Country, x.Overview.PeakName, x.Overview.Route, x.Overview.Rank, x.Overview.Range, x.Overview.Coordinates, x.Overview.WeatherInformation, x.Overview.FullDescription, x.Overview.MapEmbedCode),
            x.Itineraries.Select(i => new TrekkingItineraryUpsertDto(i.SeasonTitle, i.DayNumber, i.Title, i.ShortDescription, i.FullDescription, i.Accommodation, i.Meals, i.Elevation)).ToList(),
            x.InclusionExclusions.Select(i => new TrekkingInclusionExclusionUpsertDto(i.Type.ToString(), i.Description, i.DisplayOrder)).ToList(),
            x.FixedDepartures.Select(i => new TrekkingFixedDepartureUpsertDto(i.StartDate, i.EndDate, i.TotalSeats, i.BookedSeats, i.Price, i.Currency, i.Status.ToString(), i.IsGuaranteed)).ToList(),
            x.GearItems.Select(i => new TrekkingGearUpsertDto(i.Category.ToString(), i.ItemName, i.IsMandatory, i.DisplayOrder)).ToList(),
            x.Reviews.Select(i => new TrekkingReviewUpsertDto(i.ClientName, i.Country, i.Rating, i.Title, i.Comment, i.ImagePath, i.IsApproved)).ToList(),
            x.Faqs.Select(i => new TrekkingFaqUpsertDto(i.Question, i.Answer, i.DisplayOrder)).ToList());
    }

    public async Task<int> CreateBasicInfoAsync(TrekkingBasicInfoUpsertDto r, int? userId, CancellationToken ct)
    {
        var entity = new TrekkingBasicInfo();
        MapBasic(entity, r, userId, true);
        db.TrekkingBasicInfos.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateBasicInfoAsync(int id, TrekkingBasicInfoUpsertDto r, int? userId, CancellationToken ct)
    {
        var entity = await db.TrekkingBasicInfos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        MapBasic(entity, r, userId, false);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteBasicInfoAsync(int id, CancellationToken ct)
    {
        var entity = await db.TrekkingBasicInfos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        db.TrekkingBasicInfos.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpsertOverviewAsync(int trekkingId, TrekkingOverviewUpsertDto r, int? userId, CancellationToken ct)
    {
        var trekking = await db.TrekkingBasicInfos.Include(x => x.Overview).FirstOrDefaultAsync(x => x.Id == trekkingId, ct);
        if (trekking is null) return false;
        trekking.Overview ??= new TrekkingOverview { TrekkingId = trekkingId, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId };
        trekking.Overview.Country = r.Country;
        trekking.Overview.PeakName = r.PeakName;
        trekking.Overview.Route = r.Route;
        trekking.Overview.Rank = r.Rank;
        trekking.Overview.Range = r.Range;
        trekking.Overview.Coordinates = r.Coordinates;
        trekking.Overview.WeatherInformation = r.WeatherInformation;
        trekking.Overview.FullDescription = r.FullDescription;
        trekking.Overview.MapEmbedCode = r.MapEmbedCode;
        trekking.Overview.UpdatedAtUtc = DateTime.UtcNow;
        trekking.Overview.UpdatedBy = userId;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> ReplaceItinerariesAsync(int trekkingId, IReadOnlyCollection<TrekkingItineraryUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingItineraries,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingItinerary { TrekkingId = trekkingId, SeasonTitle = dto.SeasonTitle, DayNumber = dto.DayNumber, Title = dto.Title, ShortDescription = dto.ShortDescription, FullDescription = dto.FullDescription, Accommodation = dto.Accommodation, Meals = dto.Meals, Elevation = dto.Elevation, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceInclusionExclusionsAsync(int trekkingId, IReadOnlyCollection<TrekkingInclusionExclusionUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingInclusionExclusions,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingInclusionExclusion { TrekkingId = trekkingId, Type = Enum.TryParse<TrekkingInclusionType>(dto.Type, out var t) ? t : TrekkingInclusionType.Inclusion, Description = dto.Description, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceFixedDeparturesAsync(int trekkingId, IReadOnlyCollection<TrekkingFixedDepartureUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingFixedDepartures,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingFixedDeparture { TrekkingId = trekkingId, StartDate = dto.StartDate, EndDate = dto.EndDate, TotalSeats = dto.TotalSeats, BookedSeats = dto.BookedSeats, Price = dto.Price, Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "USD" : dto.Currency, Status = Enum.TryParse<TrekkingDepartureStatus>(dto.Status, out var s) ? s : TrekkingDepartureStatus.BookingOpen, IsGuaranteed = dto.IsGuaranteed, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceGearsAsync(int trekkingId, IReadOnlyCollection<TrekkingGearUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingGears,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingGear { TrekkingId = trekkingId, Category = Enum.TryParse<TrekkingGearCategory>(dto.Category, out var c) ? c : TrekkingGearCategory.Equipment, ItemName = dto.ItemName, IsMandatory = dto.IsMandatory, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceReviewsAsync(int trekkingId, IReadOnlyCollection<TrekkingReviewUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingReviewItems,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingReviewItem { TrekkingId = trekkingId, ClientName = dto.ClientName, Country = dto.Country, Rating = dto.Rating, Title = dto.Title, Comment = dto.Comment, ImagePath = dto.ImagePath, IsApproved = dto.IsApproved, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceFaqsAsync(int trekkingId, IReadOnlyCollection<TrekkingFaqUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(trekkingId, items, ct,
            db.TrekkingFaqItems,
            i => i.TrekkingId == trekkingId,
            dto => new TrekkingFaqItem { TrekkingId = trekkingId, Question = dto.Question, Answer = dto.Answer, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    private async Task<bool> ReplaceAsync<TEntity, TDto>(int trekkingId, IReadOnlyCollection<TDto> items, CancellationToken ct, DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, Func<TDto, TEntity> map)
        where TEntity : class
    {
        var exists = await db.TrekkingBasicInfos.AnyAsync(x => x.Id == trekkingId, ct);
        if (!exists) return false;
        var existing = await set.AsQueryable().Where(predicate).ToListAsync(ct);
        set.RemoveRange(existing);
        set.AddRange(items.Select(map));
        await db.SaveChangesAsync(ct);
        return true;
    }

    private static void MapBasic(TrekkingBasicInfo e, TrekkingBasicInfoUpsertDto r, int? userId, bool isCreate)
    {
        e.ExpeditionTypeId = r.ExpeditionTypeId;
        e.Name = r.Name;
        e.ShortDescription = r.ShortDescription;
        e.DifficultyLevel = Enum.TryParse<DifficultyLevel>(r.DifficultyLevel, out var lvl) ? lvl : DifficultyLevel.Moderate;
        e.MaxElevation = r.MaxElevation;
        e.Duration = r.Duration;
        e.WalkingHoursPerDay = r.WalkingHoursPerDay;
        e.Accommodation = r.Accommodation;
        e.BestSeason = r.BestSeason;
        e.GroupSize = r.GroupSize;
        e.IsFeatured = r.IsFeatured;
        e.BannerImagePath = r.BannerImagePath;
        e.ThumbnailImagePath = r.ThumbnailImagePath;
        e.UpdatedAtUtc = DateTime.UtcNow;
        e.UpdatedBy = userId;
        if (isCreate)
        {
            e.CreatedAtUtc = DateTime.UtcNow;
            e.CreatedBy = userId;
        }
    }
}
