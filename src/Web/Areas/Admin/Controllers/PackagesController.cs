using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/packages")]
public sealed class PackagesController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 10,
        string? name = null,
        int? categoryId = null,
        int? serviceRegionId = null,
        int? difficultyLevelId = null,
        CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, name, categoryId, serviceRegionId, difficultyLevelId, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(
        int page = 1,
        int pageSize = 10,
        string? name = null,
        int? categoryId = null,
        int? serviceRegionId = null,
        int? difficultyLevelId = null,
        CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, name, categoryId, serviceRegionId, difficultyLevelId, ct);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadOptionsAsync(null, null, null, ct);
        return View("Upsert", new PackageFormViewModel());
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.PackageService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        await LoadOptionsAsync(item.CategoryId, item.ServiceRegionId, item.DifficultyLevelId, ct);
        return View("Upsert", new PackageFormViewModel
        {
            Id = item.Id,
            CategoryId = item.CategoryId,
            ServiceRegionId = item.ServiceRegionId,
            DifficultyLevelId = item.DifficultyLevelId,
            Name = item.Name,
            ShortDescription = item.ShortDescription,
            Description = item.Description,
            Price = item.Price,
            IsDiscounted = item.IsDiscounted,
            DiscountedPrice = item.DiscountedPrice,
            Duration = item.Duration,
            DurationType = item.DurationType,
            WalkingPerDay = item.WalkingPerDay,
            MaxGroupSize = item.MaxGroupSize,
            StartingPoint = item.StartingPoint,
            EndingPoint = item.EndingPoint,
            BestSeller = item.BestSeller,
            PopularityRank = item.PopularityRank,
            Availability = item.Availability,
            TotalDistance = item.TotalDistance,
            MaxElevation = item.MaxElevation,
            SlugURL = item.SlugURL
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PackageFormViewModel model, CancellationToken ct)
    {
        await LoadOptionsAsync(model.CategoryId, model.ServiceRegionId, model.DifficultyLevelId, ct);
        if (!await ValidateModelAsync(model, ct)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.PackageService.AddAsync(new Package
        {
            CategoryId = model.CategoryId,
            ServiceRegionId = model.ServiceRegionId,
            DifficultyLevelId = model.DifficultyLevelId,
            Name = model.Name.Trim(),
            ShortDescription = string.IsNullOrWhiteSpace(model.ShortDescription) ? null : model.ShortDescription.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Price = model.Price,
            IsDiscounted = model.IsDiscounted,
            DiscountedPrice = model.IsDiscounted ? model.DiscountedPrice : null,
            Duration = model.Duration,
            DurationType = model.DurationType.Trim(),
            WalkingPerDay = model.WalkingPerDay.Trim(),
            MaxGroupSize = model.MaxGroupSize,
            StartingPoint = model.StartingPoint.Trim(),
            EndingPoint = model.EndingPoint.Trim(),
            BestSeller = model.BestSeller,
            PopularityRank = model.PopularityRank,
            Availability = string.IsNullOrWhiteSpace(model.Availability) ? null : model.Availability.Trim(),
            TotalDistance = model.TotalDistance,
            MaxElevation = string.IsNullOrWhiteSpace(model.MaxElevation) ? null : model.MaxElevation.Trim(),
            SlugURL = string.IsNullOrWhiteSpace(model.SlugURL) ? null : model.SlugURL.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Package created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PackageFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();

        await LoadOptionsAsync(model.CategoryId, model.ServiceRegionId, model.DifficultyLevelId, ct);
        if (!await ValidateModelAsync(model, ct)) return View("Upsert", model);

        var item = await uow.PackageService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.CategoryId = model.CategoryId;
        item.ServiceRegionId = model.ServiceRegionId;
        item.DifficultyLevelId = model.DifficultyLevelId;
        item.Name = model.Name.Trim();
        item.ShortDescription = string.IsNullOrWhiteSpace(model.ShortDescription) ? null : model.ShortDescription.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.Price = model.Price;
        item.IsDiscounted = model.IsDiscounted;
        item.DiscountedPrice = model.IsDiscounted ? model.DiscountedPrice : null;
        item.Duration = model.Duration;
        item.DurationType = model.DurationType.Trim();
        item.WalkingPerDay = model.WalkingPerDay.Trim();
        item.MaxGroupSize = model.MaxGroupSize;
        item.StartingPoint = model.StartingPoint.Trim();
        item.EndingPoint = model.EndingPoint.Trim();
        item.BestSeller = model.BestSeller;
        item.PopularityRank = model.PopularityRank;
        item.Availability = string.IsNullOrWhiteSpace(model.Availability) ? null : model.Availability.Trim();
        item.TotalDistance = model.TotalDistance;
        item.MaxElevation = string.IsNullOrWhiteSpace(model.MaxElevation) ? null : model.MaxElevation.Trim();
        item.SlugURL = string.IsNullOrWhiteSpace(model.SlugURL) ? null : model.SlugURL.Trim();
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Package updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.PackageService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.PackageService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Package deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> BuildIndexResult(
        int page,
        int pageSize,
        string? name,
        int? categoryId,
        int? serviceRegionId,
        int? difficultyLevelId,
        CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        var currentCategoryId = categoryId is > 0 ? categoryId : null;
        var currentServiceRegionId = serviceRegionId is > 0 ? serviceRegionId : null;
        var currentDifficultyLevelId = difficultyLevelId is > 0 ? difficultyLevelId : null;

        ViewBag.PageSize = currentPageSize;
        ViewBag.Name = currentName;

        await LoadOptionsAsync(currentCategoryId, currentServiceRegionId, currentDifficultyLevelId, ct, true);

        var model = await uow.PackageService.ListOrderedAsync(
            currentPage,
            currentPageSize,
            currentName,
            currentCategoryId,
            currentServiceRegionId,
            currentDifficultyLevelId,
            ct);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_PackagesTable", model);
        }

        return View(nameof(Index), model);
    }

    private async Task<bool> ValidateModelAsync(PackageFormViewModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(model.DurationType))
        {
            ModelState.AddModelError(nameof(model.DurationType), "Duration type is required.");
        }

        if (string.IsNullOrWhiteSpace(model.StartingPoint))
        {
            ModelState.AddModelError(nameof(model.StartingPoint), "Starting point is required.");
        }

        if (string.IsNullOrWhiteSpace(model.EndingPoint))
        {
            ModelState.AddModelError(nameof(model.EndingPoint), "Ending point is required.");
        }

        if (model.IsDiscounted)
        {
            if (!model.DiscountedPrice.HasValue)
            {
                ModelState.AddModelError(nameof(model.DiscountedPrice), "Discounted price is required when discounted is enabled.");
            }
            else if (model.DiscountedPrice.Value > model.Price)
            {
                ModelState.AddModelError(nameof(model.DiscountedPrice), "Discounted price must be less than or equal to price.");
            }
        }

        var categoryExists = model.CategoryId > 0 && await uow.CategoryService.AnyAsync(x => x.Id == model.CategoryId, ct);
        if (!categoryExists)
        {
            ModelState.AddModelError(nameof(model.CategoryId), "Select a valid category.");
        }

        var serviceRegionExists = model.ServiceRegionId > 0 && await uow.ServiceRegionService.AnyAsync(x => x.Id == model.ServiceRegionId, ct);
        if (!serviceRegionExists)
        {
            ModelState.AddModelError(nameof(model.ServiceRegionId), "Select a valid service region.");
        }

        var difficultyLevelExists = model.DifficultyLevelId > 0 && await uow.DifficultyLevelService.AnyAsync(x => x.Id == model.DifficultyLevelId, ct);
        if (!difficultyLevelExists)
        {
            ModelState.AddModelError(nameof(model.DifficultyLevelId), "Select a valid difficulty level.");
        }

        return ModelState.IsValid;
    }

    private async Task LoadOptionsAsync(int? categoryId, int? serviceRegionId, int? difficultyLevelId, CancellationToken ct, bool forFilter = false)
    {
        var categoryOptions = await uow.CategoryService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), categoryId == x.Id))
            .ToListAsync(ct);
        categoryOptions.Insert(0, new SelectListItem(forFilter ? "All" : "-- Select Category --", string.Empty, !categoryId.HasValue || categoryId <= 0));
        ViewBag.CategoryOptions = categoryOptions;

        var serviceRegionOptions = await uow.ServiceRegionService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), serviceRegionId == x.Id))
            .ToListAsync(ct);
        serviceRegionOptions.Insert(0, new SelectListItem(forFilter ? "All" : "-- Select Service Region --", string.Empty, !serviceRegionId.HasValue || serviceRegionId <= 0));
        ViewBag.ServiceRegionOptions = serviceRegionOptions;

        var difficultyLevelOptions = await uow.DifficultyLevelService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), difficultyLevelId == x.Id))
            .ToListAsync(ct);
        difficultyLevelOptions.Insert(0, new SelectListItem(forFilter ? "All" : "-- Select Difficulty Level --", string.Empty, !difficultyLevelId.HasValue || difficultyLevelId <= 0));
        ViewBag.DifficultyLevelOptions = difficultyLevelOptions;
    }
}
