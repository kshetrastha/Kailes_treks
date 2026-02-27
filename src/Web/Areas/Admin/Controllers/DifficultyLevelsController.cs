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
[Route("admin/difficulty-levels")]
public sealed class DifficultyLevelsController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? levelName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, levelName, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(int page = 1, int pageSize = 10, string? levelName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, levelName, ct);
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new DifficultyLevelFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.DifficultyLevelService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new DifficultyLevelFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Ordering = item.Ordering
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DifficultyLevelFormViewModel model, CancellationToken ct)
    {
        if (!ValidateName(model)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.DifficultyLevelService.AddAsync(new DifficultyLevel
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Ordering = model.Ordering,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Difficulty level created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DifficultyLevelFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        if (!ValidateName(model)) return View("Upsert", model);

        var item = await uow.DifficultyLevelService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.Ordering = model.Ordering;
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Difficulty level updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.DifficultyLevelService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.DifficultyLevelService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Difficulty level deleted.";
        return RedirectToAction(nameof(Index));
    }

    private bool ValidateName(DifficultyLevelFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        return ModelState.IsValid;
    }

    private async Task<IActionResult> BuildIndexResult(int page, int pageSize, string? levelName, CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentLevelName = string.IsNullOrWhiteSpace(levelName) ? null : levelName.Trim();

        ViewBag.PageSize = currentPageSize;
        ViewBag.LevelName = currentLevelName;

        var model = await uow.DifficultyLevelService.ListOrderedAsync(currentPage, currentPageSize, currentLevelName, ct);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_DifficultyLevelsTable", model);
        }

        return View(nameof(Index), model);
    }
}
