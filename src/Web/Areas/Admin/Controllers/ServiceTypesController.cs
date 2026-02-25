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
[Route("admin/service-types")]
public sealed class ServiceTypesController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        ViewBag.PageSize = currentPageSize;

        return View(await uow.ServiceTypeService.ListOrderedAsync(currentPage, currentPageSize, ct));
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new ServiceTypeFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.ServiceTypeService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new ServiceTypeFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceTypeFormViewModel model, CancellationToken ct)
    {
        if (!ValidateName(model)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.ServiceTypeService.AddAsync(new ServiceType
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service type created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceTypeFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        if (!ValidateName(model)) return View("Upsert", model);

        var item = await uow.ServiceTypeService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service type updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.ServiceTypeService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.ServiceTypeService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Service type deleted.";
        return RedirectToAction(nameof(Index));
    }

    private bool ValidateName(ServiceTypeFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        return ModelState.IsValid;
    }
}
