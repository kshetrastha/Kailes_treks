using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/banners")]
public sealed class BannersController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await uow.BannerService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new BannerFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.BannerService.Query().Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        return View("Upsert", new BannerFormViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            SubDescription = entity.SubDescription,
            Description = entity.Description,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished,
            Images = entity.Images
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new BannerImageInput
                {
                    Id = x.Id,
                    ExistingImagePath = x.ImagePath,
                    SubDescription = x.SubDescription,
                    Description = x.Description,
                    Ordering = x.Ordering,
                    IsPublished = x.IsPublished
                })
                .ToList()
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BannerFormViewModel model, CancellationToken ct)
    {
        HydrateImageFilesFromRequest(model);
        ValidateImages(model);
        if (!ModelState.IsValid) return View("Upsert", model);

        var now = DateTime.UtcNow;
        var entity = new Banner
        {
            Title = model.Title.Trim(),
            SubDescription = model.SubDescription?.Trim(),
            Description = model.Description?.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var images = await BuildImagesAsync(model.Images, ct);
        foreach (var image in images)
        {
            image.CreatedAtUtc = now;
            image.UpdatedAtUtc = now;
            entity.Images.Add(image);
        }

        await uow.BannerService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Banner created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BannerFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();

        HydrateImageFilesFromRequest(model);
        ValidateImages(model);
        if (!ModelState.IsValid) return View("Upsert", model);

        var entity = await uow.BannerService.Query().Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        entity.Title = model.Title.Trim();
        entity.SubDescription = model.SubDescription?.Trim();
        entity.Description = model.Description?.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        var retained = model.Images.Where(x => !x.Remove).ToList();
        var removeIds = entity.Images.Select(x => x.Id).Except(retained.Where(x => x.Id.HasValue).Select(x => x.Id!.Value)).ToHashSet();
        var toDelete = entity.Images.Where(x => removeIds.Contains(x.Id)).ToList();
        foreach (var item in toDelete)
        {
            DeletePhysicalFile(item.ImagePath);
            entity.Images.Remove(item);
        }

        foreach (var row in retained)
        {
            if (row.Id is > 0)
            {
                var current = entity.Images.FirstOrDefault(x => x.Id == row.Id.Value);
                if (current is null) continue;
                if (row.Image is { Length: > 0 })
                {
                    DeletePhysicalFile(current.ImagePath);
                    current.ImagePath = await SaveFileAsync(row.Image, "banners", "banner-image", ct);
                }

                current.SubDescription = row.SubDescription?.Trim();
                current.Description = row.Description?.Trim();
                current.Ordering = row.Ordering;
                current.IsPublished = row.IsPublished;
                current.UpdatedAtUtc = DateTime.UtcNow;
                continue;
            }

            if (row.Image is not { Length: > 0 }) continue;
            entity.Images.Add(new BannerImage
            {
                ImagePath = await SaveFileAsync(row.Image, "banners", "banner-image", ct),
                SubDescription = row.SubDescription?.Trim(),
                Description = row.Description?.Trim(),
                Ordering = row.Ordering,
                IsPublished = row.IsPublished,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });
        }

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Banner updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.BannerService.Query().Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        foreach (var image in entity.Images)
        {
            DeletePhysicalFile(image.ImagePath);
        }

        uow.BannerService.Remove(entity);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Banner deleted.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidateImages(BannerFormViewModel model)
    {
        for (var i = 0; i < model.Images.Count; i++)
        {
            var image = model.Images[i].Image;
            if (image is not { Length: > 0 }) continue;
            if (!image.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError($"Images[{i}].Image", "Only image files are allowed.");
            }
        }
    }

    private void HydrateImageFilesFromRequest(BannerFormViewModel model)
    {
        if (model.Images.Count == 0 || Request.Form.Files.Count == 0) return;

        for (var i = 0; i < model.Images.Count; i++)
        {
            if (model.Images[i].Image is not null) continue;

            var key = $"Images[{i}].Image";
            var file = Request.Form.Files.GetFile(key);
            if (file is { Length: > 0 }) model.Images[i].Image = file;
        }
    }

    private async Task<List<BannerImage>> BuildImagesAsync(List<BannerImageInput> inputs, CancellationToken ct)
    {
        var rows = new List<BannerImage>();
        foreach (var input in inputs.Where(x => !x.Remove && x.Image is { Length: > 0 }))
        {
            rows.Add(new BannerImage
            {
                ImagePath = await SaveFileAsync(input.Image!, "banners", "banner-image", ct),
                SubDescription = input.SubDescription?.Trim(),
                Description = input.Description?.Trim(),
                Ordering = input.Ordering,
                IsPublished = input.IsPublished
            });
        }

        return rows;
    }

    private async Task<string> SaveFileAsync(IFormFile file, string dir, string prefix, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", dir);
        Directory.CreateDirectory(folder);
        var name = $"{prefix}-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);
        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);
        return Path.Combine("uploads", dir, name).Replace('\\', '/');
    }

    private void DeletePhysicalFile(string? fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;
        var relativePath = fileUrl.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(env.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
    }
}
