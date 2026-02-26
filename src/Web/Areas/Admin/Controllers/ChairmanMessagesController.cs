using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/chairman-message")]
public sealed class ChairmanMessagesController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await uow.ChairmanMessageService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new ChairmanMessageFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.ChairmanMessageService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new ChairmanMessageFormViewModel
        {
            Id = entity.Id,
            Heading = entity.Heading,
            ChairmanName = entity.ChairmanName,
            Designation = entity.Designation,
            MessageHtml = entity.MessageHtml,
            VideoUrl = entity.VideoUrl,
            ExistingImagePath = entity.ImagePath,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ChairmanMessageFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new ChairmanMessage
        {
            Heading = model.Heading.Trim(),
            ChairmanName = model.ChairmanName.Trim(),
            Designation = model.Designation?.Trim(),
            MessageHtml = model.MessageHtml.Trim(),
            VideoUrl = model.VideoUrl?.Trim(),
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        await uow.ChairmanMessageService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Chairman message created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ChairmanMessageFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await uow.ChairmanMessageService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Heading = model.Heading.Trim();
        entity.ChairmanName = model.ChairmanName.Trim();
        entity.Designation = model.Designation?.Trim();
        entity.MessageHtml = model.MessageHtml.Trim();
        entity.VideoUrl = model.VideoUrl?.Trim();
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Chairman message updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.ChairmanMessageService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        uow.ChairmanMessageService.Remove(entity);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Chairman message deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "chairman");
        Directory.CreateDirectory(folder);
        var name = $"chairman-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "chairman", name).Replace('\\', '/');
    }
}
