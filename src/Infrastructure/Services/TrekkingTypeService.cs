using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class TrekkingTypeService(AppDbContext db) : ITrekkingTypeService
{
    public async Task<IReadOnlyCollection<TrekkingTypeDto>> ListAsync(bool includeUnpublished, CancellationToken ct)
    {
        var query = db.TrekkingTypes.AsNoTracking().Include(x => x.Images).AsQueryable();
        if (!includeUnpublished) query = query.Where(x => x.IsPublished);

        return await query.OrderBy(x => x.Ordering).ThenBy(x => x.Title)
            .Select(x => new TrekkingTypeDto(
                x.Id,
                x.Title,
                x.ShortDescription,
                x.Description,
                x.ImagePath,
                x.Ordering,
                x.IsPublished,
                x.Images.OrderBy(i => i.SortOrder).Select(i => new TrekkingTypeImageDto(i.Id, i.FilePath, i.AltText, i.SortOrder, i.IsCover)).ToList()))
            .ToListAsync(ct);
    }

    public async Task<TrekkingTypeDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.TrekkingTypes.AsNoTracking().Include(x => x.Images)
            .Where(x => x.Id == id)
            .Select(x => new TrekkingTypeDto(
                x.Id,
                x.Title,
                x.ShortDescription,
                x.Description,
                x.ImagePath,
                x.Ordering,
                x.IsPublished,
                x.Images.OrderBy(i => i.SortOrder).Select(i => new TrekkingTypeImageDto(i.Id, i.FilePath, i.AltText, i.SortOrder, i.IsCover)).ToList()))
            .FirstOrDefaultAsync(ct);

    public async Task<int> CreateAsync(TrekkingTypeUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = new TrekkingType
        {
            Title = request.Title.Trim(),
            ShortDescription = request.ShortDescription.Trim(),
            Description = request.Description,
            ImagePath = request.ImagePath,
            Ordering = request.Ordering,
            IsPublished = request.IsPublished,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = userId,
            UpdatedBy = userId
        };

        var images = request.Images ?? [];
        foreach (var image in images.OrderBy(x => x.SortOrder))
        {
            entity.Images.Add(new TrekkingTypeImage
            {
                FilePath = image.FilePath,
                AltText = image.AltText,
                SortOrder = image.SortOrder,
                IsCover = image.IsCover
            });
        }

        db.TrekkingTypes.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, TrekkingTypeUpsertDto request, int? userId, CancellationToken ct)
    {
        var entity = await db.TrekkingTypes.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        entity.Title = request.Title.Trim();
        entity.ShortDescription = request.ShortDescription.Trim();
        entity.Description = request.Description;
        entity.ImagePath = request.ImagePath;
        entity.Ordering = request.Ordering;
        entity.IsPublished = request.IsPublished;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = userId;

        entity.Images.Clear();
        var images = request.Images ?? [];
        foreach (var image in images.OrderBy(x => x.SortOrder))
        {
            entity.Images.Add(new TrekkingTypeImage
            {
                FilePath = image.FilePath,
                AltText = image.AltText,
                SortOrder = image.SortOrder,
                IsCover = image.IsCover
            });
        }

        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.TrekkingTypes.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        var hasDependencies = await db.Trekking.AnyAsync(x => x.TrekkingTypeId == id, ct);
        if (hasDependencies) return false;

        db.TrekkingTypes.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
