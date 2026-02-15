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
public sealed class ExpeditionsPageController(IExpeditionService service, IExpeditionTypeService typeService, ICurrentUser currentUser, IWebHostEnvironment env) : Controller
{
    private const long MaxFileSizeInBytes = 10 * 1024 * 1024;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? type, string? difficulty, string? destination, CancellationToken ct)
    {
        var result = await service.ListAsync(search, null, destination, null, 1, 200, ct);
        if (!string.IsNullOrWhiteSpace(type)) result = result with { Items = result.Items.Where(x => string.Equals(x.ExpeditionTypeTitle, type, StringComparison.OrdinalIgnoreCase)).ToList() };
        if (!string.IsNullOrWhiteSpace(difficulty)) result = result with { Items = result.Items.Where(x => string.Equals(x.DifficultyLevel, difficulty, StringComparison.OrdinalIgnoreCase)).ToList() };
        return View(result);
    }

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
            Id = e.Id, Name = e.Name, Slug = e.Slug, ShortDescription = e.ShortDescription, Destination = e.Destination, Region = e.Region,
            DurationDays = e.DurationDays, MaxAltitudeMeters = e.MaxAltitudeMeters, Difficulty = e.Difficulty, BestSeason = e.BestSeason,
            Overview = e.Overview, Inclusions = e.Inclusions, Exclusions = e.Exclusions, HeroImageUrl = e.HeroImageUrl, Permits = e.Permits,
            MinGroupSize = e.MinGroupSize, MaxGroupSize = e.MaxGroupSize, Price = e.Price, AvailableDates = e.AvailableDates,
            BookingCtaUrl = e.BookingCtaUrl, SeoTitle = e.SeoTitle, SeoDescription = e.SeoDescription, Status = e.Status, Featured = e.Featured,
            Ordering = e.Ordering, SummitRoute = e.SummitRoute, RequiresClimbingPermit = e.RequiresClimbingPermit, ExpeditionStyle = e.ExpeditionStyle,
            OxygenSupport = e.OxygenSupport, SherpaSupport = e.SherpaSupport, SummitBonusUsd = e.SummitBonusUsd, ExpeditionTypeId = e.ExpeditionTypeId,
            OverviewCountry = e.OverviewCountry, PeakName = e.PeakName, OverviewDuration = e.OverviewDuration, Route = e.Route, Rank = e.Rank,
            Latitude = e.Latitude, Longitude = e.Longitude, WeatherReport = e.WeatherReport, Range = e.Range, WalkingPerDay = e.WalkingPerDay,
            Accommodation = e.Accommodation, GroupSizeText = e.GroupSizeText, DifficultyLevel = e.DifficultyLevel,
            Itineraries = e.Itineraries.Select(i => new ItineraryInput { SeasonTitle = i.SeasonTitle, SortOrder = i.SortOrder, Days = i.Days.Select(d => new ItineraryDayInput { DayNumber = d.DayNumber, ShortDescription = d.ShortDescription, Description = d.Description, Meals = d.Meals, AccommodationType = d.AccommodationType }).ToList() }).ToList(),
            CostItems = e.CostItems.Select(c => new CostItemInput { Title = c.Title, ShortDescription = c.ShortDescription, IsActive = c.IsActive, Type = c.Type, SortOrder = c.SortOrder }).ToList(),
            FixedDepartures = e.FixedDepartures.Select(f => new FixedDepartureInput { StartDate = f.StartDate, EndDate = f.EndDate, ForDays = f.ForDays, Status = f.Status, GroupSize = f.GroupSize }).ToList(),
            GearLists = e.GearLists.Select(g => new GearListInput { ExistingPath = g.FilePath, ShortDescription = g.ShortDescription }).ToList(),
            Maps = e.Maps.Select(m => new MapInput { ExistingPath = m.FilePath, Title = m.Title, Notes = m.Notes }).ToList(),
            Media = e.MediaItems.Select(m => new MediaInput { ExistingPath = m.FilePath ?? m.Url, Caption = m.Caption, VideoUrl = m.VideoUrl, SortOrder = m.Ordering }).ToList(),
            Highlights = e.Highlights.Select(h => new HighlightInput { Text = h.Text, SortOrder = h.SortOrder }).ToList(),
            Reviews = e.Reviews.Select(r => new ReviewInput { FullName = r.FullName, EmailAddress = r.EmailAddress, ExistingPhotoPath = r.UserPhotoPath, VideoUrl = r.VideoUrl, Rating = r.Rating, ReviewText = r.ReviewText, ModerationStatus = r.ModerationStatus }).ToList(),
            SectionsText = string.Join('\n', e.Sections.Where(x => !string.Equals(x.SectionType, ExpeditionSectionTypes.Review, StringComparison.OrdinalIgnoreCase)).Select(x => $"{x.SectionType}|{x.Title}|{x.Content}|{x.Ordering}")),
            ItineraryText = string.Join('\n', e.ItineraryDays.Select(x => $"{x.DayNumber}|{x.Title}|{x.Description}|{x.OvernightLocation}")),
            FaqsText = string.Join('\n', e.Faqs.Select(x => $"{x.Question}|{x.Answer}|{x.Ordering}")),
            ReviewsText = string.Join('\n', e.Reviews.Select(x => $"{x.FullName}|{x.ReviewText}")),
            MediaText = string.Join('\n', e.MediaItems.Select(x => $"{x.Url}|{x.Caption}|{x.MediaType}|{x.Ordering}"))
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionFormViewModel m, CancellationToken ct)
    {
        var build = await BuildDtoAsync(m, ct);
        if (!build.ok)
        {
            if (!string.IsNullOrWhiteSpace(build.error)) ModelState.AddModelError(string.Empty, build.error);
            await LoadTypesAsync(ct);
            return View("Upsert", m);
        }

        await service.CreateAsync(build.dto!, currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionFormViewModel m, CancellationToken ct)
    {
        var build = await BuildDtoAsync(m, ct);
        if (!build.ok)
        {
            if (!string.IsNullOrWhiteSpace(build.error)) ModelState.AddModelError(string.Empty, build.error);
            await LoadTypesAsync(ct);
            return View("Upsert", m);
        }

        var ok = await service.UpdateAsync(id, build.dto!, currentUser.UserId, ct);
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

    private async Task LoadTypesAsync(CancellationToken ct) => ViewBag.ExpeditionTypes = await typeService.ListAsync(true, ct);

    private async Task<(bool ok, ExpeditionUpsertDto? dto, string? error)> BuildDtoAsync(ExpeditionFormViewModel m, CancellationToken ct)
    {
        try
        {
            var sections = ParseLines(m.SectionsText, p => new ExpeditionSectionDto(p[0], p[1], p[2], int.Parse(p[3])));
            var itinerary = ParseLines(m.ItineraryText, p => new ExpeditionItineraryDayDto(int.Parse(p[0]), p[1], p[2], p[3]));
            var faqs = ParseLines(m.FaqsText, p => new ExpeditionFaqDto(p[0], p[1], int.Parse(p[2])));
            var mediaLegacy = ParseLines(m.MediaText, p => new ExpeditionMediaDto(p[0], p[1], p[2], int.Parse(p[3])));

            var maps = new List<ExpeditionMapDto>();
            foreach (var map in m.Maps)
            {
                var path = map.ExistingPath;
                if (map.UploadFile is { Length: > 0 }) path = await SaveFileAsync(map.UploadFile, "maps", new[] { ".pdf", ".jpg", ".png" }, ct);
                if (!string.IsNullOrWhiteSpace(path)) maps.Add(new ExpeditionMapDto(0, path, map.Title, map.Notes));
            }

            var gear = new List<GearListDto>();
            var gearImageMedia = new List<ExpeditionMediaDto>();
            foreach (var item in m.GearLists)
            {
                var path = item.ExistingPath;
                if (item.UploadFile is { Length: > 0 }) path = await SaveFileAsync(item.UploadFile, "gear", new[] { ".pdf", ".doc", ".docx" }, ct);
                if (!string.IsNullOrWhiteSpace(path)) gear.Add(new GearListDto(0, item.ShortDescription, path));

                var imagePath = item.ExistingImagePath;
                if (item.UploadImage is { Length: > 0 }) imagePath = await SaveFileAsync(item.UploadImage, "gear-images", new[] { ".jpg", ".jpeg", ".png", ".webp" }, ct);
                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    var caption = string.IsNullOrWhiteSpace(item.ShortDescription) ? "Gear display image" : $"Gear image: {item.ShortDescription}";
                    gearImageMedia.Add(new ExpeditionMediaDto(imagePath, caption, "photo", 0, imagePath, null));
                }
            }

            var media = new List<ExpeditionMediaDto>();
            foreach (var item in m.Media)
            {
                var filePath = item.ExistingPath;
                if (item.PhotoFile is { Length: > 0 }) filePath = await SaveFileAsync(item.PhotoFile, "expedition-media", new[] { ".jpg", ".jpeg", ".png", ".webp" }, ct);
                if (!string.IsNullOrWhiteSpace(filePath) || !string.IsNullOrWhiteSpace(item.VideoUrl)) media.Add(new ExpeditionMediaDto(filePath ?? item.VideoUrl ?? string.Empty, item.Caption, string.IsNullOrWhiteSpace(item.VideoUrl) ? "photo" : "video", item.SortOrder, filePath, item.VideoUrl));
            }

            var reviews = ParseLines(m.ReviewsText, p => new ExpeditionReviewDto(0, p[0], $"{Guid.NewGuid():N}@placeholder.local", null, null, 5, p[1], "Approved"));

            var dto = new ExpeditionUpsertDto(m.Name, m.Slug, m.ShortDescription, m.Destination, m.Region, m.DurationDays, m.MaxAltitudeMeters, m.Difficulty, m.BestSeason,
                m.Overview, m.Inclusions, m.Exclusions, m.HeroImageUrl, m.Permits, m.MinGroupSize, m.MaxGroupSize, m.Price, m.AvailableDates,
                m.BookingCtaUrl, m.SeoTitle, m.SeoDescription, m.Status, m.Featured, m.Ordering, m.SummitRoute, m.RequiresClimbingPermit,
                m.ExpeditionStyle, m.OxygenSupport, m.SherpaSupport, m.SummitBonusUsd, m.ExpeditionTypeId, sections, itinerary, faqs,
                mediaLegacy.Concat(media).Concat(gearImageMedia).ToList(), m.OverviewCountry, m.PeakName, m.OverviewDuration, m.Route, m.Rank, m.Latitude,
                m.Longitude, m.WeatherReport, m.Range, m.WalkingPerDay, m.Accommodation, m.GroupSizeText, m.DifficultyLevel,
                m.Itineraries.Select(i => new ItineraryDto(0, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(0, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
                maps, m.CostItems.Select(c => new CostItemDto(0, c.Title, c.ShortDescription, c.IsActive, c.Type, c.SortOrder)).ToList(),
                m.FixedDepartures.Select(f => new FixedDepartureDto(0, f.StartDate, f.EndDate, f.ForDays, f.Status, f.GroupSize)).ToList(),
                gear, m.Highlights.Select(h => new ExpeditionHighlightDto(0, h.Text, h.SortOrder)).ToList(), reviews);

            return (true, dto, null);
        }
        catch (Exception ex)
        {
            var error = $"Unable to parse rows. {ex.Message}";
            return (false, null, error);
        }
    }

    private async Task<string> SaveFileAsync(IFormFile file, string folder, IEnumerable<string> exts, CancellationToken ct)
    {
        if (file.Length > MaxFileSizeInBytes) throw new InvalidOperationException("File too large.");
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!exts.Contains(ext)) throw new InvalidOperationException($"Invalid file type: {ext}");
        var target = Path.Combine(env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(target);
        var name = $"{folder}-{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(target, name);
        await using var fs = System.IO.File.Create(path);
        await file.CopyToAsync(fs, ct);
        return Path.Combine("uploads", folder, name).Replace('\\', '/');
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
