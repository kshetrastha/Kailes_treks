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
[Route("admin/company/terms-and-conditions")]
public sealed partial class TermsAndConditionsController(IUnitOfWork uow) : Controller
{
    [GeneratedRegex("[^a-z0-9]+", RegexOptions.Compiled)]
    private static partial Regex InvalidSlugRegex();

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await uow.TermsAndConditionService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new TermsAndConditionFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var e = await uow.TermsAndConditionService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        return View("Upsert", new TermsAndConditionFormViewModel
        {
            Id = e.Id,
            Country = e.Country,
            Slug = e.Slug,
            Title = e.Title,
            Summary = e.Summary,
            ContentHtml = e.ContentHtml,
            Ordering = e.Ordering,
            IsPublished = e.IsPublished
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TermsAndConditionFormViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View("Upsert", m);

        var now = DateTime.UtcNow;
        var baseSlug = GenerateSlug(m.Slug ?? m.Country);
        var slug = await EnsureUniqueSlugAsync(baseSlug, null, ct);

        var e = new TermsAndCondition
        {
            Country = m.Country.Trim(),
            Slug = slug,
            Title = m.Title.Trim(),
            Summary = m.Summary?.Trim(),
            ContentHtml = m.ContentHtml.Trim(),
            Ordering = m.Ordering,
            IsPublished = m.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await uow.TermsAndConditionService.AddAsync(e, ct);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Terms and condition created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TermsAndConditionFormViewModel m, CancellationToken ct)
    {
        if (id != m.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Upsert", m);

        var e = await uow.TermsAndConditionService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        e.Country = m.Country.Trim();
        e.Title = m.Title.Trim();
        e.Summary = m.Summary?.Trim();
        e.ContentHtml = m.ContentHtml.Trim();
        e.Ordering = m.Ordering;
        e.IsPublished = m.IsPublished;
        e.Slug = await EnsureUniqueSlugAsync(GenerateSlug(m.Slug ?? m.Country), e.Id, ct);
        e.UpdatedAtUtc = DateTime.UtcNow;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Terms and condition updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var e = await uow.TermsAndConditionService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        uow.TermsAndConditionService.Remove(e);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Terms and condition deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> EnsureUniqueSlugAsync(string slug, int? currentId, CancellationToken ct)
    {
        slug = string.IsNullOrWhiteSpace(slug) ? Guid.NewGuid().ToString("N") : slug;

        var candidate = slug;
        var i = 2;

        while (true)
        {
            var existing = await uow.TermsAndConditionService.GetBySlugAsync(candidate, false, ct);
            if (existing is null || existing.Id == currentId)
            {
                return candidate;
            }

            candidate = $"{slug}-{i++}";
        }
    }

    private static string GenerateSlug(string? input)
    {
        var raw = (input ?? string.Empty).Trim().ToLowerInvariant();
        raw = InvalidSlugRegex().Replace(raw, "-").Trim('-');
        return string.IsNullOrWhiteSpace(raw) ? "item" : raw;
    }
}
