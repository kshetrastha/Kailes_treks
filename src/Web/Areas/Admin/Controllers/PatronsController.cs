using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/patrons")]
public sealed class PatronsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await uow.PatronService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new PatronFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.PatronService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new PatronFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Role = entity.Role,
            Biography = entity.Biography,
            ExistingImagePath = entity.ImagePath,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatronFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new Patron
        {
            Name = model.Name.Trim(),
            Role = model.Role.Trim(),
            Biography = model.Biography?.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        await uow.PatronService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Patron created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PatronFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await uow.PatronService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Name = model.Name.Trim();
        entity.Role = model.Role.Trim();
        entity.Biography = model.Biography?.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Patron updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.PatronService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        uow.PatronService.Remove(entity);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Patron deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "patrons");
        Directory.CreateDirectory(folder);
        var name = $"patron-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "patrons", name).Replace('\\', '/');
    }
}
