using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/expedition-types")]
public sealed class ExpeditionTypesPageController(IExpeditionTypeService service, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await service.ListAsync(includeUnpublished: true, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new ExpeditionTypeFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new ExpeditionTypeFormViewModel
        {
            Id = item.Id,
            Title = item.Title,
            ShortDescription = item.ShortDescription,
            Description = item.Description,
            Ordering = item.Ordering,
            IsPublished = item.IsPublished
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionTypeFormViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View("Upsert", m);

        await service.CreateAsync(new ExpeditionTypeUpsertDto(m.Title, m.ShortDescription, m.Description, m.Ordering, m.IsPublished), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition type created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionTypeFormViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View("Upsert", m);

        var ok = await service.UpdateAsync(id, new ExpeditionTypeUpsertDto(m.Title, m.ShortDescription, m.Description, m.Ordering, m.IsPublished), currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Expedition type updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await service.DeleteAsync(id, ct);
        if (!ok) TempData["ErrorMessage"] = "Unable to delete expedition type. It's currently in use.";
        else TempData["SuccessMessage"] = "Expedition type deleted.";
        return RedirectToAction(nameof(Index));
    }
}
