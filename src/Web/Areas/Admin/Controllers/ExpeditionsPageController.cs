using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/expeditions")]
public sealed class ExpeditionsPageController(IExpeditionService service, IExpeditionTypeService typeService, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await service.ListAsync(null, null, null, null, 1, 200, ct));

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadTypesAsync(ct);
        return View("Upsert", new ExpeditionFormViewModel());
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var e = await service.GetByIdAsync(id, ct);
        if (e is null) return NotFound();

        await LoadTypesAsync(ct);

        return View("Upsert", new ExpeditionFormViewModel
        {
            Id = e.Id,
            Name = e.Name,
            Slug = e.Slug,
            ShortDescription = e.ShortDescription,
            Destination = e.Destination,
            Region = e.Region,
            DurationDays = e.DurationDays,
            MaxAltitudeMeters = e.MaxAltitudeMeters,
            Difficulty = e.Difficulty,
            BestSeason = e.BestSeason,
            Overview = e.Overview,
            Inclusions = e.Inclusions,
            Exclusions = e.Exclusions,
            HeroImageUrl = e.HeroImageUrl,
            Permits = e.Permits,
            MinGroupSize = e.MinGroupSize,
            MaxGroupSize = e.MaxGroupSize,
            Price = e.Price,
            AvailableDates = e.AvailableDates,
            BookingCtaUrl = e.BookingCtaUrl,
            SeoTitle = e.SeoTitle,
            SeoDescription = e.SeoDescription,
            Status = e.Status,
            Featured = e.Featured,
            Ordering = e.Ordering,
            SummitRoute = e.SummitRoute,
            RequiresClimbingPermit = e.RequiresClimbingPermit,
            ExpeditionStyle = e.ExpeditionStyle,
            OxygenSupport = e.OxygenSupport,
            SherpaSupport = e.SherpaSupport,
            SummitBonusUsd = e.SummitBonusUsd,
            ExpeditionTypeId = e.ExpeditionTypeId,
            SectionsText = string.Join('\n', e.Sections.Select(x => $"{x.SectionType}|{x.Title}|{x.Content}|{x.Ordering}")),
            ItineraryText = string.Join('\n', e.ItineraryDays.Select(x => $"{x.DayNumber}|{x.Title}|{x.Description}|{x.OvernightLocation}")),
            FaqsText = string.Join('\n', e.Faqs.Select(x => $"{x.Question}|{x.Answer}|{x.Ordering}")),
            MediaText = string.Join('\n', e.MediaItems.Select(x => $"{x.Url}|{x.Caption}|{x.MediaType}|{x.Ordering}"))
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionFormViewModel m, CancellationToken ct)
    {
        if (!TryBuildDto(m, out var dto, out var error))
        {
            ModelState.AddModelError(string.Empty, error!);
            await LoadTypesAsync(ct);
            return View("Upsert", m);
        }

        await service.CreateAsync(dto!, currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionFormViewModel m, CancellationToken ct)
    {
        if (!TryBuildDto(m, out var dto, out var error))
        {
            ModelState.AddModelError(string.Empty, error!);
            await LoadTypesAsync(ct);
            return View("Upsert", m);
        }

        var ok = await service.UpdateAsync(id, dto!, currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Expedition updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await service.DeleteAsync(id, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Expedition deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadTypesAsync(CancellationToken ct)
        => ViewBag.ExpeditionTypes = await typeService.ListAsync(includeUnpublished: true, ct);

    private static bool TryBuildDto(ExpeditionFormViewModel m, out ExpeditionUpsertDto? dto, out string? error)
    {
        dto = null;
        error = null;

        try
        {
            var sections = ParseLines(m.SectionsText, (parts) =>
                new ExpeditionSectionDto(parts[0], parts[1], parts[2], int.Parse(parts[3])));
            var itinerary = ParseLines(m.ItineraryText, (parts) =>
                new ExpeditionItineraryDayDto(int.Parse(parts[0]), parts[1], parts[2], parts[3]));
            var faqs = ParseLines(m.FaqsText, (parts) =>
                new ExpeditionFaqDto(parts[0], parts[1], int.Parse(parts[2])));
            var media = ParseLines(m.MediaText, (parts) =>
                new ExpeditionMediaDto(parts[0], parts[1], parts[2], int.Parse(parts[3])));

            dto = new ExpeditionUpsertDto(
                m.Name, m.Slug, m.ShortDescription, m.Destination, m.Region, m.DurationDays,
                m.MaxAltitudeMeters, m.Difficulty, m.BestSeason, m.Overview, m.Inclusions,
                m.Exclusions, m.HeroImageUrl, m.Permits, m.MinGroupSize, m.MaxGroupSize,
                m.Price, m.AvailableDates, m.BookingCtaUrl, m.SeoTitle, m.SeoDescription,
                m.Status, m.Featured, m.Ordering, m.SummitRoute, m.RequiresClimbingPermit,
                m.ExpeditionStyle, m.OxygenSupport, m.SherpaSupport, m.SummitBonusUsd,
                m.ExpeditionTypeId, sections, itinerary, faqs, media);

            return true;
        }
        catch (Exception ex)
        {
            error = $"Unable to parse rows. Please use documented pipe-separated format. {ex.Message}";
            return false;
        }
    }

    private static IReadOnlyCollection<T> ParseLines<T>(string text, Func<string[], T> map)
    {
        var rows = new List<T>();
        if (string.IsNullOrWhiteSpace(text)) return rows;

        foreach (var raw in text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = raw.Split('|');
            rows.Add(map(parts.Select(x => x.Trim()).ToArray()));
        }

        return rows;
    }
}
