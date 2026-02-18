using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/trekking")]
public sealed class TrekkingController(ITrekkingService service, ICurrentUser currentUser, IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? destination, string? status, int page = 1, CancellationToken ct = default)
        => View(await service.ListAsync(search, status, destination, null, page, 50, ct));

    [HttpGet("{id:int}/detail")]
    public async Task<IActionResult> Detail(int id, string activeTab = "itineraries", int? itineraryId = null, CancellationToken ct = default)
    {
        var model = await BuildDetailModelAsync(id, activeTab, itineraryId, ct);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost("{id:int}/detail/itineraries"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailItineraries(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Itineraries ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.SeasonTitle))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details =>
        {
            var itineraries = rows.Select((x, i) => new TrekkingItineraryDto(
                x.Id,
                x.SeasonTitle,
                x.SortOrder == 0 ? i + 1 : x.SortOrder,
                details.Itineraries.FirstOrDefault(it => it.Id == x.Id)?.Days ?? [])).ToList();

            return Task.FromResult(ToUpsertDto(details) with { Itineraries = itineraries });
        }, ct);

        TempData["SuccessMessage"] = "Itineraries saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "itineraries" });
    }

    [HttpPost("{id:int}/detail/itinerary-days"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailItineraryDays(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        if (!model.SelectedItineraryId.HasValue)
        {
            TempData["ErrorMessage"] = "Select itinerary first.";
            return RedirectToAction(nameof(Detail), new { id, activeTab = "itinerary-days" });
        }

        var rows = (model.ItineraryDays ?? [])
            .Where(x => x.DayNumber > 0 && (!string.IsNullOrWhiteSpace(x.ShortDescription) || !string.IsNullOrWhiteSpace(x.Description)))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details =>
        {
            var itineraries = details.Itineraries.Select(it =>
            {
                if (it.Id != model.SelectedItineraryId.Value) return it;

                var days = rows.Select(x => new TrekkingItineraryDayDto(
                    x.Id,
                    model.SelectedItineraryId.Value,
                    x.DayNumber,
                    x.ShortDescription,
                    x.Description,
                    x.Meals,
                    x.AccommodationType)).ToList();

                return it with { Days = days };
            }).ToList();

            return Task.FromResult(ToUpsertDto(details) with { Itineraries = itineraries });
        }, ct);

        TempData["SuccessMessage"] = "Itinerary days saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "itinerary-days", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/maps"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailMaps(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Maps ?? [])
            .Where(x => x.UploadFile is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Notes))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, async details =>
        {
            var maps = new List<TrekkingMapDto>();
            foreach (var row in rows)
            {
                var path = await SaveUploadedAssetAsync(row.UploadFile, "maps", row.ExistingPath, ct);
                if (string.IsNullOrWhiteSpace(path)) continue;
                maps.Add(new TrekkingMapDto(row.Id, path, row.Title, row.Notes));
            }
            return ToUpsertDto(details) with { Maps = maps };
        }, ct);

        TempData["SuccessMessage"] = "Maps saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "maps", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/cost-includes"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailCostIncludes(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.CostIncludes ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.ShortDescription))
            .Select((x, i) => new TrekkingCostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, "Inclusion", x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details =>
        {
            var combined = details.CostItems.Where(x => !string.Equals(x.Type, "Inclusion", StringComparison.OrdinalIgnoreCase)).ToList();
            combined.AddRange(rows);
            return Task.FromResult(ToUpsertDto(details) with { CostItems = combined });
        }, ct);

        TempData["SuccessMessage"] = "Cost includes saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "cost-includes", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/cost-excludes"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailCostExcludes(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.CostExcludes ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.ShortDescription))
            .Select((x, i) => new TrekkingCostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, "Exclusion", x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details =>
        {
            var combined = details.CostItems.Where(x => !string.Equals(x.Type, "Exclusion", StringComparison.OrdinalIgnoreCase)).ToList();
            combined.AddRange(rows);
            return Task.FromResult(ToUpsertDto(details) with { CostItems = combined });
        }, ct);

        TempData["SuccessMessage"] = "Cost excludes saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "cost-excludes", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/departures"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailDepartures(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.FixedDepartures ?? [])
            .Where(x => x.StartDate != DateTime.MinValue || x.EndDate != DateTime.MinValue || x.GroupSize.HasValue || x.ForDays > 0)
            .Select(x => new TrekkingFixedDepartureDto(x.Id, x.StartDate, x.EndDate, x.ForDays, x.Status, x.GroupSize))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { FixedDepartures = rows }), ct);

        TempData["SuccessMessage"] = "Fixed departures saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "departures", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/gear-lists"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailGearLists(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.GearLists ?? [])
            .Where(x => x.UploadFile is { Length: > 0 } || x.UploadImage is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.ExistingImagePath) || !string.IsNullOrWhiteSpace(x.ShortDescription))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, async details =>
        {
            var gears = new List<TrekkingGearListDto>();
            foreach (var row in rows)
            {
                var path = await SaveUploadedAssetAsync(row.UploadFile, "gear-lists", row.ExistingPath, ct);
                if (string.IsNullOrWhiteSpace(path)) continue;
                var imagePath = await SaveUploadedAssetAsync(row.UploadImage, "gear-lists", row.ExistingImagePath, ct);
                gears.Add(new TrekkingGearListDto(row.Id, row.ShortDescription, path, imagePath));
            }
            return ToUpsertDto(details) with { GearLists = gears };
        }, ct);

        TempData["SuccessMessage"] = "Gear lists saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "gear-lists", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/photos-videos"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailMedia(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.MediaItems ?? [])
            .Where(x => x.PhotoFile is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.VideoUrl) || !string.IsNullOrWhiteSpace(x.Caption))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, async details =>
        {
            var media = new List<TrekkingMediaDto>();
            foreach (var row in rows)
            {
                var imagePath = await SaveUploadedAssetAsync(row.PhotoFile, "media", row.ExistingPath, ct);
                var isVideo = !string.IsNullOrWhiteSpace(row.VideoUrl);
                if (!isVideo && string.IsNullOrWhiteSpace(imagePath)) continue;
                media.Add(new TrekkingMediaDto(row.Id, isVideo ? row.VideoUrl! : imagePath!, row.Caption, isVideo ? "Video" : "Photo", row.SortOrder, imagePath, row.VideoUrl));
            }
            return ToUpsertDto(details) with { MediaItems = media };
        }, ct);

        TempData["SuccessMessage"] = "Photos and videos saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "photos-videos", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/reviews"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailReviews(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Reviews ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.FullName) || !string.IsNullOrWhiteSpace(x.EmailAddress) || !string.IsNullOrWhiteSpace(x.ReviewText) || x.UserPhoto is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPhotoPath))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, async details =>
        {
            var reviews = new List<TrekkingReviewDto>();
            foreach (var row in rows)
            {
                var photoPath = await SaveUploadedAssetAsync(row.UserPhoto, "reviews", row.ExistingPhotoPath, ct);
                reviews.Add(new TrekkingReviewDto(row.Id, row.FullName?.Trim() ?? string.Empty, row.EmailAddress?.Trim() ?? string.Empty, photoPath, row.VideoUrl?.Trim(), row.Rating, row.ReviewText?.Trim() ?? string.Empty, string.IsNullOrWhiteSpace(row.ModerationStatus) ? "Pending" : row.ModerationStatus));
            }
            return ToUpsertDto(details) with { Reviews = reviews };
        }, ct);

        TempData["SuccessMessage"] = "Reviews saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "reviews", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/faqs"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailFaqs(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Faqs ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Question) || !string.IsNullOrWhiteSpace(x.Answer))
            .Select((x, i) => new TrekkingFaqDto(x.Id, x.Question, x.Answer, x.Ordering == 0 ? i + 1 : x.Ordering))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { Faqs = rows }), ct);
        TempData["SuccessMessage"] = "Faqs saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "faqs", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/highlights"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailHighlights(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Highlights ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Text))
            .Select((x, i) => new TrekkingHighlightDto(x.Id, x.Text, x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateTrekkingCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { Highlights = rows }), ct);
        TempData["SuccessMessage"] = "Highlights saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "highlights", itineraryId = model.SelectedItineraryId });
    }

    [HttpGet("create")]
    public IActionResult Create(string? activeTab = null)
    {
        ViewBag.ActiveTab = activeTab;
        LoadDropdowns();
        return View(new TrekkingAdminViewModel());
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrekkingAdminViewModel model, string? nextTab = null, CancellationToken ct = default)
    {
        ViewBag.ActiveTab = nextTab;
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View(model);
        }

        var id = await service.CreateAsync(ToDto(model), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Trekking created.";
        return RedirectToAction(nameof(Edit), new { id, activeTab = nextTab });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, string? activeTab = null, CancellationToken ct = default)
    {
        var details = await service.GetByIdAsync(id, ct);
        if (details is null) return NotFound();
        ViewBag.ActiveTab = activeTab;
        LoadDropdowns();
        return View("Create", ToViewModel(details));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrekkingAdminViewModel model, string? nextTab = null, CancellationToken ct = default)
    {
        ViewBag.ActiveTab = nextTab;
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View("Create", model);
        }

        var ok = await service.UpdateAsync(id, ToDto(model), currentUser.UserId, ct);
        if (!ok) return NotFound();

        TempData["SuccessMessage"] = "Trekking updated.";
        return RedirectToAction(nameof(Edit), new { id, activeTab = nextTab });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await service.DeleteAsync(id, ct);
        TempData["SuccessMessage"] = "Trekking deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ExpeditionItineraryTabsViewModel?> BuildDetailModelAsync(int id, string activeTab, int? itineraryId, CancellationToken ct)
    {
        var trekking = await service.GetByIdAsync(id, ct);
        if (trekking is null) return null;

        var itineraryRows = trekking.Itineraries.OrderBy(x => x.SortOrder).ToList();
        var itineraryOptions = itineraryRows.Select(x => new SelectListItem(x.SeasonName, x.Id.ToString(), itineraryId == x.Id)).ToList();

        var selectedItineraryId = itineraryId ?? itineraryRows.FirstOrDefault()?.Id;
        if (selectedItineraryId.HasValue)
        {
            foreach (var option in itineraryOptions) option.Selected = option.Value == selectedItineraryId.Value.ToString();
        }

        var dayRows = selectedItineraryId.HasValue
            ? itineraryRows.FirstOrDefault(x => x.Id == selectedItineraryId.Value)?.Days.OrderBy(x => x.DayNumber).ToList() ?? []
            : [];

        return new ExpeditionItineraryTabsViewModel
        {
            ExpeditionId = trekking.Id,
            ExpeditionName = trekking.Name,
            ActiveTab = activeTab,
            SelectedItineraryId = selectedItineraryId,
            ItineraryOptions = itineraryOptions,
            Itineraries = itineraryRows.Count > 0 ? itineraryRows.Select(x => new ItineraryRowInput { Id = x.Id, SeasonTitle = x.SeasonName, SortOrder = x.SortOrder }).ToList() : [new ItineraryRowInput()],
            ItineraryDays = dayRows.Count > 0 ? dayRows.Select(x => new ItineraryDayRowInput { Id = x.Id, DayNumber = x.DayNumber, ShortDescription = x.ShortDescription, Description = x.Description, Meals = x.Meals, AccommodationType = x.AccommodationType }).ToList() : [new ItineraryDayRowInput()],
            Maps = trekking.Maps.Count > 0 ? trekking.Maps.Select(x => new MapInput { Id = x.Id, ExistingPath = x.FilePath, Title = x.Title, Notes = x.Notes }).ToList() : [new MapInput()],
            CostIncludes = trekking.CostItems.Where(x => string.Equals(x.Type, "Inclusion", StringComparison.OrdinalIgnoreCase)).Any() ? trekking.CostItems.Where(x => string.Equals(x.Type, "Inclusion", StringComparison.OrdinalIgnoreCase)).Select(x => new CostItemInput { Id = x.Id, Title = x.Title, ShortDescription = x.ShortDescription, IsActive = x.IsActive, Type = x.Type, SortOrder = x.SortOrder }).ToList() : [new CostItemInput { Type = "Inclusion", IsActive = true }],
            CostExcludes = trekking.CostItems.Where(x => string.Equals(x.Type, "Exclusion", StringComparison.OrdinalIgnoreCase)).Any() ? trekking.CostItems.Where(x => string.Equals(x.Type, "Exclusion", StringComparison.OrdinalIgnoreCase)).Select(x => new CostItemInput { Id = x.Id, Title = x.Title, ShortDescription = x.ShortDescription, IsActive = x.IsActive, Type = x.Type, SortOrder = x.SortOrder }).ToList() : [new CostItemInput { Type = "Exclusion", IsActive = true }],
            FixedDepartures = trekking.FixedDepartures.Count > 0 ? trekking.FixedDepartures.Select(x => new FixedDepartureInput { Id = x.Id, StartDate = x.StartDate, EndDate = x.EndDate, ForDays = x.ForDays, Status = x.Status, GroupSize = x.GroupSize }).ToList() : [new FixedDepartureInput()],
            GearLists = trekking.GearLists.Count > 0 ? trekking.GearLists.Select(x => new GearListInput { Id = x.Id, ExistingPath = x.FilePath, ExistingImagePath = x.ImagePath, ShortDescription = x.ShortDescription }).ToList() : [new GearListInput()],
            MediaItems = trekking.MediaItems.Count > 0 ? trekking.MediaItems.Select(x => new MediaInput { Id = x.Id, ExistingPath = x.FilePath ?? x.Url, VideoUrl = x.VideoUrl, Caption = x.Caption, SortOrder = x.Ordering }).ToList() : [new MediaInput()],
            Reviews = trekking.Reviews.Count > 0 ? trekking.Reviews.Select(x => new ReviewInput { Id = x.Id, FullName = x.FullName, EmailAddress = x.EmailAddress, ExistingPhotoPath = x.UserPhotoPath, VideoUrl = x.VideoUrl, Rating = x.Rating, ReviewText = x.ReviewText, ModerationStatus = x.ModerationStatus }).ToList() : [new ReviewInput()],
            Faqs = trekking.Faqs.Count > 0 ? trekking.Faqs.Select(x => new ExpeditionFaqInput { Id = x.Id, Question = x.Question, Answer = x.Answer, Ordering = x.Ordering }).ToList() : [new ExpeditionFaqInput()],
            Highlights = trekking.Highlights.Count > 0 ? trekking.Highlights.Select(x => new HighlightInput { Id = x.Id, Text = x.Text, SortOrder = x.SortOrder }).ToList() : [new HighlightInput()]
        };
    }

    private async Task UpdateTrekkingCollectionsAsync(int id, Func<TrekkingDetailsDto, Task<TrekkingUpsertDto>> mutator, CancellationToken ct)
    {
        var details = await service.GetByIdAsync(id, ct);
        if (details is null) return;
        var upsert = await mutator(details);
        await service.UpdateAsync(id, upsert, currentUser.UserId, ct);
    }

    private async Task<string?> SaveUploadedAssetAsync(IFormFile? file, string folder, string? existingPath, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            return string.IsNullOrWhiteSpace(existingPath) ? null : existingPath;
        }

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var relative = $"/uploads/trekking/{folder}/{fileName}";
        var root = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(root)) return relative;

        var dir = Path.Combine(root, "uploads", "trekking", folder);
        Directory.CreateDirectory(dir);

        var fullPath = Path.Combine(dir, fileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);
        return relative;
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

    private static TrekkingUpsertDto ToUpsertDto(TrekkingDetailsDto m)
        => new(
            m.Name,
            m.Slug,
            m.ShortDescription,
            m.Destination,
            m.Region,
            m.DurationDays,
            m.MaxAltitudeMeters,
            m.MaxAltitudeFeet,
            m.Difficulty,
            m.BestSeason,
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
            m.Status,
            m.Featured,
            m.Ordering,
            m.SummitRoute,
            m.RequiresClimbingPermit,
            m.ExpeditionStyle,
            m.OxygenSupport,
            m.SherpaSupport,
            m.SummitBonusUsd,
            m.TrekkingTypeId,
            m.Faqs,
            m.MediaItems,
            m.OverviewCountry,
            m.PeakName,
            m.OverviewDuration,
            m.Route,
            m.Rank,
            m.Latitude,
            m.Longitude,
            m.CoordinatesText,
            m.WeatherReport,
            m.Range,
            m.WalkingPerDay,
            m.Accommodation,
            m.GroupSizeText,
            m.DifficultyLevel,
            m.BoardBasis,
            m.AverageRating,
            m.RatingLabel,
            m.ReviewCount,
            m.Itineraries,
            m.Maps,
            m.CostItems,
            m.FixedDepartures,
            m.GearLists,
            m.Highlights,
            m.Reviews);
}
