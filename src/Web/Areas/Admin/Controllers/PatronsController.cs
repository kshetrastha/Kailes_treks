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
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct) => View(await uow.PatronService.ListOrderedAsync(false, ct));
    [HttpGet("create")] public IActionResult Create() => View("Upsert", new PatronFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id, CancellationToken ct){var e=await uow.PatronService.GetByIdAsync(id, ct); if(e is null) return NotFound(); return View("Upsert", new PatronFormViewModel{Id=e.Id,Name=e.Name,Role=e.Role,Biography=e.Biography,ExistingImagePath=e.ImagePath,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(PatronFormViewModel m, CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new Patron{Name=m.Name.Trim(),Role=m.Role.Trim(),Biography=m.Biography?.Trim(),Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); await uow.PatronService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, PatronFormViewModel m, CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.PatronService.GetByIdAsync(id, ct); if(e is null) return NotFound(); e.Name=m.Name.Trim(); e.Role=m.Role.Trim(); e.Biography=m.Biography?.Trim(); e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id, CancellationToken ct){var e=await uow.PatronService.GetByIdAsync(id, ct); if(e is null) return NotFound(); uow.PatronService.Remove(e); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads","patrons"); Directory.CreateDirectory(folder); var name=$"patron-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads","patrons",name).Replace('\\','/');}
}
