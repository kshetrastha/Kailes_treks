using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/certificates-documents")]
public sealed class CertificatesDocumentsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await uow.CertificateDocumentService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new CertificateDocumentFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.CertificateDocumentService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new CertificateDocumentFormViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Category = entity.Category,
            Description = entity.Description,
            IssuedOnUtc = entity.IssuedOnUtc?.Date,
            ExistingFilePath = entity.FilePath,
            ExistingThumbnailPath = entity.ThumbnailImagePath,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CertificateDocumentFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new CertificateDocument
        {
            Title = model.Title.Trim(),
            Category = model.Category?.Trim(),
            Description = model.Description?.Trim(),
            IssuedOnUtc = NormalizeDateToUtc(model.IssuedOnUtc),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.DocumentFile is { Length: > 0 })
        {
            entity.FilePath = await SaveFileAsync(model.DocumentFile, "documents", "document", ct);
        }

        if (model.ThumbnailImage is { Length: > 0 })
        {
            entity.ThumbnailImagePath = await SaveFileAsync(model.ThumbnailImage, "documents", "thumb", ct);
        }

        await uow.CertificateDocumentService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Certificate/Document created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CertificateDocumentFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await uow.CertificateDocumentService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Title = model.Title.Trim();
        entity.Category = model.Category?.Trim();
        entity.Description = model.Description?.Trim();
        entity.IssuedOnUtc = NormalizeDateToUtc(model.IssuedOnUtc);
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.DocumentFile is { Length: > 0 })
        {
            entity.FilePath = await SaveFileAsync(model.DocumentFile, "documents", "document", ct);
        }

        if (model.ThumbnailImage is { Length: > 0 })
        {
            entity.ThumbnailImagePath = await SaveFileAsync(model.ThumbnailImage, "documents", "thumb", ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Certificate/Document updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.CertificateDocumentService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        uow.CertificateDocumentService.Remove(entity);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Certificate/Document deleted.";
        return RedirectToAction(nameof(Index));
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

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", dir, name).Replace('\\', '/');
    }
}
