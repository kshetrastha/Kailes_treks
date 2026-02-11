using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/why-with-us")]
public sealed class WhyWithUsController(AppDbContext dbContext) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await dbContext.WhyWithUs
            .AsNoTracking()
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return View(items);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View("Upsert", new WhyWithUsFormViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WhyWithUsFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new WhyWithUs
        {
            Title = model.Title.Trim(),
            Description = model.Description.Trim(),
            IconCssClass = string.IsNullOrWhiteSpace(model.IconCssClass) ? null : model.IconCssClass.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        dbContext.WhyWithUs.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await dbContext.WhyWithUs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new WhyWithUsFormViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            IconCssClass = entity.IconCssClass,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WhyWithUsFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await dbContext.WhyWithUs.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Title = model.Title.Trim();
        entity.Description = model.Description.Trim();
        entity.IconCssClass = string.IsNullOrWhiteSpace(model.IconCssClass) ? null : model.IconCssClass.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await dbContext.WhyWithUs.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.WhyWithUs.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
