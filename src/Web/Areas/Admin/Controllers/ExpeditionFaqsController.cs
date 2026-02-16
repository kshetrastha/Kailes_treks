using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities.Expeditions;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("admin/api/expeditions/{expeditionId:int}/faqs")]
public sealed class ExpeditionFaqsController(AppDbContext db, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(int expeditionId, CancellationToken ct)
    {
        var items = await db.ExpeditionFaqs.AsNoTracking()
            .Where(x => x.ExpeditionId == expeditionId)
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .Select(x => new FaqResponse(x.Id, x.ExpeditionId, x.Question, x.Answer, x.Ordering))
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int expeditionId, [FromBody] FaqRequest request, CancellationToken ct)
    {
        if (!await db.Expeditions.AnyAsync(x => x.Id == expeditionId, ct)) return NotFound();

        var entity = new ExpeditionFaq
        {
            ExpeditionId = expeditionId,
            Question = request.Question.Trim(),
            Answer = request.Answer.Trim(),
            Ordering = request.Ordering,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        db.ExpeditionFaqs.Add(entity);
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(List), new { expeditionId }, new { id = entity.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int expeditionId, int id, [FromBody] FaqRequest request, CancellationToken ct)
    {
        var entity = await db.ExpeditionFaqs.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        entity.Question = request.Question.Trim();
        entity.Answer = request.Answer.Trim();
        entity.Ordering = request.Ordering;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = currentUser.UserId;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int expeditionId, int id, CancellationToken ct)
    {
        var entity = await db.ExpeditionFaqs.FirstOrDefaultAsync(x => x.Id == id && x.ExpeditionId == expeditionId, ct);
        if (entity is null) return NotFound();

        db.ExpeditionFaqs.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    public sealed record FaqRequest(string Question, string Answer, int Ordering);
    public sealed record FaqResponse(int Id, int ExpeditionId, string Question, string Answer, int Ordering);
}
