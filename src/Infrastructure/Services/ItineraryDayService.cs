using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities.Expeditions;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class ItineraryDayService(AppDbContext db) : IItineraryDayService
{
    public async Task<IReadOnlyList<ItineraryDaySummaryDto>> ListAsync(int? itineraryId, CancellationToken ct)
    {
        var query = db.ItineraryDays
            .AsNoTracking()
            .Include(x => x.Itinerary)
            .ThenInclude(x => x.Expedition)
            .AsQueryable();

        if (itineraryId.HasValue)
        {
            query = query.Where(x => x.ItineraryId == itineraryId.Value);
        }

        return await query
            .OrderBy(x => x.Itinerary.Expedition.Name)
            .ThenBy(x => x.Itinerary.SortOrder)
            .ThenBy(x => x.DayNumber)
            .Select(x => new ItineraryDaySummaryDto(
                x.Id,
                x.ItineraryId,
                x.Itinerary.SeasonTitle,
                x.Itinerary.ExpeditionId,
                x.Itinerary.Expedition.Name,
                x.DayNumber,
                x.ShortDescription,
                x.Meals,
                x.AccommodationType))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<ItineraryDayUpsertItemDto>> ListForItineraryAsync(int itineraryId, CancellationToken ct)
        => await db.ItineraryDays
            .AsNoTracking()
            .Where(x => x.ItineraryId == itineraryId)
            .OrderBy(x => x.DayNumber)
            .ThenBy(x => x.Id)
            .Select(x => new ItineraryDayUpsertItemDto(x.Id, x.DayNumber, x.ShortDescription, x.Description, x.Meals, x.AccommodationType))
            .ToListAsync(ct);

    public async Task UpsertForItineraryAsync(int itineraryId, IReadOnlyCollection<ItineraryDayUpsertItemDto> items, int? userId, CancellationToken ct)
    {
        var existing = await db.ItineraryDays
            .Where(x => x.ItineraryId == itineraryId)
            .ToListAsync(ct);

        var incomingById = items
            .Where(x => x.Id > 0)
            .ToDictionary(x => x.Id, x => x);

        foreach (var row in existing.Where(x => !incomingById.ContainsKey(x.Id)))
        {
            db.ItineraryDays.Remove(row);
        }

        foreach (var item in items)
        {
            var model = item.Id > 0
                ? existing.FirstOrDefault(x => x.Id == item.Id)
                : null;

            if (model is null)
            {
                model = new ItineraryDay
                {
                    ItineraryId = itineraryId,
                    CreatedAtUtc = DateTime.UtcNow,
                    CreatedBy = userId
                };
                db.ItineraryDays.Add(model);
            }

            model.DayNumber = item.DayNumber;
            model.ShortDescription = item.ShortDescription?.Trim();
            model.Description = item.Description?.Trim();
            model.Meals = item.Meals?.Trim();
            model.AccommodationType = item.AccommodationType?.Trim();
            model.UpdatedAtUtc = DateTime.UtcNow;
            model.UpdatedBy = userId;
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.ItineraryDays.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        db.ItineraryDays.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
