using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/about-us")]
public sealed class AboutUsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var page = await uow.AboutUsService.GetPageAsync(asNoTracking: true, publishedOnly: false, ct);

        if (page is null)
        {
            return View(new AboutUsFormViewModel());
        }

        return View(new AboutUsFormViewModel
        {
            Id = page.Id,
            Subtitle = page.Subtitle,
            Title = page.Title,
            Description = page.Description,
            ContentHtml = page.ContentHtml,
            BadgeText = page.BadgeText,
            ContactPhone = page.ContactPhone,
            ButtonText = page.ButtonText,
            ButtonUrl = page.ButtonUrl,
            IsPublished = page.IsPublished,
            ExistingPrimaryImagePath = page.PrimaryImagePath,
            ExistingSecondaryImagePath = page.SecondaryImagePath,
            Highlights = page.Highlights
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new AboutUsHighlightInput
                {
                    Id = x.Id,
                    Text = x.Text,
                    Ordering = x.Ordering,
                    IsPublished = x.IsPublished
                })
                .ToList()
        });
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AboutUsFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        var now = DateTime.UtcNow;
        var page = await uow.AboutUsService.GetPageAsync(asNoTracking: false, publishedOnly: false, ct);

        if (page is null)
        {
            page = new AboutUsPage { CreatedAtUtc = now };
            await uow.AboutUsService.AddAsync(page, ct);
        }

        page.Subtitle = model.Subtitle.Trim();
        page.Title = model.Title.Trim();
        page.Description = model.Description?.Trim();
        page.ContentHtml = model.ContentHtml?.Trim();
        page.BadgeText = model.BadgeText?.Trim();
        page.ContactPhone = model.ContactPhone?.Trim();
        page.ButtonText = model.ButtonText?.Trim();
        page.ButtonUrl = model.ButtonUrl?.Trim();
        page.IsPublished = model.IsPublished;
        page.UpdatedAtUtc = now;

        if (model.RemovePrimaryImage && model.PrimaryImage is null)
        {
            DeletePhysicalFile(page.PrimaryImagePath);
            page.PrimaryImagePath = null;
        }
        else if (model.PrimaryImage is { Length: > 0 })
        {
            DeletePhysicalFile(page.PrimaryImagePath);
            page.PrimaryImagePath = await SaveFileAsync(model.PrimaryImage, "about-us", "about-primary", ct);
        }

        if (model.RemoveSecondaryImage && model.SecondaryImage is null)
        {
            DeletePhysicalFile(page.SecondaryImagePath);
            page.SecondaryImagePath = null;
        }
        else if (model.SecondaryImage is { Length: > 0 })
        {
            DeletePhysicalFile(page.SecondaryImagePath);
            page.SecondaryImagePath = await SaveFileAsync(model.SecondaryImage, "about-us", "about-secondary", ct);
        }

        SyncHighlights(page, model.Highlights, now);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "About Us page saved.";
        return RedirectToAction(nameof(Index));
    }

    private static void SyncHighlights(AboutUsPage page, List<AboutUsHighlightInput> inputs, DateTime now)
    {
        foreach (var input in inputs)
        {
            var text = input.Text?.Trim();

            if (input.Id is { } id && id > 0)
            {
                var current = page.Highlights.FirstOrDefault(x => x.Id == id);
                if (current is null) continue;

                if (input.Remove || string.IsNullOrWhiteSpace(text))
                {
                    page.Highlights.Remove(current);
                    continue;
                }

                current.Text = text;
                current.Ordering = input.Ordering;
                current.IsPublished = input.IsPublished;
                current.UpdatedAtUtc = now;
                continue;
            }

            if (input.Remove || string.IsNullOrWhiteSpace(text)) continue;

            page.Highlights.Add(new AboutUsHighlight
            {
                Text = text,
                Ordering = input.Ordering,
                IsPublished = input.IsPublished,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            });
        }
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

    private void DeletePhysicalFile(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        var fullPath = Path.Combine(env.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
    }
}
