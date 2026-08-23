using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/privacy-policy")]
public sealed class PrivacyPolicyController(IUnitOfWork uow) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await uow.PrivacyPolicyService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new PrivacyPolicySectionFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var e = await uow.PrivacyPolicyService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        return View("Upsert", new PrivacyPolicySectionFormViewModel
        {
            Id = e.Id,
            Title = e.Title,
            ContentHtml = e.ContentHtml,
            Ordering = e.Ordering,
            IsContactBlock = e.IsContactBlock,
            IsPublished = e.IsPublished
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrivacyPolicySectionFormViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View("Upsert", m);

        var now = DateTime.UtcNow;
        var e = new PrivacyPolicySection
        {
            Title = m.Title.Trim(),
            ContentHtml = m.ContentHtml.Trim(),
            Ordering = m.Ordering,
            IsContactBlock = m.IsContactBlock,
            IsPublished = m.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await uow.PrivacyPolicyService.AddAsync(e, ct);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Privacy policy section created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrivacyPolicySectionFormViewModel m, CancellationToken ct)
    {
        if (id != m.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Upsert", m);

        var e = await uow.PrivacyPolicyService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        e.Title = m.Title.Trim();
        e.ContentHtml = m.ContentHtml.Trim();
        e.Ordering = m.Ordering;
        e.IsContactBlock = m.IsContactBlock;
        e.IsPublished = m.IsPublished;
        e.UpdatedAtUtc = DateTime.UtcNow;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Privacy policy section updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var e = await uow.PrivacyPolicyService.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        uow.PrivacyPolicyService.Remove(e);
        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Privacy policy section deleted.";
        return RedirectToAction(nameof(Index));
    }
}
