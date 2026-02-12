using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;
using TravelCleanArch.Web.Common;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/reviews")]
public sealed class ReviewsController(IUnitOfWork uow, IWebHostEnvironment env) : Controller
{
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct)=>View(await uow.ReviewService.ListOrderedAsync(false,ct));
    [HttpGet("create")] public IActionResult Create()=>View("Upsert",new ReviewFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id,CancellationToken ct){var e=await uow.ReviewService.GetByIdAsync(id,ct); if(e is null) return NotFound(); return View("Upsert",new ReviewFormViewModel{Id=e.Id,ReviewerName=e.ReviewerName,ReviewerRole=e.ReviewerRole,ReviewText=e.ReviewText,Rating=e.Rating,SourceName=e.SourceName,SourceUrl=e.SourceUrl,ReviewedOnUtc=e.ReviewedOnUtc,ExistingImagePath=e.ReviewerImagePath,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(ReviewFormViewModel m,CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var n=DateTime.UtcNow; var e=new Review{ReviewerName=m.ReviewerName.Trim(),ReviewerRole=m.ReviewerRole?.Trim(),ReviewText=m.ReviewText.Trim(),Rating=m.Rating,SourceName=m.SourceName?.Trim(),SourceUrl=m.SourceUrl?.Trim(),ReviewedOnUtc=m.ReviewedOnUtc.AsUtc(),Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=n,UpdatedAtUtc=n}; if(m.Image is {Length:>0}) e.ReviewerImagePath=await SaveFileAsync(m.Image,ct); await uow.ReviewService.AddAsync(e,ct); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,ReviewFormViewModel m,CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.ReviewService.GetByIdAsync(id,ct); if(e is null) return NotFound(); e.ReviewerName=m.ReviewerName.Trim(); e.ReviewerRole=m.ReviewerRole?.Trim(); e.ReviewText=m.ReviewText.Trim(); e.Rating=m.Rating; e.SourceName=m.SourceName?.Trim(); e.SourceUrl=m.SourceUrl?.Trim(); e.ReviewedOnUtc=m.ReviewedOnUtc.AsUtc(); e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; if(m.Image is {Length:>0}) e.ReviewerImagePath=await SaveFileAsync(m.Image,ct); e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id,CancellationToken ct){var e=await uow.ReviewService.GetByIdAsync(id,ct); if(e is null) return NotFound(); uow.ReviewService.Remove(e); await uow.SaveChangesAsync(ct); return RedirectToAction(nameof(Index));}
    private async Task<string> SaveFileAsync(IFormFile file,CancellationToken ct){var ext=Path.GetExtension(file.FileName); var folder=Path.Combine(env.WebRootPath,"uploads","reviews"); Directory.CreateDirectory(folder); var name=$"review-{Guid.NewGuid():N}{ext}"; var path=Path.Combine(folder,name); await using var s=System.IO.File.Create(path); await file.CopyToAsync(s,ct); return Path.Combine("uploads","reviews",name).Replace('\\','/');}
}
