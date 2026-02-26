using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/reviews")]
public sealed class ReviewsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await uow.ReviewService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new ReviewFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.ReviewService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new ReviewFormViewModel
        {
            Id = entity.Id,
            ReviewerName = entity.ReviewerName,
            ReviewerRole = entity.ReviewerRole,
            ReviewText = entity.ReviewText,
            Rating = entity.Rating,
            SourceName = entity.SourceName,
            SourceUrl = entity.SourceUrl,
            ReviewedOnUtc = entity.ReviewedOnUtc?.Date,
            ExistingImagePath = entity.ReviewerImagePath,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new Review
        {
            ReviewerName = model.ReviewerName.Trim(),
            ReviewerRole = model.ReviewerRole?.Trim(),
            ReviewText = model.ReviewText.Trim(),
            Rating = model.Rating,
            SourceName = model.SourceName?.Trim(),
            SourceUrl = model.SourceUrl?.Trim(),
            ReviewedOnUtc = NormalizeDateToUtc(model.ReviewedOnUtc),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.Image is { Length: > 0 })
        {
            entity.ReviewerImagePath = await SaveFileAsync(model.Image, ct);
        }

        await uow.ReviewService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Review created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReviewFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await uow.ReviewService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.ReviewerName = model.ReviewerName.Trim();
        entity.ReviewerRole = model.ReviewerRole?.Trim();
        entity.ReviewText = model.ReviewText.Trim();
        entity.Rating = model.Rating;
        entity.SourceName = model.SourceName?.Trim();
        entity.SourceUrl = model.SourceUrl?.Trim();
        entity.ReviewedOnUtc = NormalizeDateToUtc(model.ReviewedOnUtc);
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ReviewerImagePath = await SaveFileAsync(model.Image, ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Review updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.ReviewService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        uow.ReviewService.Remove(entity);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Review deleted.";
        return RedirectToAction(nameof(Index));
    }

    private static DateTime? NormalizeDateToUtc(DateTime? value) =>
        value.HasValue ? DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc) : null;

    private async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "reviews");
        Directory.CreateDirectory(folder);
        var name = $"review-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "reviews", name).Replace('\\', '/');
    }
}
