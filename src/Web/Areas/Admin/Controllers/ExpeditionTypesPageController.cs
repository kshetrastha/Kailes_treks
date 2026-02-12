using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/expedition-types")]
public sealed class ExpeditionTypesPageController(IExpeditionTypeService service, ICurrentUser currentUser, IWebHostEnvironment environment) : Controller
{

    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await service.ListAsync(includeUnpublished: true, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new ExpeditionTypeFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new ExpeditionTypeFormViewModel
        {
            Id = item.Id,
            Title = item.Title,
            ShortDescription = item.ShortDescription,
            Description = item.Description,
            ExistingImagePath = item.ImagePath,
            Ordering = item.Ordering,
            IsPublished = item.IsPublished
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionTypeFormViewModel m, CancellationToken ct)
    {
        ValidateModel(m);

        if (!ModelState.IsValid) return View("Upsert", m);

        string? imagePath = null;
        if (m.Image is { Length: > 0 })
        {
            imagePath = await UploadImageAsync(m.Image, ct);
        }

        await service.CreateAsync(new ExpeditionTypeUpsertDto(m.Title, m.ShortDescription, m.Description, imagePath, m.Ordering, m.IsPublished), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition type created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionTypeFormViewModel m, CancellationToken ct)
    {
        ValidateModel(m);

        if (!ModelState.IsValid) return View("Upsert", m);

        var existing = await service.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var imagePath = existing.ImagePath;
        if (m.Image is { Length: > 0 })
        {
            imagePath = await UploadImageAsync(m.Image, ct);
        }

        var ok = await service.UpdateAsync(id, new ExpeditionTypeUpsertDto(m.Title, m.ShortDescription, m.Description, imagePath, m.Ordering, m.IsPublished), currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Expedition type updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await service.DeleteAsync(id, ct);
        if (!ok) TempData["ErrorMessage"] = "Unable to delete expedition type. It's currently in use.";
        else TempData["SuccessMessage"] = "Expedition type deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> UploadImageAsync(IFormFile image, CancellationToken ct)
    {
        var fileExtension = Path.GetExtension(image.FileName);
        var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "expedition-types");

        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"expedition-type-{Guid.NewGuid():N}{fileExtension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await image.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "expedition-types", fileName).Replace('\\', '/');
    }

    private void ValidateModel(ExpeditionTypeFormViewModel model)
    {
        if (model.Image is null)
        {
            return;
        }

        if (model.Image.Length > MaxImageSizeInBytes)
        {
            ModelState.AddModelError(nameof(model.Image), "Image size must be 5 MB or less.");
        }

        var extension = Path.GetExtension(model.Image.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension))
        {
            ModelState.AddModelError(nameof(model.Image), "Image must be a valid image file (jpg, jpeg, png, webp, gif).");
        }
    }

}
