using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/why-with-us")]
public sealed class WhyWithUsController(
    IUnitOfWork unitOfWork,
    IWebHostEnvironment environment) : Controller
{
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await unitOfWork.WhyWithUsService.ListOrderedAsync(publishedOnly: false, ct);

        return View(items);
    }

    [HttpGet("header")]
    public async Task<IActionResult> EditHeader(CancellationToken ct)
    {
        var hero = await unitOfWork.WhyWithUsHeroService.GetFirstAsync(asNoTracking: true, ct);

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
        ValidateHeroModel(model);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var hero = await unitOfWork.WhyWithUsHeroService.GetFirstAsync(asNoTracking: false, ct);

        if (hero is null)
        {
            hero = new WhyWithUsHero();
            await unitOfWork.WhyWithUsHeroService.AddAsync(hero, ct);
        }

        hero.Header = model.Header.Trim();
        hero.Title = model.Title.Trim();
        hero.Description = model.Description.Trim();

        if (model.BackgroundImage is { Length: > 0 })
        {
            hero.BackgroundImagePath = await UploadImageAsync(model.BackgroundImage, "why-with-us", "why-with-us-hero", ct);
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
        ValidateItemModel(model);

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

        await unitOfWork.WhyWithUsService.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await unitOfWork.WhyWithUsService.Query()
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
            ExistingImagePath = entity.ImagePath,
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

        ValidateItemModel(model);

        if (!ModelState.IsValid)
        {
            var existingImagePath = await unitOfWork.WhyWithUsService.GetImagePathByIdAsync(id, ct);

            model.ExistingImagePath = existingImagePath;
            return View("Upsert", model);
        }

        var entity = await unitOfWork.WhyWithUsService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Title = model.Title.Trim();
        entity.Description = model.Description.Trim();
        entity.IconCssClass = string.IsNullOrWhiteSpace(model.IconCssClass) ? null : model.IconCssClass.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await UploadImageAsync(model.Image, "why-with-us", "why-with-us-item", ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await unitOfWork.WhyWithUsService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        unitOfWork.WhyWithUsService.Remove(entity);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Why With Us entry deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> UploadImageAsync(IFormFile image, string relativeDirectory, string fileNamePrefix, CancellationToken ct)
    {
        var fileExtension = Path.GetExtension(image.FileName);
        var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", relativeDirectory);

        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"{fileNamePrefix}-{Guid.NewGuid():N}{fileExtension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await image.CopyToAsync(stream, ct);

        return Path.Combine("uploads", relativeDirectory, fileName).Replace('\\', '/');
    }

    private void ValidateHeroModel(WhyWithUsHeroFormViewModel model)
    {
        ValidateTrimmedRequiredText(nameof(model.Header), model.Header, "Header");
        ValidateTrimmedRequiredText(nameof(model.Title), model.Title, "Title");
        ValidateTrimmedRequiredText(nameof(model.Description), model.Description, "Description");

        if (model.BackgroundImage is not null)
        {
            ValidateImage(model.BackgroundImage, nameof(model.BackgroundImage), "Background image");
        }
    }

    private void ValidateItemModel(WhyWithUsFormViewModel model)
    {
        ValidateTrimmedRequiredText(nameof(model.Title), model.Title, "Title");
        ValidateTrimmedRequiredText(nameof(model.Description), model.Description, "Description");

        if (!string.IsNullOrWhiteSpace(model.IconCssClass) && model.IconCssClass.Contains('<'))
        {
            ModelState.AddModelError(nameof(model.IconCssClass), "Icon CSS class contains invalid characters.");
        }

        if (model.Image is not null)
        {
            ValidateImage(model.Image, nameof(model.Image), "Image");
        }
    }

    private void ValidateTrimmedRequiredText(string propertyName, string? value, string displayName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            ModelState.AddModelError(propertyName, $"{displayName} is required.");
            return;
        }

        if (!string.Equals(value, value.Trim(), StringComparison.Ordinal))
        {
            ModelState.AddModelError(propertyName, $"{displayName} cannot start or end with spaces.");
        }
    }

    private void ValidateImage(IFormFile image, string propertyName, string displayName)
    {
        if (image.Length > MaxImageSizeInBytes)
        {
            ModelState.AddModelError(propertyName, $"{displayName} size must be 5 MB or less.");
        }

        var extension = Path.GetExtension(image.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension))
        {
            ModelState.AddModelError(propertyName, $"{displayName} must be a valid image file (jpg, jpeg, png, webp, gif).");
        }
    }
}
