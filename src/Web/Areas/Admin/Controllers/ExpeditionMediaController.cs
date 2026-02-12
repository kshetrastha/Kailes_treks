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
[Route("admin/api/expeditions/{expeditionId:int}/media")]
public sealed class ExpeditionMediaController(AppDbContext db, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(int expeditionId, CancellationToken ct)
    {
        var items = await db.ExpeditionMedia.AsNoTracking()
            .Where(x => x.ExpeditionId == expeditionId)
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .Select(x => new MediaResponse(x.Id, x.ExpeditionId, x.Url, x.Caption, x.MediaType, x.Ordering))
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int expeditionId, [FromBody] MediaRequest request, CancellationToken ct)
    {
        if (!await db.Expeditions.AnyAsync(x => x.Id == expeditionId, ct)) return NotFound();

        var entity = new ExpeditionMedia
        {
            ExpeditionId = expeditionId,
            Url = request.Url.Trim(),
            Caption = request.Caption,
            MediaType = request.MediaType.Trim(),
            Ordering = request.Ordering,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        db.ExpeditionMedia.Add(entity);
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(List), new { expeditionId }, new { id = entity.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int expeditionId, int id, [FromBody] MediaRequest request, CancellationToken ct)
    {
        var entity = await db.ExpeditionMedia.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        entity.Url = request.Url.Trim();
        entity.Caption = request.Caption;
        entity.MediaType = request.MediaType.Trim();
        entity.Ordering = request.Ordering;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = currentUser.UserId;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int expeditionId, int id, CancellationToken ct)
    {
        var entity = await db.ExpeditionMedia.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        db.ExpeditionMedia.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    public sealed record MediaRequest(string Url, string? Caption, string MediaType, int Ordering);
    public sealed record MediaResponse(int Id, int ExpeditionId, string Url, string? Caption, string MediaType, int Ordering);
}
