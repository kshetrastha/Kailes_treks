using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("admin/api/expeditions/{expeditionId:int}/itinerary-days")]
public sealed class ExpeditionItineraryDaysController(AppDbContext db, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(int expeditionId, CancellationToken ct)
    {
        var items = await db.ExpeditionItineraryDays.AsNoTracking()
            .Where(x => x.ExpeditionId == expeditionId)
            .OrderBy(x => x.DayNumber)
            .ThenBy(x => x.Id)
            .Select(x => new ItineraryDayResponse(x.Id, x.ExpeditionId, x.DayNumber, x.Title, x.Description, x.OvernightLocation))
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int expeditionId, [FromBody] ItineraryDayRequest request, CancellationToken ct)
    {
        if (!await db.Expeditions.AnyAsync(x => x.Id == expeditionId, ct)) return NotFound();

        var entity = new ExpeditionItineraryDay
        {
            ExpeditionId = expeditionId,
            DayNumber = request.DayNumber,
            Title = request.Title.Trim(),
            Description = request.Description,
            OvernightLocation = request.OvernightLocation,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        db.ExpeditionItineraryDays.Add(entity);
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(List), new { expeditionId }, new { id = entity.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int expeditionId, int id, [FromBody] ItineraryDayRequest request, CancellationToken ct)
    {
        var entity = await db.ExpeditionItineraryDays.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        entity.DayNumber = request.DayNumber;
        entity.Title = request.Title.Trim();
        entity.Description = request.Description;
        entity.OvernightLocation = request.OvernightLocation;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = currentUser.UserId;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int expeditionId, int id, CancellationToken ct)
    {
        var entity = await db.ExpeditionItineraryDays.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();
        db.ExpeditionItineraryDays.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    public sealed record ItineraryDayRequest(int DayNumber, string Title, string? Description, string? OvernightLocation);
    public sealed record ItineraryDayResponse(int Id, int ExpeditionId, int DayNumber, string Title, string? Description, string? OvernightLocation);
}
