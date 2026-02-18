using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Web.Areas.Admin.Models;
using TravelCleanArch.Web.Models.Expeditions;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/expeditions")]
public sealed class ExpeditionsController(
    IExpeditionService service,
    IExpeditionTypeService typeService,
    IItineraryService itineraryService,
    IItineraryDayService itineraryDayService,
    ICurrentUser currentUser,
    IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? destination, string? difficulty, int page = 1, CancellationToken ct = default)
        => View(await service.ListAsync(search, null, destination, null, page, 50, ct));

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
            .Select(x => new ItineraryUpsertItemDto(x.Id, x.SeasonTitle, x.SortOrder))
            .ToList();

        if (rows.Count == 0)
        {
            TempData["ErrorMessage"] = "Add at least one itinerary row.";
            return RedirectToAction(nameof(Detail), new { id, activeTab = "itineraries" });
        }

        await itineraryService.UpsertForExpeditionAsync(id, rows, currentUser.UserId, ct);
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
            .Select(x => new ItineraryDayUpsertItemDto(x.Id, x.DayNumber, x.ShortDescription, x.Description, x.Meals, x.AccommodationType))
            .ToList();

        if (rows.Count == 0)
        {
            TempData["ErrorMessage"] = "Add at least one itinerary day row.";
            return RedirectToAction(nameof(Detail), new { id, activeTab = "itinerary-days", itineraryId = model.SelectedItineraryId });
        }

        await itineraryDayService.UpsertForItineraryAsync(model.SelectedItineraryId.Value, rows, currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Itinerary days saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "itinerary-days", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/maps"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailMaps(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Maps ?? [])
            .Where(x => x.UploadFile is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Notes))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, async details =>
        {
            var maps = new List<ExpeditionMapDto>();
            foreach (var row in rows)
            {
                var path = await SaveUploadedAssetAsync(row.UploadFile, "maps", row.ExistingPath, ct);
                if (string.IsNullOrWhiteSpace(path)) continue;
                maps.Add(new ExpeditionMapDto(row.Id, path, row.Title, row.Notes));
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
            .Select((x, i) => new CostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, "Inclusion", x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, details =>
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
            .Select((x, i) => new CostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, "Exclusion", x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, details =>
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
            .Where(x => x.StartDate != DateTime.MinValue || x.EndDate != DateTime.MinValue || x.ForDays > 0 || x.GroupSize.HasValue)
            .Select(x => new FixedDepartureDto(x.Id, x.StartDate, x.EndDate, x.ForDays, x.Status, x.GroupSize))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { FixedDepartures = rows }), ct);
        TempData["SuccessMessage"] = "Departures saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "departures", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/gear-lists"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailGearLists(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.GearLists ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.ShortDescription) || x.UploadFile is { Length: > 0 } || x.UploadImage is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.ExistingImagePath))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, async details =>
        {
            var gears = new List<GearListDto>();
            foreach (var row in rows)
            {
                var filePath = await SaveUploadedAssetAsync(row.UploadFile, "gear/files", row.ExistingPath, ct);
                var imagePath = await SaveUploadedAssetAsync(row.UploadImage, "gear/images", row.ExistingImagePath, ct);
                if (string.IsNullOrWhiteSpace(filePath) && string.IsNullOrWhiteSpace(imagePath)) continue;
                gears.Add(new GearListDto(row.Id, row.ShortDescription, filePath ?? string.Empty, imagePath ?? string.Empty));
            }
            return ToUpsertDto(details) with { GearLists = gears };
        }, ct);

        TempData["SuccessMessage"] = "Gear list saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "gear-lists", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/photos-videos"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailPhotosVideos(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.MediaItems ?? [])
            .Where(x => x.PhotoFile is { Length: > 0 } || !string.IsNullOrWhiteSpace(x.ExistingPath) || !string.IsNullOrWhiteSpace(x.VideoUrl) || !string.IsNullOrWhiteSpace(x.Caption))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, async details =>
        {
            var media = new List<ExpeditionMediaDto>();
            foreach (var row in rows)
            {
                var imagePath = await SaveUploadedAssetAsync(row.PhotoFile, "media/photos", row.ExistingPath, ct);
                var isVideo = !string.IsNullOrWhiteSpace(row.VideoUrl);
                if (!isVideo && string.IsNullOrWhiteSpace(imagePath))
                {
                    continue;
                }
                media.Add(new ExpeditionMediaDto(row.Id, isVideo ? row.VideoUrl! : imagePath!, row.Caption, isVideo ? "Video" : "Photo", row.SortOrder, imagePath, row.VideoUrl));
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

        await UpdateExpeditionCollectionsAsync(id, async details =>
        {
            var reviews = new List<ExpeditionReviewDto>();
            foreach (var row in rows)
            {
                var photoPath = await SaveUploadedAssetAsync(row.UserPhoto, "reviews", row.ExistingPhotoPath, ct);
                reviews.Add(new ExpeditionReviewDto(
                    row.Id,
                    row.FullName?.Trim() ?? string.Empty,
                    row.EmailAddress?.Trim() ?? string.Empty,
                    photoPath,
                    row.VideoUrl?.Trim(),
                    row.Rating,
                    row.ReviewText?.Trim() ?? string.Empty,
                    string.IsNullOrWhiteSpace(row.ModerationStatus) ? "Pending" : row.ModerationStatus));
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
            .Select((x, i) => new ExpeditionFaqDto(x.Id, x.Question, x.Answer, x.Ordering == 0 ? i + 1 : x.Ordering))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { Faqs = rows }), ct);
        TempData["SuccessMessage"] = "Faqs saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "faqs", itineraryId = model.SelectedItineraryId });
    }

    [HttpPost("{id:int}/detail/highlights"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetailHighlights(int id, ExpeditionItineraryTabsViewModel model, CancellationToken ct = default)
    {
        var rows = (model.Highlights ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Text))
            .Select((x, i) => new ExpeditionHighlightDto(x.Id, x.Text, x.SortOrder == 0 ? i + 1 : x.SortOrder))
            .ToList();

        await UpdateExpeditionCollectionsAsync(id, details => Task.FromResult(ToUpsertDto(details) with { Highlights = rows }), ct);
        TempData["SuccessMessage"] = "Highlights saved.";
        return RedirectToAction(nameof(Detail), new { id, activeTab = "highlights", itineraryId = model.SelectedItineraryId });
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(string? activeTab = null, CancellationToken ct = default)
    {
        ViewBag.ActiveTab = activeTab;
        await LoadDropdowns(ct);
        return View(new ExpeditionCreateUpdateModel());
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionCreateUpdateModel model, string? nextTab = null, CancellationToken ct = default)
    {
        ViewBag.ActiveTab = nextTab;

        if (!ModelState.IsValid)
        {
            await LoadDropdowns(ct);
            return View(model);
        }

        model.HeroImageUrl = await ResolveHeroImageUrlAsync(model, ct);
        var id = await service.CreateAsync(ToDto(model), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition created.";
        return RedirectToAction(nameof(Edit), new { id, activeTab = nextTab });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, string? activeTab = null, CancellationToken ct = default)
    {
        var item = await service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        ViewBag.ActiveTab = activeTab;
        await LoadDropdowns(ct);
        return View("Create", ToCreateUpdateModel(item));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionCreateUpdateModel model, string? nextTab = null, CancellationToken ct = default)
    {
        ViewBag.ActiveTab = nextTab;

        if (!ModelState.IsValid)
        {
            await LoadDropdowns(ct);
            return View("Create", model);
        }

        model.HeroImageUrl = await ResolveHeroImageUrlAsync(model, ct);
        if (!await service.UpdateAsync(id, ToDto(model), currentUser.UserId, ct)) return NotFound();
        TempData["SuccessMessage"] = "Expedition updated.";
        return RedirectToAction(nameof(Edit), new { id, activeTab = nextTab });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return RedirectToAction(nameof(Index));
    }

    private async Task<ExpeditionItineraryTabsViewModel?> BuildDetailModelAsync(int id, string activeTab, int? itineraryId, CancellationToken ct)
    {
        var expedition = await service.GetByIdAsync(id, ct);
        if (expedition is null) return null;

        var itineraryRows = await itineraryService.ListForExpeditionAsync(id, ct);
        var itineraryOptions = itineraryRows
            .Select(x => new SelectListItem(x.SeasonTitle, x.Id.ToString(), itineraryId == x.Id))
            .ToList();

        var selectedItineraryId = itineraryId ?? itineraryRows.FirstOrDefault()?.Id;
        if (selectedItineraryId.HasValue)
        {
            foreach (var option in itineraryOptions)
            {
                option.Selected = option.Value == selectedItineraryId.Value.ToString();
            }
        }

        var dayRows = selectedItineraryId.HasValue
            ? await itineraryDayService.ListForItineraryAsync(selectedItineraryId.Value, ct)
            : [];

        return new ExpeditionItineraryTabsViewModel
        {
            ExpeditionId = expedition.Id,
            ExpeditionName = expedition.Name,
            ActiveTab = activeTab,
            SelectedItineraryId = selectedItineraryId,
            ItineraryOptions = itineraryOptions,
            Itineraries = itineraryRows.Count > 0
                ? itineraryRows.Select(x => new ItineraryRowInput { Id = x.Id, SeasonTitle = x.SeasonTitle, SortOrder = x.SortOrder }).ToList()
                : [new ItineraryRowInput()],
            ItineraryDays = dayRows.Count > 0
                ? dayRows.Select(x => new ItineraryDayRowInput
                {
                    Id = x.Id,
                    DayNumber = x.DayNumber,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    Meals = x.Meals,
                    AccommodationType = x.AccommodationType
                }).ToList()
                : [new ItineraryDayRowInput()],
            Maps = expedition.Maps.Count > 0
                ? expedition.Maps.Select(x => new MapInput { Id = x.Id, ExistingPath = x.FilePath, Title = x.Title, Notes = x.Notes }).ToList()
                : [new MapInput()],
            CostIncludes = expedition.CostItems.Where(x => string.Equals(x.Type, "Inclusion", StringComparison.OrdinalIgnoreCase)).Any()
                ? expedition.CostItems.Where(x => string.Equals(x.Type, "Inclusion", StringComparison.OrdinalIgnoreCase)).Select(x => new CostItemInput { Id = x.Id, Title = x.Title, ShortDescription = x.ShortDescription, IsActive = x.IsActive, Type = x.Type, SortOrder = x.SortOrder }).ToList()
                : [new CostItemInput { Type = "Inclusion", IsActive = true }],
            CostExcludes = expedition.CostItems.Where(x => string.Equals(x.Type, "Exclusion", StringComparison.OrdinalIgnoreCase)).Any()
                ? expedition.CostItems.Where(x => string.Equals(x.Type, "Exclusion", StringComparison.OrdinalIgnoreCase)).Select(x => new CostItemInput { Id = x.Id, Title = x.Title, ShortDescription = x.ShortDescription, IsActive = x.IsActive, Type = x.Type, SortOrder = x.SortOrder }).ToList()
                : [new CostItemInput { Type = "Exclusion", IsActive = true }],
            FixedDepartures = expedition.FixedDepartures.Count > 0
                ? expedition.FixedDepartures.Select(x => new FixedDepartureInput { Id = x.Id, StartDate = x.StartDate, EndDate = x.EndDate, ForDays = x.ForDays, Status = x.Status, GroupSize = x.GroupSize }).ToList()
                : [new FixedDepartureInput()],
            GearLists = expedition.GearLists.Count > 0
                ? expedition.GearLists.Select(x => new GearListInput { Id = x.Id, ExistingPath = x.FilePath, ExistingImagePath = x.ImagePath, ShortDescription = x.ShortDescription }).ToList()
                : [new GearListInput()],
            MediaItems = expedition.MediaItems.Count > 0
                ? expedition.MediaItems.Select(x => new MediaInput { Id = x.Id, ExistingPath = x.FilePath ?? x.Url, VideoUrl = x.VideoUrl, Caption = x.Caption, SortOrder = x.Ordering }).ToList()
                : [new MediaInput()],
            Reviews = expedition.Reviews.Count > 0
                ? expedition.Reviews.Select(x => new ReviewInput { Id = x.Id, FullName = x.FullName, EmailAddress = x.EmailAddress, ExistingPhotoPath = x.UserPhotoPath, VideoUrl = x.VideoUrl, Rating = x.Rating, ReviewText = x.ReviewText, ModerationStatus = x.ModerationStatus }).ToList()
                : [new ReviewInput()],
            Faqs = expedition.Faqs.Count > 0
                ? expedition.Faqs.Select(x => new ExpeditionFaqInput { Id = x.Id, Question = x.Question, Answer = x.Answer, Ordering = x.Ordering }).ToList()
                : [new ExpeditionFaqInput()],
            Highlights = expedition.Highlights.Count > 0
                ? expedition.Highlights.Select(x => new HighlightInput { Id = x.Id, Text = x.Text, SortOrder = x.SortOrder }).ToList()
                : [new HighlightInput()]
        };
    }

    private async Task UpdateExpeditionCollectionsAsync(int id, Func<ExpeditionDetailsDto, Task<ExpeditionUpsertDto>> map, CancellationToken ct)
    {
        var details = await service.GetByIdAsync(id, ct);
        if (details is null) return;
        var dto = await map(details);
        await service.UpdateAsync(id, dto, currentUser.UserId, ct);
    }

    private static ExpeditionUpsertDto ToUpsertDto(ExpeditionDetailsDto x)
        => new(
            x.Name, x.Slug, x.ShortDescription, x.Destination, x.Region, x.DurationDays, x.MaxAltitudeMeters, x.MaxAltitudeFeet, x.Difficulty, x.BestSeason,
            x.Overview, x.Inclusions, x.Exclusions, x.HeroImageUrl, x.HeroVideoUrl, x.Permits, x.MinGroupSize, x.MaxGroupSize, x.PriceOnRequest, x.Price,
            x.CurrencyCode, x.PriceNotesUrl, x.TripPdfUrl, x.AvailableDates, x.BookingCtaUrl, x.SeoTitle, x.SeoDescription, x.Status, x.Featured, x.Ordering,
            x.SummitRoute, x.RequiresClimbingPermit, x.ExpeditionStyle, x.OxygenSupport, x.SherpaSupport, x.SummitBonusUsd, x.ExpeditionTypeId,
            x.Sections, [], x.Faqs, x.MediaItems, x.OverviewCountry, x.PeakName, x.OverviewDuration, x.Route, x.Rank, x.Latitude, x.Longitude,
            x.CoordinatesText, x.WeatherReport, x.Range, x.WalkingPerDay, x.Accommodation, x.GroupSizeText, x.DifficultyLevel, x.BoardBasis,
            x.AverageRating, x.RatingLabel, x.ReviewCount, x.Itineraries, x.Maps, x.CostItems, x.FixedDepartures, x.GearLists, x.Highlights, x.Reviews);

    private async Task<string?> SaveUploadedAssetAsync(IFormFile? upload, string subFolder, string? existingPath, CancellationToken ct)
    {
        if (upload is not { Length: > 0 }) return existingPath;

        var extension = Path.GetExtension(upload.FileName);
        var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "expeditions", subFolder);
        Directory.CreateDirectory(uploadsDirectory);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);
        await using var stream = System.IO.File.Create(filePath);
        await upload.CopyToAsync(stream, ct);
        return Path.Combine("uploads", "expeditions", subFolder, fileName).Replace('\\', '/');
    }

    private async Task LoadDropdowns(CancellationToken ct)
    {
        ViewBag.ExpeditionTypes = await typeService.ListAsync(true, ct);
        ViewBag.DifficultyLevels = Enum.GetNames<DifficultyLevel>();
        ViewBag.Seasons = Enum.GetNames<Season>();
        ViewBag.Countries = Enum.GetNames<Country>();
        ViewBag.TravelStatuses = Enum.GetNames<TravelStatus>();
    }

    private async Task<string?> ResolveHeroImageUrlAsync(ExpeditionCreateUpdateModel model, CancellationToken ct)
    {
        if (model.HeroImageFile is not { Length: > 0 }) return model.HeroImageUrl;

        var extension = Path.GetExtension(model.HeroImageFile.FileName);
        var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "expeditions");
        Directory.CreateDirectory(uploadsDirectory);
        var fileName = $"hero-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);
        await using var stream = System.IO.File.Create(filePath);
        await model.HeroImageFile.CopyToAsync(stream, ct);
        return Path.Combine("uploads", "expeditions", fileName).Replace('\\', '/');
    }

    private static ExpeditionUpsertDto ToDto(ExpeditionCreateUpdateModel m)
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
            m.SummitRoute,
            m.RequiresClimbingPermit,
            m.ExpeditionStyle,
            m.OxygenSupport,
            m.SherpaSupport,
            m.SummitBonusUsd,
            m.ExpeditionTypeId,
            [],
            [],
            m.Faqs.Select(x => new ExpeditionFaqDto(x.Id??0,x.Question, x.Answer, x.Ordering)).ToList(),
            m.Photos.Select(x => new ExpeditionMediaDto(x.Id ?? 0,x.Url, x.Caption, string.IsNullOrWhiteSpace(x.VideoUrl) ? "Photo" : "Video", x.Ordering, x.FilePath, x.VideoUrl)).ToList(),
            m.OverviewCountry.ToString(),
            m.PeakName,
            m.OverviewDuration,
            m.Route,
            m.Rank,
            m.Latitude,
            m.Longitude,
            m.CoordinatesText,
            m.WeatherReportUrl,
            m.Range,
            m.WalkingPerDay,
            m.Accommodation,
            m.GroupSizeText,
            m.DifficultyLevel?.ToString(),
            m.BoardBasis,
            m.AverageRating,
            m.RatingLabel,
            m.ReviewCount,
            [],
            [],
            [],
            [],
            [],
            [],
            []);

    //private static ExpeditionUpsertDto ToDto(ExpeditionFormViewModel m)
    //    => new(
    //        m.Name,
    //        m.Slug,
    //        m.ShortDescription,
    //        m.Destination,
    //        m.Region,
    //        m.DurationDays,
    //        m.MaxAltitudeMeters,
    //        m.MaxAltitudeFeet,
    //        m.Difficulty,
    //        m.BestSeason?.ToString(),
    //        m.Overview,
    //        m.Inclusions,
    //        m.Exclusions,
    //        m.HeroImageUrl,
    //        m.HeroVideoUrl,
    //        m.Permits,
    //        m.MinGroupSize,
    //        m.MaxGroupSize,
    //        m.PriceOnRequest,
    //        m.Price,
    //        m.CurrencyCode,
    //        m.PriceNotesUrl,
    //        m.TripPdfUrl,
    //        m.AvailableDates,
    //        m.BookingCtaUrl,
    //        m.SeoTitle,
    //        m.SeoDescription,
    //        m.Status.ToString(),
    //        m.Featured,
    //        m.Ordering,
    //        m.SummitRoute,
    //        m.RequiresClimbingPermit,
    //        m.ExpeditionStyle,
    //        m.OxygenSupport,
    //        m.SherpaSupport,
    //        m.SummitBonusUsd,
    //        m.ExpeditionTypeId,
    //        [],
    //        [],
    //        ParseFaqs(m.FaqsText),
    //        m.Media.Select(x => new ExpeditionMediaDto(x.Id,x.VideoUrl, x.Caption, string.IsNullOrWhiteSpace(x.VideoUrl) ? "Photo" : "Video", x.SortOrder, x.ExistingPath, x.VideoUrl)).ToList(),
    //        m.OverviewCountry.ToString(),
    //        m.PeakName,
    //        m.OverviewDuration,
    //        m.Route,
    //        m.Rank,
    //        m.Latitude,
    //        m.Longitude,
    //        m.CoordinatesText,
    //        m.WeatherReportUrl,
    //        m.Range,
    //        m.WalkingPerDay,
    //        m.Accommodation,
    //        m.GroupSizeText,
    //        m.DifficultyLevel?.ToString(),
    //        m.BoardBasis,
    //        m.AverageRating,
    //        m.RatingLabel,
    //        m.ReviewCount,
    //        m.Itineraries.Select(i => new ItineraryDto(i.Id, i.SeasonTitle, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(d.Id, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
    //        m.Maps.Select(x => new ExpeditionMapDto(x.Id, x.ExistingPath ?? string.Empty, x.Title, x.Notes)).ToList(),
    //        m.CostItems.Select(x => new CostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, x.Type, x.SortOrder)).ToList(),
    //        m.FixedDepartures.Select(x => new FixedDepartureDto(x.Id, x.StartDate, x.EndDate, x.ForDays, Enum.TryParse<DepartureStatus>(x.Status, true, out var status) ? status : DepartureStatus.BookingOpen, x.GroupSize)).ToList(),
    //        m.GearLists.Select(x => new GearListDto(x.Id, x.ShortDescription, x.ExistingPath ?? string.Empty, x.ExistingImagePath ?? string.Empty)).ToList(),
    //        m.Highlights.Select(x => new ExpeditionHighlightDto(x.Id, x.Text, x.SortOrder)).ToList(),
    //        m.Reviews.Select(x => new ExpeditionReviewDto(x.Id, x.FullName, x.EmailAddress, x.ExistingPhotoPath, x.VideoUrl, x.Rating, x.ReviewText, x.ModerationStatus)).ToList());

    //private static List<ExpeditionFaqDto> ParseFaqs(string? text)
    //    => (text ?? string.Empty)
    //        .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    //        .Select(x => x.Split('|'))
    //        .Where(x => x.Length >= 3)
    //       .Select(x => new ExpeditionFaqDto(x[0], x[0].Trim(), x[1].Trim(), int.TryParse(x[2], out var order) ? order : 0));

    private void ValidateCollections(ExpeditionFormViewModel model)
    {
        for (var i = 0; i < model.FixedDepartures.Count; i++)
        {
            var departure = model.FixedDepartures[i];
            if (departure.StartDate == DateTime.MinValue || departure.EndDate == DateTime.MinValue)
            {
                ModelState.AddModelError($"FixedDepartures[{i}]", "Departure start and end dates are required.");
            }
        }

        for (var i = 0; i < model.Reviews.Count; i++)
        {
            var review = model.Reviews[i];
            if (string.IsNullOrWhiteSpace(review.FullName) || string.IsNullOrWhiteSpace(review.EmailAddress) || string.IsNullOrWhiteSpace(review.ReviewText))
            {
                ModelState.AddModelError($"Reviews[{i}]", "Review name, email, and review text are required.");
            }
        }

        if (model.PriceOnRequest)
        {
            model.Price = null;
        }
    }

    private static void NormalizeOptionalCollections(ExpeditionFormViewModel model)
    {
        model.CostItems = model.CostItems
            .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.ShortDescription))
            .ToList();

        model.FixedDepartures = model.FixedDepartures
            .Where(x => x.StartDate != DateTime.MinValue || x.EndDate != DateTime.MinValue || x.GroupSize.HasValue || x.ForDays > 0)
            .ToList();

        model.GearLists = model.GearLists
            .Where(x => !string.IsNullOrWhiteSpace(x.ShortDescription) || !string.IsNullOrWhiteSpace(x.ExistingPath) || x.UploadFile is { Length: > 0 })
            .ToList();

        model.Maps = model.Maps
            .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Notes) || !string.IsNullOrWhiteSpace(x.ExistingPath) || x.UploadFile is { Length: > 0 })
            .ToList();

        model.Media = model.Media
            .Where(x => !string.IsNullOrWhiteSpace(x.Caption) || !string.IsNullOrWhiteSpace(x.VideoUrl) || !string.IsNullOrWhiteSpace(x.ExistingPath) || x.PhotoFile is { Length: > 0 })
            .ToList();

        model.Highlights = model.Highlights
            .Where(x => !string.IsNullOrWhiteSpace(x.Text))
            .ToList();

        model.Reviews = model.Reviews
            .Where(x => !string.IsNullOrWhiteSpace(x.FullName) || !string.IsNullOrWhiteSpace(x.EmailAddress) || !string.IsNullOrWhiteSpace(x.ReviewText) || !string.IsNullOrWhiteSpace(x.ExistingPhotoPath) || x.UserPhoto is { Length: > 0 })
            .ToList();
    }

    private static ExpeditionCreateUpdateModel ToCreateUpdateModel(ExpeditionDetailsDto m)
    {
        return new ExpeditionCreateUpdateModel
        {
            // =========================
            // Basic Info
            // =========================
            Id = m.Id,
            ExpeditionTypeId = m.ExpeditionTypeId,
            Name = m.Name,
            Slug = m.Slug,
            ShortDescription = m.ShortDescription,
            Featured = m.Featured,
            Ordering = m.Ordering,

            // =========================
            // Trip Facts
            // =========================
            Destination = m.Destination,
            Region = m.Region,
            DurationDays = m.DurationDays,
            MaxAltitudeMeters = m.MaxAltitudeMeters,
            MaxAltitudeFeet = m.MaxAltitudeFeet,
            DifficultyLevel = Enum.TryParse<DifficultyLevel>(m.DifficultyLevel ?? m.Difficulty, true, out var difficulty)
                ? difficulty
                : null,
            BestSeason = Enum.TryParse<Season>(m.BestSeason, true, out var season)
                ? season
                : null,
            WalkingPerDay = m.WalkingPerDay,
            Accommodation = m.Accommodation,

            // =========================
            // Overview
            // =========================
            Overview = m.Overview,
            OverviewCountry = Enum.TryParse<Country>(m.OverviewCountry, true, out var country)
                ? country
                : Country.Nepal,
            PeakName = m.PeakName,
            Route = m.Route,
            Rank = m.Rank,
            Latitude = m.Latitude,
            Longitude = m.Longitude,
            CoordinatesText = m.CoordinatesText,
            WeatherReportUrl = m.WeatherReport,
            Range = m.Range,
            OverviewDuration = m.OverviewDuration,

            // =========================
            // Hero
            // =========================
            HeroImageUrl = m.HeroImageUrl,
            HeroVideoUrl = m.HeroVideoUrl,

            // =========================
            // Group
            // =========================
            MinGroupSize = m.MinGroupSize,
            MaxGroupSize = m.MaxGroupSize,
            GroupSizeText = m.GroupSizeText,

            // =========================
            // Pricing
            // =========================
            PriceOnRequest = m.PriceOnRequest,
            Price = m.Price,
            CurrencyCode = m.CurrencyCode,
            PriceNotesUrl = m.PriceNotesUrl,
            TripPdfUrl = m.TripPdfUrl,

            // =========================
            // SEO
            // =========================
            SeoTitle = m.SeoTitle,
            SeoDescription = m.SeoDescription,

            // =========================
            // Rating
            // =========================
            AverageRating = m.AverageRating,
            RatingLabel = m.RatingLabel,
            ReviewCount = m.ReviewCount,

            // =========================
            // Extra
            // =========================
            Status = Enum.TryParse<TravelStatus>(m.Status, true, out var status)
                ? status
                : TravelStatus.Draft,
            ExpeditionStyle = m.ExpeditionStyle,
            BoardBasis = m.BoardBasis,
            OxygenSupport = m.OxygenSupport,
            SherpaSupport = m.SherpaSupport,
            SummitBonusUsd = m.SummitBonusUsd,
            Permits = m.Permits,
            RequiresClimbingPermit = m.RequiresClimbingPermit,

            // =========================
            // Legacy
            // =========================
            Inclusions = m.Inclusions,
            Exclusions = m.Exclusions,
            AvailableDates = m.AvailableDates,
            BookingCtaUrl = m.BookingCtaUrl,
            SummitRoute = m.SummitRoute,

            // =========================
            // Collections
            // =========================
            Faqs = m.Faqs?.Select(x => new ExpeditionFaqCreateUpdateModel
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Ordering = x.Ordering
            }).ToList() ?? new(),

            Photos = m.MediaItems?.Select(x => new ExpeditionMediaCreateUpdateModel
            {
                Id = x.Id,
                Url = x.Url,
                Caption = x.Caption,
                Ordering = x.Ordering,
                MediaKind = Enum.TryParse<ExpeditionMediaType>(x.MediaType, true, out var mediaType) ? mediaType : ExpeditionMediaType.Photo,
                FilePath = x.FilePath,
                VideoUrl = x.VideoUrl
            }).ToList() ?? new(),
            Itineraries = m.Itineraries?.Select(x => new ItineraryCreateUpdateModel
            {
                Id = x.Id,
                SeasonName = x.SeasonName,
                Title = x.Title,
                Day = x.Days.Count
            }).ToList() ?? new(),

            ItineraryDays = m.ItineraryDays?.Select(x => new ItineraryDayCreateUpdateModel
            {
                Id = x.Id,
                ItineraryId = x.ItineraryId,
                DayNumber = x.DayNumber,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Meals = x.Meals,
                AccommodationType = x.AccommodationType
            }).ToList() ?? new(),

            ExpeditionMaps = m.Maps?.Select(x => new ExpeditionMapCreateUpdateModel
            {
                Id = x.Id,
                FilePath = x.FilePath,
                Title = x.Title,
                Notes = x.Notes
            }).ToList() ?? new(),

            CostItems = m.CostItems?.Select(x => new CostItemCreateUpdateModel
            {
                Id = x.Id,
                Title = x.Title,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                Type = Enum.TryParse<CostItemType>(x.Type, true, out var costItemType) ? costItemType : CostItemType.Inclusion,
                ShortDescription = x.ShortDescription
            }).ToList() ?? new(),

            FixedDepartures = m.FixedDepartures?.Select(x => new FixedDepartureCreateUpdateModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ForDays = x.ForDays,
                Status = x.Status,
                GroupSize = x.GroupSize
            }).ToList() ?? new(),

            GearLists = m.GearLists?.Select(x => new GearListCreateUpdateModel
            {
                Id = x.Id,
                ShortDescription = x.ShortDescription,
                FilePath = x.FilePath,
                ImagePath = x.ImagePath,
            }).ToList() ?? new(),

            Highlights = m.Highlights?.Select(x => new ExpeditionHighlightCreateUpdateModel
            {
                Id = x.Id,
                Text = x.Text,
                SortOrder = x.SortOrder
            }).ToList() ?? new(),

            Reviews = m.Reviews?.Select(x => new ExpeditionReviewCreateUpdateModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Rating = x.Rating,
                ReviewText = x.ReviewText,
                VideoUrl = x.VideoUrl
            }).ToList() ?? new(),
        };
    }


    private static ExpeditionFormViewModel ToViewModel(ExpeditionDetailsDto m)
        => new()
        {
            Id = m.Id,
            Name = m.Name,
            Slug = m.Slug,
            ShortDescription = m.ShortDescription,
            Destination = m.Destination,
            Region = m.Region,
            DurationDays = m.DurationDays,
            MaxAltitudeMeters = m.MaxAltitudeMeters,
            MaxAltitudeFeet = m.MaxAltitudeFeet,
            Difficulty = m.Difficulty,
            DifficultyLevel = Enum.TryParse<DifficultyLevel>(m.DifficultyLevel, true, out var difficultyLevel) ? difficultyLevel : null,
            BestSeason = Enum.TryParse<Season>(m.BestSeason, true, out var bestSeason) ? bestSeason : null,
            WalkingPerDay = m.WalkingPerDay,
            Accommodation = m.Accommodation,
            Overview = m.Overview,
            OverviewCountry = Enum.TryParse<Country>(m.OverviewCountry, true, out var country) ? country : Country.Nepal,
            PeakName = m.PeakName,
            OverviewDuration = m.OverviewDuration,
            Route = m.Route,
            Rank = m.Rank,
            Latitude = m.Latitude,
            Longitude = m.Longitude,
            CoordinatesText = m.CoordinatesText,
            WeatherReportUrl = m.WeatherReport,
            Range = m.Range,
            HeroImageUrl = m.HeroImageUrl,
            HeroVideoUrl = m.HeroVideoUrl,
            MinGroupSize = m.MinGroupSize,
            MaxGroupSize = m.MaxGroupSize,
            GroupSizeText = m.GroupSizeText,
            PriceOnRequest = m.PriceOnRequest,
            Price = m.Price,
            CurrencyCode = m.CurrencyCode,
            PriceNotesUrl = m.PriceNotesUrl,
            TripPdfUrl = m.TripPdfUrl,
            SeoTitle = m.SeoTitle,
            SeoDescription = m.SeoDescription,
            AverageRating = m.AverageRating,
            RatingLabel = m.RatingLabel,
            ReviewCount = m.ReviewCount,
            Status = Enum.TryParse<TravelStatus>(m.Status, true, out var status) ? status : TravelStatus.Draft,
            Featured = m.Featured,
            Ordering = m.Ordering,
            ExpeditionStyle = m.ExpeditionStyle,
            BoardBasis = m.BoardBasis,
            OxygenSupport = m.OxygenSupport,
            SherpaSupport = m.SherpaSupport,
            SummitBonusUsd = m.SummitBonusUsd,
            Permits = m.Permits,
            RequiresClimbingPermit = m.RequiresClimbingPermit,
            SummitRoute = m.SummitRoute,
            ExpeditionTypeId = m.ExpeditionTypeId,
            Itineraries = m.Itineraries.Select(x => new ItineraryInput
            {
                Id = x.Id,
                SeasonTitle = x.SeasonName,
                SortOrder = x.SortOrder,
                Days = x.Days.Select(d => new ItineraryDayInput
                {
                    Id = d.Id,
                    DayNumber = d.DayNumber,
                    ShortDescription = d.ShortDescription,
                    Description = d.Description,
                    Meals = d.Meals,
                    AccommodationType = d.AccommodationType
                }).ToList()
            }).ToList(),
            CostItems = m.CostItems.Select(x => new CostItemInput { Id = x.Id, Title = x.Title, ShortDescription = x.ShortDescription, IsActive = x.IsActive, Type = x.Type, SortOrder = x.SortOrder }).ToList(),
            FixedDepartures = m.FixedDepartures.Select(x => new FixedDepartureInput { Id = x.Id, StartDate = x.StartDate, EndDate = x.EndDate, ForDays = x.ForDays, Status = x.Status, GroupSize = x.GroupSize }).ToList(),
            GearLists = m.GearLists.Select(x => new GearListInput { Id = x.Id, ExistingPath = x.FilePath, ShortDescription = x.ShortDescription }).ToList(),
            Maps = m.Maps.Select(x => new MapInput { Id = x.Id, ExistingPath = x.FilePath, Title = x.Title, Notes = x.Notes }).ToList(),
            Media = m.MediaItems.Select(x => new MediaInput { Id = 0, ExistingPath = x.FilePath, VideoUrl = x.VideoUrl, Caption = x.Caption, SortOrder = x.Ordering }).ToList(),
            Reviews = m.Reviews.Select(x => new ReviewInput { Id = x.Id, FullName = x.FullName, EmailAddress = x.EmailAddress, ExistingPhotoPath = x.UserPhotoPath, VideoUrl = x.VideoUrl, Rating = x.Rating, ReviewText = x.ReviewText, ModerationStatus = x.ModerationStatus }).ToList(),
            Highlights = m.Highlights.Select(x => new HighlightInput { Id = x.Id, Text = x.Text, SortOrder = x.SortOrder }).ToList(),
            FaqsText = string.Join("\n", m.Faqs.Select(x => $"{x.Question}|{x.Answer}|{x.Ordering}"))
        };
}
