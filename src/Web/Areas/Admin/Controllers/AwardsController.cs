using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;
using TravelCleanArch.Web.Common;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/awards")]
public sealed class AwardsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct) => View(await uow.AwardService.ListOrderedAsync(false, ct));
    [HttpGet("create")] public IActionResult Create() => View("Upsert", new AwardFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id, CancellationToken ct){var e=await uow.AwardService.GetByIdAsync(id, ct); if(e is null) return NotFound(); return View("Upsert", new AwardFormViewModel{Id=e.Id,Title=e.Title,Issuer=e.Issuer,AwardedOnUtc=e.AwardedOnUtc?.Date,Description=e.Description,ReferenceUrl=e.ReferenceUrl,ExistingImagePath=e.ImagePath,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(AwardFormViewModel m, CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new Award{Title=m.Title.Trim(),Issuer=m.Issuer?.Trim(),AwardedOnUtc=m.AwardedOnUtc.AsUtc(),Description=m.Description?.Trim(),ReferenceUrl=m.ReferenceUrl?.Trim(),Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,"awards","award",ct); await uow.AwardService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="Award created."; return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, AwardFormViewModel m, CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.AwardService.GetByIdAsync(id, ct); if(e is null) return NotFound(); e.Title=m.Title.Trim(); e.Issuer=m.Issuer?.Trim(); e.AwardedOnUtc=m.AwardedOnUtc.AsUtc(); e.Description=m.Description?.Trim(); e.ReferenceUrl=m.ReferenceUrl?.Trim(); e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,"awards","award",ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="Award updated."; return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id, CancellationToken ct){var e=await uow.AwardService.GetByIdAsync(id, ct); if(e is null) return NotFound(); uow.AwardService.Remove(e); await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="Award deleted."; return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file, string dir, string prefix, CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads",dir); Directory.CreateDirectory(folder); var name=$"{prefix}-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads",dir,name).Replace('\\','/');}
}
