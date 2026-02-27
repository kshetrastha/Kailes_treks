using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/service-region/service-faqs")]
public sealed class ServiceRegionFaqsController(IUnitOfWork uow, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? serviceRegionId = null, string? question = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, serviceRegionId, question, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(int page = 1, int pageSize = 10, int? serviceRegionId = null, string? question = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, serviceRegionId, question, ct);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(int? serviceRegionId, CancellationToken ct)
    {
        await LoadServiceRegionOptionsAsync(serviceRegionId, ct);
        return View("Upsert", new ServiceRegionFaqFormViewModel
        {
            ServiceRegionId = serviceRegionId.GetValueOrDefault()
        });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.ServiceRegionFaqService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        await LoadServiceRegionOptionsAsync(item.ServiceRegionId, ct);
        return View("Upsert", new ServiceRegionFaqFormViewModel
        {
            Id = item.Id,
            ServiceRegionId = item.ServiceRegionId,
            Question = item.Question,
            Answer = item.Answer,
            DisplayOrder = item.DisplayOrder
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRegionFaqFormViewModel model, CancellationToken ct)
    {
        await LoadServiceRegionOptionsAsync(model.ServiceRegionId, ct);
        if (!await ValidateModelAsync(model, ct)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        await uow.ServiceRegionFaqService.AddAsync(new ServiceRegionFaq
        {
            ServiceRegionId = model.ServiceRegionId,
            Question = model.Question.Trim(),
            Answer = model.Answer.Trim(),
            DisplayOrder = model.DisplayOrder,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        }, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service region FAQ created.";
        return RedirectToAction(nameof(Index), new { serviceRegionId = model.ServiceRegionId });
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceRegionFaqFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();

        await LoadServiceRegionOptionsAsync(model.ServiceRegionId, ct);
        if (!await ValidateModelAsync(model, ct)) return View("Upsert", model);

        var item = await uow.ServiceRegionFaqService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.ServiceRegionId = model.ServiceRegionId;
        item.Question = model.Question.Trim();
        item.Answer = model.Answer.Trim();
        item.DisplayOrder = model.DisplayOrder;
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service region FAQ updated.";
        return RedirectToAction(nameof(Index), new { serviceRegionId = model.ServiceRegionId });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int? serviceRegionId, CancellationToken ct)
    {
        var item = await uow.ServiceRegionFaqService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        uow.ServiceRegionFaqService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Service region FAQ deleted.";
        return RedirectToAction(nameof(Index), new { serviceRegionId = serviceRegionId ?? item.ServiceRegionId });
    }

    private async Task<bool> ValidateModelAsync(ServiceRegionFaqFormViewModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Question))
        {
            ModelState.AddModelError(nameof(model.Question), "Question is required.");
        }

        if (string.IsNullOrWhiteSpace(model.Answer))
        {
            ModelState.AddModelError(nameof(model.Answer), "Answer is required.");
        }

        var serviceRegionExists = model.ServiceRegionId > 0 &&
                                  await uow.ServiceRegionService.AnyAsync(x => x.Id == model.ServiceRegionId, ct);
        if (!serviceRegionExists)
        {
            ModelState.AddModelError(nameof(model.ServiceRegionId), "Select a valid service region.");
        }

        return ModelState.IsValid;
    }

    private async Task<IActionResult> BuildIndexResult(int page, int pageSize, int? serviceRegionId, string? question, CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentServiceRegionId = serviceRegionId is > 0 ? serviceRegionId : null;
        var currentQuestion = string.IsNullOrWhiteSpace(question) ? null : question.Trim();

        ViewBag.PageSize = currentPageSize;
        ViewBag.ServiceRegionId = currentServiceRegionId;
        ViewBag.Question = currentQuestion;
        ViewBag.ServiceRegionOptions = await GetServiceRegionFilterOptionsAsync(currentServiceRegionId, ct);

        var model = await uow.ServiceRegionFaqService.ListOrderedAsync(currentPage, currentPageSize, currentServiceRegionId, currentQuestion, ct);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_ServiceRegionFaqsTable", model);
        }

        return View(nameof(Index), model);
    }

    private async Task LoadServiceRegionOptionsAsync(int? selectedId, CancellationToken ct)
    {
        var items = await uow.ServiceRegionService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), selectedId == x.Id))
            .ToListAsync(ct);

        items.Insert(0, new SelectListItem("-- Select Service Region --", string.Empty, !selectedId.HasValue || selectedId.Value <= 0));
        ViewBag.ServiceRegionOptions = items;
    }

    private async Task<List<SelectListItem>> GetServiceRegionFilterOptionsAsync(int? selectedId, CancellationToken ct)
    {
        var items = await uow.ServiceRegionService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), selectedId == x.Id))
            .ToListAsync(ct);

        items.Insert(0, new SelectListItem("All", string.Empty, !selectedId.HasValue));
        return items;
    }
}
