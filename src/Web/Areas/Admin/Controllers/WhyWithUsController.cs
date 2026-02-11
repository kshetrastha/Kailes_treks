using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/why-with-us")]
public sealed class WhyWithUsController(
    IGenericRepository<WhyWithUs> whyWithUsRepository,
    IGenericRepository<WhyWithUsHero> whyWithUsHeroRepository,
    IUnitOfWork unitOfWork,
    IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await whyWithUsRepository.Query()
            .AsNoTracking()
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return View(items);
    }

    [HttpGet("header")]
    public async Task<IActionResult> EditHeader(CancellationToken ct)
    {
        var hero = await whyWithUsHeroRepository.Query()
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(ct);

        var model = new WhyWithUsHeroFormViewModel
        {
            Id = hero?.Id,
            Header = hero?.Header ?? "Because we are the best",
            Title = hero?.Title ?? "Why with us?",
            Description = hero?.Description ?? "",
            ExistingBackgroundImagePath = hero?.BackgroundImagePath
        };

        return View(model);
    }

    [HttpPost("header")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHeader(WhyWithUsHeroFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var hero = await whyWithUsHeroRepository.Query()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (hero is null)
        {
            hero = new WhyWithUsHero();
            await whyWithUsHeroRepository.AddAsync(hero, ct);
        }

        hero.Header = model.Header.Trim();
        hero.Title = model.Title.Trim();
        hero.Description = model.Description.Trim();

        if (model.BackgroundImage is { Length: > 0 })
        {
            var fileExtension = Path.GetExtension(model.BackgroundImage.FileName);
            var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "why-with-us");

            Directory.CreateDirectory(uploadsDirectory);

            var fileName = $"why-with-us-hero-{Guid.NewGuid():N}{fileExtension}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await model.BackgroundImage.CopyToAsync(stream, ct);

            hero.BackgroundImagePath = Path.Combine("uploads", "why-with-us", fileName).Replace('\\', '/');
        }

        hero.UpdatedAtUtc = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us hero section updated successfully.";
        return RedirectToAction(nameof(EditHeader));
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

        await whyWithUsRepository.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await whyWithUsRepository.Query()
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

        var entity = await whyWithUsRepository.GetByIdAsync(id, ct);
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

        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await whyWithUsRepository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        whyWithUsRepository.Remove(entity);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
