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
[Route("admin/categories")]
public sealed class CategoriesController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? categoryName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, categoryName, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(int page = 1, int pageSize = 10, string? categoryName = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, categoryName, ct);
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new CategoryFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.CategoryService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new CategoryFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model, CancellationToken ct)
    {
        if (!ValidateName(model)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.CategoryService.AddAsync(new Category
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Category created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        if (!ValidateName(model)) return View("Upsert", model);

        var item = await uow.CategoryService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Category updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.CategoryService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.CategoryService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Category deleted.";
        return RedirectToAction(nameof(Index));
    }

    private bool ValidateName(CategoryFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        return ModelState.IsValid;
    }

    private async Task<IActionResult> BuildIndexResult(int page, int pageSize, string? categoryName, CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentCategoryName = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName.Trim();

        ViewBag.PageSize = currentPageSize;
        ViewBag.CategoryName = currentCategoryName;

        var model = await uow.CategoryService.ListOrderedAsync(currentPage, currentPageSize, currentCategoryName, ct);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_CategoriesTable", model);
        }

        return View(nameof(Index), model);
    }
}
