using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ExpeditionModuleService(AppDbContext db) : IExpeditionModuleService
{
    public async Task<IReadOnlyCollection<ExpeditionBasicInfoListItemDto>> ListBasicInfosAsync(CancellationToken ct)
        => await db.ExpeditionBasicInfos.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ExpeditionBasicInfoListItemDto(x.Id, x.Name, x.DifficultyLevel.ToString(), x.Duration, x.IsFeatured))
            .ToListAsync(ct);

    public async Task<ExpeditionModuleDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(slug)) return null;

        var slugLookup = await db.ExpeditionBasicInfos.AsNoTracking()
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(ct);

        var matched = slugLookup.FirstOrDefault(x =>
            string.Equals(TravelSlug.Normalize(x.Name), slug, StringComparison.OrdinalIgnoreCase));

        return matched is null ? null : await GetDetailsAsync(matched.Id, ct);
    }

    public async Task<ExpeditionModuleDetailsDto?> GetDetailsAsync(int id, CancellationToken ct)
    {
        var x = await db.ExpeditionBasicInfos.AsNoTracking()
            .Include(e => e.Overview)
            .Include(e => e.Itineraries.OrderBy(i => i.SeasonTitle).ThenBy(i => i.DayNumber))
            .Include(e => e.InclusionExclusions.OrderBy(i => i.Type).ThenBy(i => i.DisplayOrder))
            .Include(e => e.FixedDepartures.OrderBy(i => i.StartDate))
            .Include(e => e.GearItems.OrderBy(i => i.Category).ThenBy(i => i.DisplayOrder))
            .Include(e => e.Reviews.OrderByDescending(i => i.CreatedAtUtc))
            .Include(e => e.Faqs.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        return x is null ? null : new ExpeditionModuleDetailsDto(
            x.Id, x.ExpeditionTypeId, x.Name, x.ShortDescription, x.DifficultyLevel.ToString(), x.MaxElevation, x.Duration, x.WalkingHoursPerDay, x.Accommodation, x.BestSeason, x.GroupSize,
            x.IsFeatured, x.BannerImagePath, x.ThumbnailImagePath,
            x.Overview is null ? null : new ExpeditionOverviewUpsertDto(x.Overview.Country, x.Overview.PeakName, x.Overview.Route, x.Overview.Rank, x.Overview.Range, x.Overview.Coordinates, x.Overview.WeatherInformation, x.Overview.FullDescription, x.Overview.MapEmbedCode),
            x.Itineraries.Select(i => new ExpeditionItineraryUpsertDto(i.SeasonTitle, i.DayNumber, i.Title, i.ShortDescription, i.FullDescription, i.Accommodation, i.Meals, i.Elevation)).ToList(),
            x.InclusionExclusions.Select(i => new ExpeditionInclusionExclusionUpsertDto(i.Type.ToString(), i.Description, i.DisplayOrder)).ToList(),
            x.FixedDepartures.Select(i => new ExpeditionFixedDepartureUpsertDto(i.StartDate, i.EndDate, i.TotalSeats, i.BookedSeats, i.Price, i.Currency, i.Status.ToString(), i.IsGuaranteed)).ToList(),
            x.GearItems.Select(i => new ExpeditionGearUpsertDto(i.Category.ToString(), i.ItemName, i.IsMandatory, i.DisplayOrder)).ToList(),
            x.Reviews.Select(i => new ExpeditionReviewUpsertDto(i.ClientName, i.Country, i.Rating, i.Title, i.Comment, i.ImagePath, i.IsApproved)).ToList(),
            x.Faqs.Select(i => new ExpeditionFaqUpsertDto(i.Question, i.Answer, i.DisplayOrder)).ToList());
    }

    public async Task<int> CreateBasicInfoAsync(ExpeditionBasicInfoUpsertDto r, int? userId, CancellationToken ct)
    {
        var entity = new ExpeditionBasicInfo();
        MapBasic(entity, r, userId, true);
        db.ExpeditionBasicInfos.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateBasicInfoAsync(int id, ExpeditionBasicInfoUpsertDto r, int? userId, CancellationToken ct)
    {
        var entity = await db.ExpeditionBasicInfos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        MapBasic(entity, r, userId, false);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteBasicInfoAsync(int id, CancellationToken ct)
    {
        var entity = await db.ExpeditionBasicInfos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        db.ExpeditionBasicInfos.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpsertOverviewAsync(int expeditionId, ExpeditionOverviewUpsertDto r, int? userId, CancellationToken ct)
    {
        var expedition = await db.ExpeditionBasicInfos.Include(x => x.Overview).FirstOrDefaultAsync(x => x.Id == expeditionId, ct);
        if (expedition is null) return false;
        expedition.Overview ??= new ExpeditionOverview { ExpeditionId = expeditionId, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId };
        expedition.Overview.Country = r.Country;
        expedition.Overview.PeakName = r.PeakName;
        expedition.Overview.Route = r.Route;
        expedition.Overview.Rank = r.Rank;
        expedition.Overview.Range = r.Range;
        expedition.Overview.Coordinates = r.Coordinates;
        expedition.Overview.WeatherInformation = r.WeatherInformation;
        expedition.Overview.FullDescription = r.FullDescription;
        expedition.Overview.MapEmbedCode = r.MapEmbedCode;
        expedition.Overview.UpdatedAtUtc = DateTime.UtcNow;
        expedition.Overview.UpdatedBy = userId;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> ReplaceItinerariesAsync(int expeditionId, IReadOnlyCollection<ExpeditionItineraryUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionItineraries,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionItinerary { ExpeditionId = expeditionId, SeasonTitle = dto.SeasonTitle, DayNumber = dto.DayNumber, Title = dto.Title, ShortDescription = dto.ShortDescription, FullDescription = dto.FullDescription, Accommodation = dto.Accommodation, Meals = dto.Meals, Elevation = dto.Elevation, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceInclusionExclusionsAsync(int expeditionId, IReadOnlyCollection<ExpeditionInclusionExclusionUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionInclusionExclusions,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionInclusionExclusion { ExpeditionId = expeditionId, Type = Enum.TryParse<ExpeditionInclusionType>(dto.Type, out var t) ? t : ExpeditionInclusionType.Inclusion, Description = dto.Description, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceFixedDeparturesAsync(int expeditionId, IReadOnlyCollection<ExpeditionFixedDepartureUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionFixedDepartures,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionFixedDeparture { ExpeditionId = expeditionId, StartDate = dto.StartDate, EndDate = dto.EndDate, TotalSeats = dto.TotalSeats, BookedSeats = dto.BookedSeats, Price = dto.Price, Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "USD" : dto.Currency, Status = Enum.TryParse<ExpeditionDepartureStatus>(dto.Status, out var s) ? s : ExpeditionDepartureStatus.BookingOpen, IsGuaranteed = dto.IsGuaranteed, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceGearsAsync(int expeditionId, IReadOnlyCollection<ExpeditionGearUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionGears,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionGear { ExpeditionId = expeditionId, Category = Enum.TryParse<ExpeditionGearCategory>(dto.Category, out var c) ? c : ExpeditionGearCategory.Equipment, ItemName = dto.ItemName, IsMandatory = dto.IsMandatory, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceReviewsAsync(int expeditionId, IReadOnlyCollection<ExpeditionReviewUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionReviewItems,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionReviewItem { ExpeditionId = expeditionId, ClientName = dto.ClientName, Country = dto.Country, Rating = dto.Rating, Title = dto.Title, Comment = dto.Comment, ImagePath = dto.ImagePath, IsApproved = dto.IsApproved, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    public Task<bool> ReplaceFaqsAsync(int expeditionId, IReadOnlyCollection<ExpeditionFaqUpsertDto> items, int? userId, CancellationToken ct)
        => ReplaceAsync(expeditionId, items, ct,
            db.ExpeditionFaqItems,
            i => i.ExpeditionId == expeditionId,
            dto => new ExpeditionFaqItem { ExpeditionId = expeditionId, Question = dto.Question, Answer = dto.Answer, DisplayOrder = dto.DisplayOrder, CreatedAtUtc = DateTime.UtcNow, CreatedBy = userId });

    private async Task<bool> ReplaceAsync<TEntity, TDto>(int expeditionId, IReadOnlyCollection<TDto> items, CancellationToken ct, DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, Func<TDto, TEntity> map)
        where TEntity : class
    {
        var exists = await db.ExpeditionBasicInfos.AnyAsync(x => x.Id == expeditionId, ct);
        if (!exists) return false;
        var existing = await set.AsQueryable().Where(predicate).ToListAsync(ct);
        set.RemoveRange(existing);
        set.AddRange(items.Select(map));
        await db.SaveChangesAsync(ct);
        return true;
    }

    private static void MapBasic(ExpeditionBasicInfo e, ExpeditionBasicInfoUpsertDto r, int? userId, bool isCreate)
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
