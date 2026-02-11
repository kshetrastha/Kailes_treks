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
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct)=>View(await uow.CertificateDocumentService.ListOrderedAsync(false,ct));
    [HttpGet("create")] public IActionResult Create()=>View("Upsert",new CertificateDocumentFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id,CancellationToken ct){var e=await uow.CertificateDocumentService.GetByIdAsync(id,ct); if(e is null) return NotFound(); return View("Upsert",new CertificateDocumentFormViewModel{Id=e.Id,Title=e.Title,Category=e.Category,Description=e.Description,IssuedOnUtc=e.IssuedOnUtc,ExistingFilePath=e.FilePath,ExistingThumbnailPath=e.ThumbnailImagePath,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(CertificateDocumentFormViewModel m,CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new CertificateDocument{Title=m.Title.Trim(),Category=m.Category?.Trim(),Description=m.Description?.Trim(),IssuedOnUtc=m.IssuedOnUtc,Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.DocumentFile is {Length:>0}) e.FilePath=await SaveFileAsync(m.DocumentFile,"documents","document",ct); if(m.ThumbnailImage is {Length:>0}) e.ThumbnailImagePath=await SaveFileAsync(m.ThumbnailImage,"documents","thumb",ct); await uow.CertificateDocumentService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,CertificateDocumentFormViewModel m,CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.CertificateDocumentService.GetByIdAsync(id,ct); if(e is null) return NotFound(); e.Title=m.Title.Trim(); e.Category=m.Category?.Trim(); e.Description=m.Description?.Trim(); e.IssuedOnUtc=m.IssuedOnUtc; e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; if(m.DocumentFile is {Length:>0}) e.FilePath=await SaveFileAsync(m.DocumentFile,"documents","document",ct); if(m.ThumbnailImage is {Length:>0}) e.ThumbnailImagePath=await SaveFileAsync(m.ThumbnailImage,"documents","thumb",ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id,CancellationToken ct){var e=await uow.CertificateDocumentService.GetByIdAsync(id,ct); if(e is null) return NotFound(); uow.CertificateDocumentService.Remove(e); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file,string dir,string prefix,CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads",dir); Directory.CreateDirectory(folder); var name=$"{prefix}-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads",dir,name).Replace('\\','/');}
}
