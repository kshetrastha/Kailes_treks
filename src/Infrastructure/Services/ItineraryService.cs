using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ItineraryService(AppDbContext db) : IItineraryService
{
    public async Task<IReadOnlyList<ItinerarySummaryDto>> ListAsync(int? expeditionId, CancellationToken ct)
    {
        var query = db.Itineraries
            .AsNoTracking()
            .Include(x => x.Expedition)
            .Include(x => x.Days)
            .AsQueryable();

        if (expeditionId.HasValue)
        {
            query = query.Where(x => x.ExpeditionId == expeditionId.Value);
        }

        return await query
            .OrderBy(x => x.Expedition.Name)
            .ThenBy(x => x.SortOrder)
            .Select(x => new ItinerarySummaryDto(
                x.Id,
                x.ExpeditionId,
                x.Expedition.Name,
                x.SeasonTitle,
                x.SortOrder,
                x.Days.Count))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<ItineraryUpsertItemDto>> ListForExpeditionAsync(int expeditionId, CancellationToken ct)
        => await db.Itineraries
            .AsNoTracking()
            .Where(x => x.ExpeditionId == expeditionId)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Id)
            .Select(x => new ItineraryUpsertItemDto(x.Id, x.SeasonTitle, x.SortOrder))
            .ToListAsync(ct);

    public async Task UpsertForExpeditionAsync(int expeditionId, IReadOnlyCollection<ItineraryUpsertItemDto> items, int? userId, CancellationToken ct)
    {
        var existing = await db.Itineraries
            .Where(x => x.ExpeditionId == expeditionId)
            .ToListAsync(ct);

        var incomingById = items
            .Where(x => x.Id > 0)
            .ToDictionary(x => x.Id, x => x);

        foreach (var row in existing.Where(x => !incomingById.ContainsKey(x.Id)))
        {
            db.Itineraries.Remove(row);
        }

        foreach (var item in items)
        {
            var model = item.Id > 0
                ? existing.FirstOrDefault(x => x.Id == item.Id)
                : null;

            if (model is null)
            {
                model = new Itinerary
                {
                    ExpeditionId = expeditionId,
                    CreatedAtUtc = DateTime.UtcNow,
                    CreatedBy = userId
                };
                db.Itineraries.Add(model);
            }

            model.SeasonTitle = item.SeasonTitle.Trim();
            model.SortOrder = item.SortOrder;
            model.UpdatedAtUtc = DateTime.UtcNow;
            model.UpdatedBy = userId;
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Itineraries.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        db.Itineraries.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
