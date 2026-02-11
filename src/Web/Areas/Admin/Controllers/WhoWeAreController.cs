using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
[Route("admin/company/who-we-are")]
public sealed class WhoWeAreController(
    IUnitOfWork unitOfWork,
    IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await unitOfWork.WhoWeAreService.ListOrderedAsync(publishedOnly: false, ct);
        return View(items);
    }

    [HttpGet("header")]
    public async Task<IActionResult> EditHeader(CancellationToken ct)
    {
        var hero = await unitOfWork.WhoWeAreHeroService.GetFirstAsync(asNoTracking: true, ct);

        var model = new WhoWeAreHeroFormViewModel
        {
            Id = hero?.Id,
            Header = hero?.Header ?? "Leading Expedition Operator",
            Title = hero?.Title ?? "Who we are?",
            Description = hero?.Description ?? "",
            ExistingBackgroundImagePath = hero?.BackgroundImagePath
        };

        return View(model);
    }

    [HttpPost("header")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHeader(WhoWeAreHeroFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var hero = await unitOfWork.WhoWeAreHeroService.GetFirstAsync(asNoTracking: false, ct);

        if (hero is null)
        {
            hero = new WhoWeAreHero();
            await unitOfWork.WhoWeAreHeroService.AddAsync(hero, ct);
        }

        hero.Header = model.Header.Trim();
        hero.Title = model.Title.Trim();
        hero.Description = model.Description.Trim();

        if (model.BackgroundImage is { Length: > 0 })
        {
            hero.BackgroundImagePath = await UploadImageAsync(model.BackgroundImage, "who-we-are", "who-we-are-hero", ct);
        }

        hero.UpdatedAtUtc = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Who We Are hero section updated successfully.";
        return RedirectToAction(nameof(EditHeader));
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View("Upsert", new WhoWeAreFormViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WhoWeAreFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new WhoWeAre
        {
            Title = model.Title.Trim(),
            SubDescription = string.IsNullOrWhiteSpace(model.SubDescription) ? null : model.SubDescription.Trim(),
            Description = model.Description.Trim(),
            ImageCaption = string.IsNullOrWhiteSpace(model.ImageCaption) ? null : model.ImageCaption.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await UploadImageAsync(model.Image, "who-we-are", "who-we-are-item", ct);
        }

        await AddAdditionalImagesAsync(entity, model.AdditionalImages, model.AdditionalImageCaptions, ct);

        await unitOfWork.WhoWeAreService.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Who We Are entry created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await unitOfWork.WhoWeAreService.Query()
            .AsNoTracking()
            .Include(x => x.Images.OrderBy(img => img.Ordering).ThenBy(img => img.Id))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new WhoWeAreFormViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            SubDescription = entity.SubDescription,
            Description = entity.Description,
            ImageCaption = entity.ImageCaption,
            ExistingImagePath = entity.ImagePath,
            ExistingAdditionalImages = entity.Images
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new WhoWeAreAdditionalImageViewModel
                {
                    ImagePath = x.ImagePath,
                    Caption = x.Caption,
                    Ordering = x.Ordering
                })
                .ToList(),
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WhoWeAreFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.ExistingImagePath = await unitOfWork.WhoWeAreService.GetImagePathByIdAsync(id, ct);
            model.ExistingAdditionalImages = await unitOfWork.WhoWeAreService.Query()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SelectMany(x => x.Images)
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new WhoWeAreAdditionalImageViewModel
                {
                    ImagePath = x.ImagePath,
                    Caption = x.Caption,
                    Ordering = x.Ordering
                })
                .ToListAsync(ct);
            return View("Upsert", model);
        }

        var entity = await unitOfWork.WhoWeAreService.Query()
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Title = model.Title.Trim();
        entity.SubDescription = string.IsNullOrWhiteSpace(model.SubDescription) ? null : model.SubDescription.Trim();
        entity.Description = model.Description.Trim();
        entity.ImageCaption = string.IsNullOrWhiteSpace(model.ImageCaption) ? null : model.ImageCaption.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await UploadImageAsync(model.Image, "who-we-are", "who-we-are-item", ct);
        }

        if (model.AdditionalImages.Count != 0)
        {
            entity.Images.Clear();
            await AddAdditionalImagesAsync(entity, model.AdditionalImages, model.AdditionalImageCaptions, ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Who We Are entry updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await unitOfWork.WhoWeAreService.Query()
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        unitOfWork.WhoWeAreService.Remove(entity);
        await unitOfWork.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Who We Are entry deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task AddAdditionalImagesAsync(
        WhoWeAre entity,
        IReadOnlyList<IFormFile> images,
        IReadOnlyList<string?> captions,
        CancellationToken ct)
    {
        for (var index = 0; index < images.Count; index++)
        {
            var file = images[index];
            if (file.Length <= 0)
            {
                continue;
            }

            var caption = captions.Count > index ? captions[index] : null;
            var imagePath = await UploadImageAsync(file, "who-we-are", "who-we-are-gallery", ct);
            entity.Images.Add(new WhoWeAreImage
            {
                ImagePath = imagePath,
                Caption = string.IsNullOrWhiteSpace(caption) ? null : caption.Trim(),
                Ordering = index + 1
            });
        }
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
}
