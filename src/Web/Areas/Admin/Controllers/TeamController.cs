using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/team")]
public sealed class TeamController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct) => View(await uow.TeamMemberService.ListOrderedAsync(false, ct));
    [HttpGet("create")] public IActionResult Create()=>View("Upsert",new TeamMemberFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id,CancellationToken ct){var e=await uow.TeamMemberService.GetByIdAsync(id,ct); if(e is null) return NotFound(); return View("Upsert",new TeamMemberFormViewModel{Id=e.Id,FullName=e.FullName,Role=e.Role,Biography=e.Biography,Email=e.Email,LinkedInUrl=e.LinkedInUrl,ExistingImagePath=e.ImagePath,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(TeamMemberFormViewModel m,CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new TeamMember{FullName=m.FullName.Trim(),Role=m.Role.Trim(),Biography=m.Biography?.Trim(),Email=m.Email?.Trim(),LinkedInUrl=m.LinkedInUrl?.Trim(),Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); await uow.TeamMemberService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,TeamMemberFormViewModel m,CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.TeamMemberService.GetByIdAsync(id,ct); if(e is null) return NotFound(); e.FullName=m.FullName.Trim(); e.Role=m.Role.Trim(); e.Biography=m.Biography?.Trim(); e.Email=m.Email?.Trim(); e.LinkedInUrl=m.LinkedInUrl?.Trim(); e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; if(m.Image is {Length:>0}) e.ImagePath=await SaveFileAsync(m.Image,ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id,CancellationToken ct){var e=await uow.TeamMemberService.GetByIdAsync(id,ct); if(e is null) return NotFound(); uow.TeamMemberService.Remove(e); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file,CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads","team"); Directory.CreateDirectory(folder); var name=$"team-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads","team",name).Replace('\\','/');}
}
