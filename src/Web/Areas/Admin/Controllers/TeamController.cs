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
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await uow.TeamMemberService.ListOrderedAsync(false, ct));

    [HttpGet("create")]
    public IActionResult Create() => View("Upsert", new TeamMemberFormViewModel());

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await uow.TeamMemberService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        return View("Upsert", new TeamMemberFormViewModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Role = entity.Role,
            Biography = entity.Biography,
            Email = entity.Email,
            LinkedInUrl = entity.LinkedInUrl,
            ExistingImagePath = entity.ImagePath,
            Ordering = entity.Ordering,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeamMemberFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var now = DateTime.UtcNow;
        var entity = new TeamMember
        {
            FullName = model.FullName.Trim(),
            Role = model.Role.Trim(),
            Biography = model.Biography?.Trim(),
            Email = model.Email?.Trim(),
            LinkedInUrl = model.LinkedInUrl?.Trim(),
            Ordering = model.Ordering,
            IsPublished = model.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        await uow.TeamMemberService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Team member created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TeamMemberFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var entity = await uow.TeamMemberService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        entity.FullName = model.FullName.Trim();
        entity.Role = model.Role.Trim();
        entity.Biography = model.Biography?.Trim();
        entity.Email = model.Email?.Trim();
        entity.LinkedInUrl = model.LinkedInUrl?.Trim();
        entity.Ordering = model.Ordering;
        entity.IsPublished = model.IsPublished;

        if (model.Image is { Length: > 0 })
        {
            entity.ImagePath = await SaveFileAsync(model.Image, ct);
        }

        entity.UpdatedAtUtc = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Team member updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await uow.TeamMemberService.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return NotFound();
        }

        uow.TeamMemberService.Remove(entity);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Team member deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "team");
        Directory.CreateDirectory(folder);
        var name = $"team-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, name);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "team", name).Replace('\\', '/');
    }
}
