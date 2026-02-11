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
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct)=>View(await uow.ChairmanMessageService.ListOrderedAsync(false,ct));
    [HttpGet("create")] public IActionResult Create()=>View("Upsert",new ChairmanMessageFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id,CancellationToken ct){var e=await uow.ChairmanMessageService.GetByIdAsync(id,ct); if(e is null) return NotFound(); return View("Upsert",new ChairmanMessageFormViewModel{Id=e.Id,Heading=e.Heading,ChairmanName=e.ChairmanName,Designation=e.Designation,MessageHtml=e.MessageHtml,VideoUrl=e.VideoUrl,ExistingImagePath=e.ImagePath,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(ChairmanMessageFormViewModel m,CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new ChairmanMessage{Heading=m.Heading.Trim(),ChairmanName=m.ChairmanName.Trim(),Designation=m.Designation?.Trim(),MessageHtml=m.MessageHtml.Trim(),VideoUrl=m.VideoUrl?.Trim(),IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); await uow.ChairmanMessageService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,ChairmanMessageFormViewModel m,CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.ChairmanMessageService.GetByIdAsync(id,ct); if(e is null) return NotFound(); e.Heading=m.Heading.Trim(); e.ChairmanName=m.ChairmanName.Trim(); e.Designation=m.Designation?.Trim(); e.MessageHtml=m.MessageHtml.Trim(); e.VideoUrl=m.VideoUrl?.Trim(); e.IsPublished=m.IsPublished; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id,CancellationToken ct){var e=await uow.ChairmanMessageService.GetByIdAsync(id,ct); if(e is null) return NotFound(); uow.ChairmanMessageService.Remove(e); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file,CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads","chairman"); Directory.CreateDirectory(folder); var name=$"chairman-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads","chairman",name).Replace('\\','/');}
}
