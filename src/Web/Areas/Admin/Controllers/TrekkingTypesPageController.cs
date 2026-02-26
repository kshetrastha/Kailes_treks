using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/trekking-types")]
public sealed class TrekkingTypesPageController(ITrekkingTypeService service, ICurrentUser currentUser, IWebHostEnvironment environment) : Controller
{
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, CancellationToken ct)
    {
        var items = await service.ListAsync(true, ct);
        if (!string.IsNullOrWhiteSpace(search)) items = items.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewBag.Search = search;
        return View(items);
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new TrekkingTypeFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        return View("Upsert", new TrekkingTypeFormViewModel
        {
            Id = item.Id,
            Title = item.Title,
            ShortDescription = item.ShortDescription,
            Description = item.Description,
            ExistingImagePath = item.ImagePath,
            Ordering = item.Ordering,
            IsPublished = item.IsPublished,
            Images = item.Images?.Select(x => new TrekkingTypeImageInput { ExistingPath = x.FilePath, AltText = x.AltText, SortOrder = x.SortOrder, IsCover = x.IsCover }).ToList() ?? []
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrekkingTypeFormViewModel m, CancellationToken ct)
    {
        ValidateModel(m);
        if (!ModelState.IsValid) return View("Upsert", m);

        var images = await BuildImagesAsync(m.Images, ct);
        var imagePath = m.Image is { Length: > 0 } ? await UploadImageAsync(m.Image, "trekking-types", ct) : null;

        await service.CreateAsync(new TrekkingTypeUpsertDto(m.Title, m.ShortDescription, m.Description, imagePath, m.Ordering, m.IsPublished, images), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Trekking type created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrekkingTypeFormViewModel m, CancellationToken ct)
    {
        ValidateModel(m);
        if (!ModelState.IsValid) return View("Upsert", m);

        var existing = await service.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var imagePath = existing.ImagePath;
        if (m.Image is { Length: > 0 }) imagePath = await UploadImageAsync(m.Image, "trekking-types", ct);

        var images = await BuildImagesAsync(m.Images, ct);
        var ok = await service.UpdateAsync(id, new TrekkingTypeUpsertDto(m.Title, m.ShortDescription, m.Description, imagePath, m.Ordering, m.IsPublished, images), currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Trekking type updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await service.DeleteAsync(id, ct);
        TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok ? "Trekking type deleted." : "Unable to delete trekking type. It's currently in use.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IReadOnlyCollection<TrekkingTypeImageDto>> BuildImagesAsync(List<TrekkingTypeImageInput> inputs, CancellationToken ct)
    {
        var rows = new List<TrekkingTypeImageDto>();
        var index = 0;
        foreach (var input in inputs.Where(x => !x.Remove))
        {
            var path = input.ExistingPath;
            if (input.File is { Length: > 0 }) path = await UploadImageAsync(input.File, "trekking-types", ct);
            if (!string.IsNullOrWhiteSpace(path)) rows.Add(new TrekkingTypeImageDto(0, path, input.AltText, input.SortOrder == 0 ? index++ : input.SortOrder, input.IsCover));
        }
        return rows;
    }

    private async Task<string> UploadImageAsync(IFormFile image, string folder, CancellationToken ct)
    {
        var extension = Path.GetExtension(image.FileName);
        var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsDirectory);
        var fileName = $"{folder}-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);
        await using var stream = System.IO.File.Create(filePath);
        await image.CopyToAsync(stream, ct);
        return Path.Combine("uploads", folder, fileName).Replace('\\', '/');
    }

    private void ValidateModel(TrekkingTypeFormViewModel model)
    {
        if (model.Image is not null) ValidateImage(model.Image, nameof(model.Image));
        for (var i = 0; i < model.Images.Count; i++) if (model.Images[i].File is not null) ValidateImage(model.Images[i].File!, $"Images[{i}].File");
    }

    private void ValidateImage(IFormFile image, string key)
    {
        if (image.Length > MaxImageSizeInBytes) ModelState.AddModelError(key, "Image size must be 5 MB or less.");
        var extension = Path.GetExtension(image.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension)) ModelState.AddModelError(key, "Image must be jpg, jpeg, png, or webp.");
    }
}
