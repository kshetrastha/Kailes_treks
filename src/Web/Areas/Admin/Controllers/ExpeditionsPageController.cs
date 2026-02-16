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
[Route("admin/expeditions")]
public sealed class ExpeditionsPageController(IExpeditionService service, IExpeditionTypeService typeService, ICurrentUser currentUser, IWebHostEnvironment env) : Controller
{
    private const long MaxFileSizeInBytes = 10 * 1024 * 1024;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? type, string? difficulty, string? country, string? status, CancellationToken ct)
    {
        var result = await service.ListAsync(search, status, country, null, 1, 200, ct);
        if (!string.IsNullOrWhiteSpace(type)) result = result with { Items = result.Items.Where(x => string.Equals(x.ExpeditionTypeTitle, type, StringComparison.OrdinalIgnoreCase)).ToList() };
        if (!string.IsNullOrWhiteSpace(difficulty)) result = result with { Items = result.Items.Where(x => string.Equals(x.DifficultyLevel, difficulty, StringComparison.OrdinalIgnoreCase)).ToList() };
        ViewBag.DifficultyLevels = Enum.GetNames<DifficultyLevel>();
        ViewBag.Countries = Enum.GetNames<Country>();
        ViewBag.TravelStatuses = Enum.GetNames<TravelStatus>();
        return View(result);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(int step = 0, CancellationToken ct = default)
    {
        await LoadTypesAsync(ct);
        ViewBag.ActiveStep = Math.Max(0, step);
        return View("Upsert", new ExpeditionFormViewModel());
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, int step = 0, CancellationToken ct = default)
    {
        var e = await service.GetByIdAsync(id, ct);
        if (e is null) return NotFound();
        await LoadTypesAsync(ct);
        ViewBag.ActiveStep = Math.Max(0, step);

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
            Itineraries = e.Itineraries.Select(i => new ItineraryInput { Id = i.Id, SeasonTitle = i.SeasonTitle, SortOrder = i.SortOrder, Days = i.Days.Select(d => new ItineraryDayInput { Id = d.Id, DayNumber = d.DayNumber, ShortDescription = d.ShortDescription, Description = d.Description, Meals = d.Meals, AccommodationType = d.AccommodationType }).ToList() }).ToList(),
            CostItems = e.CostItems.Select(c => new CostItemInput { Id = c.Id, Title = c.Title, ShortDescription = c.ShortDescription, IsActive = c.IsActive, Type = c.Type, SortOrder = c.SortOrder }).ToList(),
            FixedDepartures = e.FixedDepartures.Select(f => new FixedDepartureInput { Id = f.Id, StartDate = f.StartDate, EndDate = f.EndDate, ForDays = f.ForDays, Status = f.Status, GroupSize = f.GroupSize }).ToList(),
            GearLists = e.GearLists.Select(g => new GearListInput { Id = g.Id, ExistingPath = g.FilePath, ShortDescription = g.ShortDescription }).ToList(),
            Maps = e.Maps.Select(m => new MapInput { Id = m.Id, ExistingPath = m.FilePath, Title = m.Title, Notes = m.Notes }).ToList(),
            Media = e.MediaItems.Select(m => new MediaInput { ExistingPath = m.FilePath ?? m.Url, Caption = m.Caption, VideoUrl = m.VideoUrl, SortOrder = m.Ordering }).ToList(),
            Highlights = e.Highlights.Select(h => new HighlightInput { Id = h.Id, Text = h.Text, SortOrder = h.SortOrder }).ToList(),
            Reviews = e.Reviews.Select(r => new ReviewInput { Id = r.Id, FullName = r.FullName, EmailAddress = r.EmailAddress, ExistingPhotoPath = r.UserPhotoPath, VideoUrl = r.VideoUrl, Rating = r.Rating, ReviewText = r.ReviewText, ModerationStatus = r.ModerationStatus }).ToList(),
            SectionsText = string.Empty,
            ItineraryText = string.Empty,
            FaqsText = string.Join('\n', e.Faqs.Select(x => $"{x.Question}|{x.Answer}|{x.Ordering}")),
            ReviewsText = string.Join('\n', e.Reviews.Select(x => $"{x.FullName}|{x.ReviewText}")),
            MediaText = string.Join('\n', e.MediaItems.Select(x => $"{x.Url}|{x.Caption}|{x.MediaType}|{x.Ordering}"))
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionFormViewModel m, int step = 0, string? intent = null, CancellationToken ct = default)
    {
        var build = await BuildDtoAsync(m, ct);
        if (!build.ok)
        {
            if (!string.IsNullOrWhiteSpace(build.error)) ModelState.AddModelError(string.Empty, build.error);
            await LoadTypesAsync(ct);
            ViewBag.ActiveStep = Math.Max(0, step);
            return View("Upsert", m);
        }

        var id = await service.CreateAsync(build.dto!, currentUser.UserId, ct);
        TempData["SuccessMessage"] = GetSaveMessage(step, intent, isCreate: true);
        return RedirectToAction(nameof(Edit), new { id, step = ResolveStepAfterSave(step, intent) });
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionFormViewModel m, int step = 0, string? intent = null, CancellationToken ct = default)
    {
        var build = await BuildDtoAsync(m, ct);
        if (!build.ok)
        {
            if (!string.IsNullOrWhiteSpace(build.error)) ModelState.AddModelError(string.Empty, build.error);
            await LoadTypesAsync(ct);
            ViewBag.ActiveStep = Math.Max(0, step);
            return View("Upsert", m);
        }

        var existing = await service.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var mergedDto = MergeByStep(ToUpsertDto(existing), build.dto!, step);
        var ok = await service.UpdateAsync(id, mergedDto, currentUser.UserId, ct);
        if (!ok) return NotFound();
        TempData["SuccessMessage"] = GetSaveMessage(step, intent, isCreate: false);
        return RedirectToAction(nameof(Edit), new { id, step = ResolveStepAfterSave(step, intent) });
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
    {
        ViewBag.ExpeditionTypes = await typeService.ListAsync(true, ct);
        ViewBag.DifficultyLevels = Enum.GetNames<DifficultyLevel>();
        ViewBag.Countries = Enum.GetNames<Country>();
        ViewBag.TravelStatuses = Enum.GetNames<TravelStatus>();
    }

    private async Task<(bool ok, ExpeditionUpsertDto? dto, string? error)> BuildDtoAsync(ExpeditionFormViewModel m, CancellationToken ct)
    {
        try
        {
            var faqs = ParseLines(m.FaqsText, 3, p => new ExpeditionFaqDto(p[0], p[1], ParseInt(p[2])));
            var mediaLegacy = ParseLines(m.MediaText, 4, p => new ExpeditionMediaDto(p[0], p[1], p[2], ParseInt(p[3]))).ToList();

            var maps = new List<ExpeditionMapDto>();
            foreach (var map in m.Maps)
            {
                var path = map.ExistingPath;
                if (map.UploadFile is { Length: > 0 }) path = await SaveFileAsync(map.UploadFile, "maps", new[] { ".pdf", ".jpg", ".png" }, ct);
                if (!string.IsNullOrWhiteSpace(path)) maps.Add(new ExpeditionMapDto(map.Id, path, map.Title, map.Notes));
            }

            var gear = new List<GearListDto>();
            var gearImageMedia = new List<ExpeditionMediaDto>();
            foreach (var item in m.GearLists)
            {
                var path = item.ExistingPath;
                if (item.UploadFile is { Length: > 0 }) path = await SaveFileAsync(item.UploadFile, "gear", new[] { ".pdf", ".doc", ".docx" }, ct);
                if (!string.IsNullOrWhiteSpace(path)) gear.Add(new GearListDto(item.Id, item.ShortDescription, path));

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

                var hasVideo = !string.IsNullOrWhiteSpace(item.VideoUrl);
                var hasImage = !string.IsNullOrWhiteSpace(filePath);
                if (hasImage || hasVideo)
                {
                    var mediaType = hasVideo ? "video" : "photo";
                    media.Add(new ExpeditionMediaDto(filePath ?? item.VideoUrl ?? string.Empty, item.Caption, mediaType, item.SortOrder, filePath, item.VideoUrl));
                }
            }

            var primaryMediaUrl = m.HeroImageUrl?.Trim();

            string? heroImagePath = null;
            if (m.HeroImageFile is { Length: > 0 })
            {
                heroImagePath = await SaveFileAsync(m.HeroImageFile, "expeditions", new[] { ".jpg", ".jpeg", ".png", ".webp" }, ct);
            }

            m.HeroImageUrl = !string.IsNullOrWhiteSpace(primaryMediaUrl)
                ? primaryMediaUrl
                : heroImagePath;

            if (mediaLegacy.Count == 0 && media.Count == 0 && gearImageMedia.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(primaryMediaUrl))
                {
                    var isVideoUrl = Uri.TryCreate(primaryMediaUrl, UriKind.Absolute, out var mediaUri)
                        && !string.IsNullOrWhiteSpace(mediaUri.AbsolutePath)
                        && !new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg", ".avif" }
                            .Any(ext => mediaUri.AbsolutePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
                    mediaLegacy.Add(new ExpeditionMediaDto(primaryMediaUrl, "Primary media", isVideoUrl ? "video" : "photo", 0, isVideoUrl ? null : primaryMediaUrl, isVideoUrl ? primaryMediaUrl : null));
                }

                if (!string.IsNullOrWhiteSpace(heroImagePath))
                {
                    mediaLegacy.Add(new ExpeditionMediaDto(heroImagePath, "Primary image", "photo", 1, heroImagePath, null));
                }
            }

            var reviews = BuildReviews(m.Reviews, m.ReviewsText);

            if (string.IsNullOrWhiteSpace(m.Difficulty) && !string.IsNullOrWhiteSpace(m.DifficultyLevel))
            {
                m.Difficulty = m.DifficultyLevel;
            }

            if (string.IsNullOrWhiteSpace(m.Destination) && !string.IsNullOrWhiteSpace(m.OverviewCountry))
            {
                m.Destination = m.OverviewCountry;
            }

            var dto = new ExpeditionUpsertDto(m.Name, m.Slug, m.ShortDescription, m.Destination, m.Region, m.DurationDays, m.MaxAltitudeMeters, m.Difficulty, m.BestSeason,
                m.Overview, m.Inclusions, m.Exclusions, m.HeroImageUrl, m.Permits, m.MinGroupSize, m.MaxGroupSize, m.Price, m.AvailableDates,
                m.BookingCtaUrl, m.SeoTitle, m.SeoDescription, m.Status, m.Featured, m.Ordering, m.SummitRoute, m.RequiresClimbingPermit,
                m.ExpeditionStyle, m.OxygenSupport, m.SherpaSupport, m.SummitBonusUsd, m.ExpeditionTypeId, [], [], faqs,
                mediaLegacy.Concat(media).Concat(gearImageMedia).ToList(), m.OverviewCountry, m.PeakName, m.OverviewDuration, m.Route, m.Rank, m.Latitude,
                m.Longitude, m.WeatherReport, m.Range, m.WalkingPerDay, m.Accommodation, m.GroupSizeText, m.DifficultyLevel,
                m.Itineraries.Select(i => new ItineraryDto(i.Id, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(d.Id, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
                maps, m.CostItems.Select(c => new CostItemDto(c.Id, c.Title, c.ShortDescription, c.IsActive, c.Type, c.SortOrder)).ToList(),
                m.FixedDepartures
                    .Where(f => f.StartDate.Year > 1900 && f.EndDate.Year > 1900)
                    .Select(f => new FixedDepartureDto(f.Id, f.StartDate, f.EndDate, f.ForDays, f.Status, f.GroupSize))
                    .ToList(),
                gear, m.Highlights.Select(h => new ExpeditionHighlightDto(h.Id, h.Text, h.SortOrder)).ToList(), reviews);

            return (true, dto, null);
        }
        catch (Exception ex)
        {
            var error = $"Unable to parse rows. {ex.Message}";
            return (false, null, error);
        }
    }

    private static IReadOnlyCollection<ExpeditionReviewDto> BuildReviews(IReadOnlyCollection<ReviewInput> reviewInputs, string reviewsText)
    {
        var reviews = new List<ExpeditionReviewDto>();
        foreach (var review in reviewInputs.Where(r => !string.IsNullOrWhiteSpace(r.FullName) && !string.IsNullOrWhiteSpace(r.ReviewText)))
        {
            var email = string.IsNullOrWhiteSpace(review.EmailAddress) ? $"{Guid.NewGuid():N}@placeholder.local" : review.EmailAddress;
            reviews.Add(new ExpeditionReviewDto(review.Id, review.FullName, email, review.ExistingPhotoPath, review.VideoUrl, review.Rating, review.ReviewText, review.ModerationStatus));
        }

        if (reviews.Count > 0)
        {
            return reviews;
        }

        return ParseLines(reviewsText, 6, p => new ExpeditionReviewDto(ParseInt(p[0]), p[1], string.IsNullOrWhiteSpace(p[2]) ? $"{Guid.NewGuid():N}@placeholder.local" : p[2], null, null, ParseInt(p[3]) <= 0 ? 5 : ParseInt(p[3]), p[5], string.IsNullOrWhiteSpace(p[4]) ? "Approved" : p[4]));
    }

    private static int ParseInt(string value)
        => int.TryParse(value, out var parsed) ? parsed : 0;

    private static int ResolveStepAfterSave(int step, string? intent)
    {
        var currentStep = Math.Max(0, step);
        if (string.Equals(intent, "next", StringComparison.OrdinalIgnoreCase))
        {
            return Math.Min(currentStep + 1, 5);
        }

        return currentStep;
    }

    private static string GetSaveMessage(int step, string? intent, bool isCreate)
    {
        var action = isCreate ? "created" : "updated";
        var tab = step switch
        {
            0 => "Basic info",
            1 => "Overview",
            2 => "Itineraries",
            3 => "Inclusions/Exclusions",
            4 => "Departure and Gear",
            5 => "Reviews and FAQs",
            _ => "Current tab"
        };

        if (string.Equals(intent, "next", StringComparison.OrdinalIgnoreCase))
        {
            return $"{tab} saved. Expedition {action}.";
        }

        return $"{tab} saved.";
    }

    private static ExpeditionUpsertDto MergeByStep(ExpeditionUpsertDto existing, ExpeditionUpsertDto incoming, int step)
    {
        return step switch
        {
            0 => existing with
            {
                Name = incoming.Name,
                Slug = incoming.Slug,
                ShortDescription = incoming.ShortDescription,
                Destination = incoming.Destination,
                Region = incoming.Region,
                ExpeditionTypeId = incoming.ExpeditionTypeId
            },
            1 => existing with
            {
                DurationDays = incoming.DurationDays,
                MaxAltitudeMeters = incoming.MaxAltitudeMeters,
                Difficulty = incoming.Difficulty,
                BestSeason = incoming.BestSeason,
                Overview = incoming.Overview,
                HeroImageUrl = incoming.HeroImageUrl,
                Permits = incoming.Permits,
                MinGroupSize = incoming.MinGroupSize,
                MaxGroupSize = incoming.MaxGroupSize,
                Price = incoming.Price,
                Status = incoming.Status,
                Ordering = incoming.Ordering,
                OverviewCountry = incoming.OverviewCountry,
                PeakName = incoming.PeakName,
                OverviewDuration = incoming.OverviewDuration,
                Route = incoming.Route,
                Rank = incoming.Rank,
                Latitude = incoming.Latitude,
                Longitude = incoming.Longitude,
                WeatherReport = incoming.WeatherReport,
                Range = incoming.Range,
                WalkingPerDay = incoming.WalkingPerDay,
                Accommodation = incoming.Accommodation,
                GroupSizeText = incoming.GroupSizeText,
                DifficultyLevel = incoming.DifficultyLevel,
                MediaItems = incoming.MediaItems
            },
            2 => existing with
            {
                Itineraries = MergeById(existing.Itineraries, incoming.Itineraries),
                MediaItems = MergeByKeys(existing.MediaItems, incoming.MediaItems, x => $"{x.Url}|{x.MediaType}|{x.Ordering}")
            },
            3 => existing with
            {
                Inclusions = incoming.Inclusions,
                Exclusions = incoming.Exclusions,
                CostItems = MergeById(existing.CostItems, incoming.CostItems)
            },
            4 => existing with
            {
                AvailableDates = incoming.AvailableDates,
                FixedDepartures = MergeById(existing.FixedDepartures, incoming.FixedDepartures),
                GearLists = MergeById(existing.GearLists, incoming.GearLists),
                Maps = MergeById(existing.Maps, incoming.Maps),
                MediaItems = MergeByKeys(existing.MediaItems, incoming.MediaItems, x => $"{x.Url}|{x.MediaType}|{x.Ordering}")
            },
            5 => existing with
            {
                Reviews = MergeById(existing.Reviews, incoming.Reviews),
                Faqs = MergeByKeys(existing.Faqs, incoming.Faqs, x => $"{x.Question}|{x.Answer}|{x.Ordering}")
            },
            _ => incoming
        };
    }

    private static ExpeditionUpsertDto ToUpsertDto(ExpeditionDetailsDto e)
        => new(e.Name, e.Slug, e.ShortDescription, e.Destination, e.Region, e.DurationDays, e.MaxAltitudeMeters, e.Difficulty, e.BestSeason,
            e.Overview, e.Inclusions, e.Exclusions, e.HeroImageUrl, e.Permits, e.MinGroupSize, e.MaxGroupSize, e.Price, e.AvailableDates,
            e.BookingCtaUrl, e.SeoTitle, e.SeoDescription, e.Status, e.Featured, e.Ordering, e.SummitRoute, e.RequiresClimbingPermit,
            e.ExpeditionStyle, e.OxygenSupport, e.SherpaSupport, e.SummitBonusUsd, e.ExpeditionTypeId, [], [], e.Faqs,
            e.MediaItems, e.OverviewCountry, e.PeakName, e.OverviewDuration, e.Route, e.Rank, e.Latitude, e.Longitude, e.WeatherReport,
            e.Range, e.WalkingPerDay, e.Accommodation, e.GroupSizeText, e.DifficultyLevel, e.Itineraries, e.Maps, e.CostItems,
            e.FixedDepartures, e.GearLists, e.Highlights, e.Reviews);

    private static IReadOnlyCollection<T> MergeById<T>(IReadOnlyCollection<T>? existing, IReadOnlyCollection<T>? incoming) where T : class
    {
        var existingItems = existing?.ToList() ?? [];
        var incomingItems = incoming?.ToList() ?? [];
        if (incomingItems.Count == 0) return existingItems;

        var keyProperty = typeof(T).GetProperty("Id");
        if (keyProperty is null) return incomingItems;

        var byId = existingItems
            .Where(x => GetIntId(keyProperty, x) > 0)
            .ToDictionary(x => GetIntId(keyProperty, x), x => x);

        foreach (var item in incomingItems)
        {
            var id = GetIntId(keyProperty, item);
            if (id > 0)
            {
                byId[id] = item;
            }
            else
            {
                existingItems.Add(item);
            }
        }

        return byId.Values.Concat(existingItems.Where(x => GetIntId(keyProperty, x) <= 0)).ToList();
    }

    private static int GetIntId(System.Reflection.PropertyInfo keyProperty, object item)
        => keyProperty.GetValue(item) is int id ? id : 0;

    private static IReadOnlyCollection<T> MergeByKeys<T>(IReadOnlyCollection<T>? existing, IReadOnlyCollection<T>? incoming, Func<T, string> keySelector)
    {
        var merged = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in existing ?? [])
        {
            merged[keySelector(item)] = item;
        }

        foreach (var item in incoming ?? [])
        {
            merged[keySelector(item)] = item;
        }

        return merged.Values.ToList();
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

    private static IReadOnlyCollection<T> ParseLines<T>(string text, int minParts, Func<string[], T> map)
    {
        var rows = new List<T>();
        if (string.IsNullOrWhiteSpace(text)) return rows;

        foreach (var raw in text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = raw.Split('|').Select(x => x.Trim()).ToArray();
            if (parts.Length < minParts)
            {
                continue;
            }

            rows.Add(map(parts));
        }

        return rows;
    }

}
