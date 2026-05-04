using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/company/master-faqs")]
public sealed class MasterFaqsController(IUnitOfWork uow) : Controller
{
    [HttpGet("")] public async Task<IActionResult> Index(CancellationToken ct) => View(await uow.MasterFaqService.ListOrderedAsync(false, ct));
    [HttpGet("create")] public IActionResult Create() => View("Upsert", new MasterFaqFormViewModel());
    [HttpGet("{id:int}/edit")] public async Task<IActionResult> Edit(int id, CancellationToken ct){var e=await uow.MasterFaqService.GetByIdAsync(id, ct); if(e is null) return NotFound(); return View("Upsert", new MasterFaqFormViewModel{Id=e.Id,Question=e.Question,Answer=e.Answer,Ordering=e.Ordering,IsPublished=e.IsPublished});}
    [HttpPost("create"),ValidateAntiForgeryToken] public async Task<IActionResult> Create(MasterFaqFormViewModel m, CancellationToken ct){if(!ModelState.IsValid) return View("Upsert",m); var now=DateTime.UtcNow; await uow.MasterFaqService.AddAsync(new MasterFaq{Question=m.Question.Trim(),Answer=m.Answer.Trim(),Ordering=m.Ordering,IsPublished=m.IsPublished,CreatedAtUtc=now,UpdatedAtUtc=now},ct); await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="FAQ created."; return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/edit"),ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, MasterFaqFormViewModel m, CancellationToken ct){if(id!=m.Id) return BadRequest(); if(!ModelState.IsValid) return View("Upsert",m); var e=await uow.MasterFaqService.GetByIdAsync(id, ct); if(e is null) return NotFound(); e.Question=m.Question.Trim(); e.Answer=m.Answer.Trim(); e.Ordering=m.Ordering; e.IsPublished=m.IsPublished; e.UpdatedAtUtc=DateTime.UtcNow; await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="FAQ updated."; return RedirectToAction(nameof(Index));}
    [HttpPost("{id:int}/delete"),ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id, CancellationToken ct){var e=await uow.MasterFaqService.GetByIdAsync(id, ct); if(e is null) return NotFound(); uow.MasterFaqService.Remove(e); await uow.SaveChangesAsync(ct); TempData["SuccessMessage"]="FAQ deleted."; return RedirectToAction(nameof(Index));}
}
