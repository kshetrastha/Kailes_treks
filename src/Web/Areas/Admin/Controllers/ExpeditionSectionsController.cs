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
[Route("admin/api/expeditions/{expeditionId:int}/sections")]
public sealed class ExpeditionSectionsController(AppDbContext db, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(int expeditionId, CancellationToken ct)
    {
        var exists = await db.Expeditions.AnyAsync(x => x.Id == expeditionId, ct);
        if (!exists) return NotFound();

        var items = await db.ExpeditionSections.AsNoTracking()
            .Where(x => x.ExpeditionId == expeditionId)
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .Select(x => new ExpeditionSectionResponse(x.Id, x.ExpeditionId, x.SectionType, x.Title, x.Content, x.Ordering))
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int expeditionId, [FromBody] ExpeditionSectionRequest request, CancellationToken ct)
    {
        if (!await db.Expeditions.AnyAsync(x => x.Id == expeditionId, ct)) return NotFound();
        if (!ExpeditionSectionTypes.Allowed.Contains(request.SectionType))
            return BadRequest(new { message = "Invalid section type." });

        var entity = new ExpeditionSection
        {
            ExpeditionId = expeditionId,
            SectionType = request.SectionType.Trim().ToLowerInvariant(),
            Title = request.Title.Trim(),
            Content = request.Content,
            Ordering = request.Ordering,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        db.ExpeditionSections.Add(entity);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(List), new { expeditionId }, new { id = entity.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int expeditionId, int id, [FromBody] ExpeditionSectionRequest request, CancellationToken ct)
    {
        var entity = await db.ExpeditionSections.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();
        if (!ExpeditionSectionTypes.Allowed.Contains(request.SectionType))
            return BadRequest(new { message = "Invalid section type." });

        entity.SectionType = request.SectionType.Trim().ToLowerInvariant();
        entity.Title = request.Title.Trim();
        entity.Content = request.Content;
        entity.Ordering = request.Ordering;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = currentUser.UserId;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int expeditionId, int id, CancellationToken ct)
    {
        var entity = await db.ExpeditionSections.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        db.ExpeditionSections.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    public sealed record ExpeditionSectionRequest(string SectionType, string Title, string? Content, int Ordering);
    public sealed record ExpeditionSectionResponse(int Id, int ExpeditionId, string SectionType, string Title, string? Content, int Ordering);
}
