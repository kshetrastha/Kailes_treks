using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/itineraries")]
public sealed class ItinerariesController(
    IItineraryService itineraryService,
    IExpeditionService expeditionService,
    ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int? expeditionId, CancellationToken ct)
    {
        ViewBag.ExpeditionId = expeditionId;
        ViewBag.Expeditions = await GetExpeditionOptionsAsync(ct, expeditionId);
        return View(await itineraryService.ListAsync(expeditionId, ct));
    }

    [HttpGet("upsert")]
    public async Task<IActionResult> Upsert(int? expeditionId, CancellationToken ct)
    {
        if (!expeditionId.HasValue)
        {
            var options = await GetExpeditionOptionsAsync(ct);
            return View(new ItineraryBulkFormViewModel { ExpeditionOptions = options });
        }

        var rows = (await itineraryService.ListForExpeditionAsync(expeditionId.Value, ct))
            .Select(x => new ItineraryRowInput { Id = x.Id, SeasonTitle = x.SeasonTitle, SortOrder = x.SortOrder })
            .ToList();

        if (rows.Count == 0)
        {
            rows.Add(new ItineraryRowInput());
        }

        return View(new ItineraryBulkFormViewModel
        {
            ExpeditionId = expeditionId.Value,
            ExpeditionOptions = await GetExpeditionOptionsAsync(ct, expeditionId),
            Rows = rows
        });
    }

    [HttpPost("upsert"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ItineraryBulkFormViewModel model, CancellationToken ct)
    {
        model.Rows = model.Rows
            .Where(x => !string.IsNullOrWhiteSpace(x.SeasonTitle))
            .ToList();

        if (model.Rows.Count == 0)
        {
            ModelState.AddModelError(nameof(model.Rows), "Add at least one itinerary row.");
        }

        if (!ModelState.IsValid)
        {
            model.ExpeditionOptions = await GetExpeditionOptionsAsync(ct, model.ExpeditionId);
            return View(model);
        }

        var items = model.Rows
            .Select(x => new ItineraryUpsertItemDto(x.Id, x.SeasonTitle, x.SortOrder))
            .ToList();

        await itineraryService.UpsertForExpeditionAsync(model.ExpeditionId, items, currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Itineraries saved.";
        return RedirectToAction(nameof(Index), new { expeditionId = model.ExpeditionId });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await itineraryService.DeleteAsync(id, ct);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetExpeditionOptionsAsync(CancellationToken ct, int? selected = null)
    {
        var paged = await expeditionService.ListAsync(null, null, null, null, 1, 1000, ct);
        return paged.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), selected == x.Id))
            .ToList();
    }
}
