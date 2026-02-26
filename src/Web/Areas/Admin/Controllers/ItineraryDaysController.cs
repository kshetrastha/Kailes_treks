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
[Route("admin/itinerary-days")]
public sealed class ItineraryDaysController(
    IItineraryDayService itineraryDayService,
    IItineraryService itineraryService,
    ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int? itineraryId, CancellationToken ct)
    {
        ViewBag.ItineraryId = itineraryId;
        ViewBag.Itineraries = await GetItineraryOptionsAsync(ct, itineraryId);
        return View(await itineraryDayService.ListAsync(itineraryId, ct));
    }

    [HttpGet("upsert")]
    public async Task<IActionResult> Upsert(int? itineraryId, CancellationToken ct)
    {
        if (!itineraryId.HasValue)
        {
            return View(new ItineraryDayBulkFormViewModel { ItineraryOptions = await GetItineraryOptionsAsync(ct) });
        }

        var rows = (await itineraryDayService.ListForItineraryAsync(itineraryId.Value, ct))
            .Select(x => new ItineraryDayRowInput
            {
                Id = x.Id,
                DayNumber = x.DayNumber,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Meals = x.Meals,
                AccommodationType = x.AccommodationType
            })
            .ToList();

        if (rows.Count == 0)
        {
            rows.Add(new ItineraryDayRowInput());
        }

        return View(new ItineraryDayBulkFormViewModel
        {
            ItineraryId = itineraryId.Value,
            ItineraryOptions = await GetItineraryOptionsAsync(ct, itineraryId),
            Rows = rows
        });
    }

    [HttpPost("upsert"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ItineraryDayBulkFormViewModel model, CancellationToken ct)
    {
        model.Rows = model.Rows
            .Where(x => x.DayNumber > 0 && (!string.IsNullOrWhiteSpace(x.ShortDescription) || !string.IsNullOrWhiteSpace(x.Description)))
            .ToList();

        if (model.Rows.Count == 0)
        {
            ModelState.AddModelError(nameof(model.Rows), "Add at least one itinerary day row.");
        }

        if (!ModelState.IsValid)
        {
            model.ItineraryOptions = await GetItineraryOptionsAsync(ct, model.ItineraryId);
            return View(model);
        }

        var items = model.Rows
            .Select(x => new ItineraryDayUpsertItemDto(x.Id, x.DayNumber, x.ShortDescription, x.Description, x.Meals, x.AccommodationType))
            .ToList();

        await itineraryDayService.UpsertForItineraryAsync(model.ItineraryId, items, currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Itinerary days saved.";
        return RedirectToAction(nameof(Index), new { itineraryId = model.ItineraryId });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await itineraryDayService.DeleteAsync(id, ct);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetItineraryOptionsAsync(CancellationToken ct, int? selected = null)
    {
        var rows = await itineraryService.ListAsync(null, ct);
        return rows
            .Select(x => new SelectListItem($"{x.ExpeditionName} - {x.SeasonTitle}", x.Id.ToString(), selected == x.Id))
            .ToList();
    }
}
