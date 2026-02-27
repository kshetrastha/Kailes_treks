using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/accomodations")]
public sealed class AccomodationsController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? accomodationName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, accomodationName, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(int page = 1, int pageSize = 10, string? accomodationName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, accomodationName, ct);
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new AccomodationFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.AccomodationService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new AccomodationFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccomodationFormViewModel model, CancellationToken ct)
    {
        if (!ValidateName(model)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.AccomodationService.AddAsync(new Accomodation
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Accomodation created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AccomodationFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        if (!ValidateName(model)) return View("Upsert", model);

        var item = await uow.AccomodationService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Accomodation updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.AccomodationService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.AccomodationService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Accomodation deleted.";
        return RedirectToAction(nameof(Index));
    }

    private bool ValidateName(AccomodationFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        return ModelState.IsValid;
    }

    private async Task<IActionResult> BuildIndexResult(int page, int pageSize, string? accomodationName, CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentAccomodationName = string.IsNullOrWhiteSpace(accomodationName) ? null : accomodationName.Trim();

        ViewBag.PageSize = currentPageSize;
        ViewBag.AccomodationName = currentAccomodationName;

        var model = await uow.AccomodationService.ListOrderedAsync(currentPage, currentPageSize, currentAccomodationName, ct);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_AccomodationsTable", model);
        }

        return View(nameof(Index), model);
    }
}
