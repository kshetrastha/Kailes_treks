using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/trekking")]
public sealed class TrekkingController(ITrekkingService service, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? destination, string? status, int page = 1, CancellationToken ct = default)
        => View(await service.ListAsync(search, status, destination, null, page, 50, ct));

    [HttpGet("create")]
    public IActionResult Create()
    {
        LoadDropdowns();
        return View(new TrekkingAdminViewModel());
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrekkingAdminViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View(model);
        }

        var id = await service.CreateAsync(ToDto(model), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Trekking created.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
    {
        var details = await service.GetByIdAsync(id, ct);
        if (details is null) return NotFound();

        LoadDropdowns();
        return View("Create", ToViewModel(details));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrekkingAdminViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View("Create", model);
        }

        var ok = await service.UpdateAsync(id, ToDto(model), currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Trekking updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await service.DeleteAsync(id, ct);
        TempData["SuccessMessage"] = "Trekking deleted.";
        return RedirectToAction(nameof(Index));
    }

    private void LoadDropdowns()
    {
        ViewBag.DifficultyLevels = Enum.GetNames<DifficultyLevel>();
        ViewBag.Seasons = Enum.GetNames<Season>();
        ViewBag.Statuses = Enum.GetNames<TravelStatus>();
    }

    private static TrekkingAdminViewModel ToViewModel(TrekkingDetailsDto x)
        => new()
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Destination = x.Destination,
            Region = x.Region,
            DurationDays = x.DurationDays,
            MaxAltitudeMeters = x.MaxAltitudeMeters,
            MaxAltitudeFeet = x.MaxAltitudeFeet,
            DifficultyLevel = Enum.TryParse<DifficultyLevel>(x.DifficultyLevel ?? x.Difficulty, true, out var d) ? d : null,
            BestSeason = Enum.TryParse<Season>(x.BestSeason, true, out var s) ? s : null,
            Overview = x.Overview,
            Inclusions = x.Inclusions,
            Exclusions = x.Exclusions,
            HeroImageUrl = x.HeroImageUrl,
            HeroVideoUrl = x.HeroVideoUrl,
            Permits = x.Permits,
            MinGroupSize = x.MinGroupSize,
            MaxGroupSize = x.MaxGroupSize,
            PriceOnRequest = x.PriceOnRequest,
            Price = x.Price,
            CurrencyCode = x.CurrencyCode,
            PriceNotesUrl = x.PriceNotesUrl,
            TripPdfUrl = x.TripPdfUrl,
            AvailableDates = x.AvailableDates,
            BookingCtaUrl = x.BookingCtaUrl,
            SeoTitle = x.SeoTitle,
            SeoDescription = x.SeoDescription,
            Status = Enum.TryParse<TravelStatus>(x.Status, true, out var st) ? st : TravelStatus.Draft,
            Featured = x.Featured,
            Ordering = x.Ordering,
            TrekkingTypeId = x.TrekkingTypeId
        };

    private static TrekkingUpsertDto ToDto(TrekkingAdminViewModel m)
        => new(
            m.Name,
            m.Slug,
            m.ShortDescription,
            m.Destination,
            m.Region,
            m.DurationDays,
            m.MaxAltitudeMeters,
            m.MaxAltitudeFeet,
            m.DifficultyLevel?.ToString() ?? string.Empty,
            m.BestSeason?.ToString(),
            m.Overview,
            m.Inclusions,
            m.Exclusions,
            m.HeroImageUrl,
            m.HeroVideoUrl,
            m.Permits,
            m.MinGroupSize,
            m.MaxGroupSize,
            m.PriceOnRequest,
            m.Price,
            m.CurrencyCode,
            m.PriceNotesUrl,
            m.TripPdfUrl,
            m.AvailableDates,
            m.BookingCtaUrl,
            m.SeoTitle,
            m.SeoDescription,
            m.Status.ToString(),
            m.Featured,
            m.Ordering,
            null,
            false,
            null,
            false,
            false,
            null,
            m.TrekkingTypeId,
            [],
            [],
            DifficultyLevel: m.DifficultyLevel?.ToString());
}
