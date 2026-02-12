using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/blogs")]
public sealed partial class BlogsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [GeneratedRegex("[^a-z0-9]+", RegexOptions.Compiled)]
    private static partial Regex InvalidSlugRegex();

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await uow.BlogPostService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new BlogPostFormViewModel { PublishedOnUtc = DateTime.UtcNow.Date });

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var e = await uow.BlogPostService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        return View("Upsert", new BlogPostFormViewModel
        {
            Id = e.Id,
            Title = e.Title,
            Slug = e.Slug,
            Summary = e.Summary,
            ContentHtml = e.ContentHtml,
            PublishedOnUtc = e.PublishedOnUtc?.Date,
            ExistingHeroImagePath = e.HeroImagePath,
            ExistingThumbnailImagePath = e.ThumbnailImagePath,
            Ordering = e.Ordering,
            IsFeatured = e.IsFeatured,
            IsPublished = e.IsPublished
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostFormViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View("Upsert", m);

        var now = DateTime.UtcNow;
        var baseSlug = GenerateSlug(m.Slug ?? m.Title);
        var slug = await EnsureUniqueSlugAsync(baseSlug, null, ct);

        var e = new BlogPost
        {
            Title = m.Title.Trim(),
            Slug = slug,
            Summary = m.Summary?.Trim(),
            ContentHtml = m.ContentHtml.Trim(),
            PublishedOnUtc = NormalizeDateToUtc(m.PublishedOnUtc),
            Ordering = m.Ordering,
            IsFeatured = m.IsFeatured,
            IsPublished = m.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (m.HeroImage is { Length: > 0 }) e.HeroImagePath = await SaveFileAsync(m.HeroImage, "blogs", "blog-hero", ct);
        if (m.ThumbnailImage is { Length: > 0 }) e.ThumbnailImagePath = await SaveFileAsync(m.ThumbnailImage, "blogs", "blog-thumb", ct);

        await uow.BlogPostService.AddAsync(e, ct);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Blog created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BlogPostFormViewModel m, CancellationToken ct)
    {
        if (id != m.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Upsert", m);

        var e = await uow.BlogPostService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        e.Title = m.Title.Trim();
        e.Summary = m.Summary?.Trim();
        e.ContentHtml = m.ContentHtml.Trim();
        e.PublishedOnUtc = NormalizeDateToUtc(m.PublishedOnUtc);
        e.Ordering = m.Ordering;
        e.IsFeatured = m.IsFeatured;
        e.IsPublished = m.IsPublished;
        e.Slug = await EnsureUniqueSlugAsync(GenerateSlug(m.Slug ?? m.Title), e.Id, ct);

        if (m.HeroImage is { Length: > 0 }) e.HeroImagePath = await SaveFileAsync(m.HeroImage, "blogs", "blog-hero", ct);
        if (m.ThumbnailImage is { Length: > 0 }) e.ThumbnailImagePath = await SaveFileAsync(m.ThumbnailImage, "blogs", "blog-thumb", ct);

        e.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Blog updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var e = await uow.BlogPostService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();
        uow.BlogPostService.Remove(e);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Blog deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> EnsureUniqueSlugAsync(string baseSlug, int? currentId, CancellationToken ct)
    {
        var slug = baseSlug;
        var index = 2;
        while (true)
        {
            var found = await uow.BlogPostService.GetBySlugAsync(slug, false, ct);
            if (found is null || found.Id == currentId) return slug;
            slug = $"{baseSlug}-{index++}";
        }
    }

    private static string GenerateSlug(string value)
    {
        var slug = InvalidSlugRegex().Replace(value.ToLowerInvariant().Trim(), "-").Trim('-');
        return string.IsNullOrWhiteSpace(slug) ? "blog" : slug;
    }

    private static DateTime? NormalizeDateToUtc(DateTime? value) =>
        value.HasValue ? DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc) : null;

    private async Task<string> SaveFileAsync(IFormFile file, string dir, string prefix, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", dir);
        Directory.CreateDirectory(folder);
        var name = $"{prefix}-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);
        await using var s = System.IO.File.Create(path);
        await file.CopyToAsync(s, ct);
        return Path.Combine("uploads", dir, name).Replace('\\', '/');
    }
}
