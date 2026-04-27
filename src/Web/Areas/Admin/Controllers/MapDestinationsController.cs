using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Web.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/map-destinations")]
public sealed class MapDestinationsController(AppDbContext db, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await db.MapDestinations
            .AsNoTracking()
            .Include(x => x.Images)
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        return View(items);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var model = new MapDestinationFormViewModel();
        PopulateDestinationOptions(model);
        return View("Upsert", model);
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await db.MapDestinations
            .Include(x => x.Images.OrderBy(i => i.SortOrder))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is null) return NotFound();

        var model = new MapDestinationFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            ShortDescription = item.ShortDescription,
            Description = item.Description,
            ExistingHeroImagePath = item.HeroImagePath,
            Ordering = item.Ordering,
            IsPublished = item.IsPublished,
            Images = item.Images
                .OrderBy(x => x.SortOrder)
                .Select(x => new MapDestinationImageInput
                {
                    Id = x.Id,
                    ExistingPath = x.ImagePath,
                    Caption = x.Caption,
                    SortOrder = x.SortOrder
                })
                .ToList()
        };

        PopulateDestinationOptions(model);
        return View("Upsert", model);
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MapDestinationFormViewModel model, CancellationToken ct)
    {
        ValidateDestination(model);
        if (!ModelState.IsValid)
        {
            PopulateDestinationOptions(model);
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new MapDestination
        {
            Name = model.Name.Trim(),
            ShortDescription = model.ShortDescription.Trim(),
            Description = model.Description.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.HeroImage is { Length: > 0 })
        {
            entity.HeroImagePath = await SaveImageAsync(model.HeroImage, "hero", ct);
        }

        var rows = model.Images
            .Where(x => x.File is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.Caption))
            .ToList();

        foreach (var row in rows)
        {
            var path = row.ExistingPath;
            if (row.File is { Length: > 0 }) path = await SaveImageAsync(row.File, "gallery", ct);
            if (string.IsNullOrWhiteSpace(path)) continue;

            entity.Images.Add(new MapDestinationImage
            {
                ImagePath = path,
                Caption = row.Caption?.Trim(),
                SortOrder = row.SortOrder,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            });
        }

        await db.MapDestinations.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Map destination created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MapDestinationFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        ValidateDestination(model);
        if (!ModelState.IsValid)
        {
            PopulateDestinationOptions(model);
            return View("Upsert", model);
        }

        var item = await db.MapDestinations
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.ShortDescription = model.ShortDescription.Trim();
        item.Description = model.Description.Trim();
        item.Ordering = model.Ordering;
        item.IsPublished = model.IsPublished;
        item.UpdatedAtUtc = DateTime.UtcNow;

        if (model.HeroImage is { Length: > 0 })
        {
            item.HeroImagePath = await SaveImageAsync(model.HeroImage, "hero", ct);
        }

        var rows = model.Images
            .Where(x => x.File is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.Caption))
            .ToList();

        var existingById = item.Images.ToDictionary(x => x.Id);
        var rowIds = rows.Where(x => x.Id > 0).Select(x => x.Id).ToHashSet();
        foreach (var old in item.Images.Where(x => !rowIds.Contains(x.Id)).ToList())
        {
            db.MapDestinationImages.Remove(old);
        }

        foreach (var row in rows)
        {
            var path = row.ExistingPath;
            if (row.File is { Length: > 0 }) path = await SaveImageAsync(row.File, "gallery", ct);
            if (string.IsNullOrWhiteSpace(path)) continue;

            if (row.Id > 0 && existingById.TryGetValue(row.Id, out var existing))
            {
                existing.ImagePath = path;
                existing.Caption = row.Caption?.Trim();
                existing.SortOrder = row.SortOrder;
                existing.UpdatedAtUtc = DateTime.UtcNow;
            }
            else
            {
                item.Images.Add(new MapDestinationImage
                {
                    ImagePath = path,
                    Caption = row.Caption?.Trim(),
                    SortOrder = row.SortOrder,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                });
            }
        }

        await db.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Map destination updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await db.MapDestinations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) return NotFound();

        db.MapDestinations.Remove(item);
        await db.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Map destination deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImageAsync(IFormFile file, string kind, CancellationToken ct)
    {
        var extension = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "map-destinations", kind);
        Directory.CreateDirectory(folder);

        var fileName = $"map-{kind}-{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(folder, fileName);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "map-destinations", kind, fileName).Replace('\\', '/');
    }

    private static void PopulateDestinationOptions(MapDestinationFormViewModel model)
    {
        model.DestinationOptions = Enum.GetValues<PilgrimageDestination>()
            .Select(x => new SelectListItem(GetDisplayName(x), x.ToString(), string.Equals(model.Name, x.ToString(), StringComparison.Ordinal)))
            .ToList();
    }

    private void ValidateDestination(MapDestinationFormViewModel model)
    {
        if (Enum.TryParse<PilgrimageDestination>(model.Name, true, out _)) return;
        ModelState.AddModelError(nameof(MapDestinationFormViewModel.Name), "Please select a valid destination.");
    }

    private static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
        var display = field?.GetCustomAttribute<DisplayAttribute>();
        return string.IsNullOrWhiteSpace(display?.Name) ? value.ToString() : display.Name!;
    }
}
