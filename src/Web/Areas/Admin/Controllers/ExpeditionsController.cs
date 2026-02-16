using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    ICurrentUser currentUser,
    IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? destination, string? difficulty, int page = 1, CancellationToken ct = default)
        => View(await service.ListAsync(search, null, destination, null, page, 50, ct));

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadDropdowns(ct);
        return View(new ExpeditionCreateUpdateModel());
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionCreateUpdateModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns(ct);
            return View(model);
        }

        model.HeroImageUrl = await ResolveHeroImageUrlAsync(model, ct);
        var id = await service.CreateAsync(ToDto(model), currentUser.UserId, ct);
        TempData["SuccessMessage"] = "Expedition created.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        await LoadDropdowns(ct);
        return View("Create", ToCreateUpdateModel(item));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionCreateUpdateModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns(ct);
            return View("Create", model);
        }

        model.HeroImageUrl = await ResolveHeroImageUrlAsync(model, ct);
        if (!await service.UpdateAsync(id, ToDto(model), currentUser.UserId, ct)) return NotFound();
        TempData["SuccessMessage"] = "Expedition updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return RedirectToAction(nameof(Index));
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
            m.Faqs.Select(x => new ExpeditionFaqDto(x.Question, x.Answer, x.Ordering)).ToList(),
            m.Photos.Select(x => new ExpeditionMediaDto(x.Url, x.Caption, string.IsNullOrWhiteSpace(x.VideoUrl) ? "Photo" : "Video", x.Ordering, x.FilePath, x.VideoUrl)).ToList(),
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

    private static ExpeditionUpsertDto ToDto(ExpeditionFormViewModel m)
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
            ParseFaqs(m.FaqsText),
            m.Media.Select(x => new ExpeditionMediaDto(string.Empty, x.Caption, string.IsNullOrWhiteSpace(x.VideoUrl) ? "Photo" : "Video", x.SortOrder, x.ExistingPath, x.VideoUrl)).ToList(),
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
            m.Itineraries.Select(i => new ItineraryDto(i.Id, i.SeasonTitle, i.SortOrder, i.Days.Select(d => new ItineraryDayDto(d.Id, d.DayNumber, d.ShortDescription, d.Description, d.Meals, d.AccommodationType)).ToList())).ToList(),
            m.Maps.Select(x => new ExpeditionMapDto(x.Id, x.ExistingPath ?? string.Empty, x.Title, x.Notes)).ToList(),
            m.CostItems.Select(x => new CostItemDto(x.Id, x.Title, x.ShortDescription, x.IsActive, x.Type, x.SortOrder)).ToList(),
            m.FixedDepartures.Select(x => new FixedDepartureDto(x.Id, x.StartDate, x.EndDate, x.ForDays, x.Status, x.GroupSize)).ToList(),
            m.GearLists.Select(x => new GearListDto(x.Id, x.ShortDescription, x.ExistingPath ?? string.Empty)).ToList(),
            m.Highlights.Select(x => new ExpeditionHighlightDto(x.Id, x.Text, x.SortOrder)).ToList(),
            m.Reviews.Select(x => new ExpeditionReviewDto(x.Id, x.FullName, x.EmailAddress, x.ExistingPhotoPath, x.VideoUrl, x.Rating, x.ReviewText, x.ModerationStatus)).ToList());

    private static List<ExpeditionFaqDto> ParseFaqs(string? text)
        => (text ?? string.Empty)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split('|'))
            .Where(x => x.Length >= 3)
            .Select(x => new ExpeditionFaqDto(x[0].Trim(), x[1].Trim(), int.TryParse(x[2], out var order) ? order : 0))
            .ToList();

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
        => new()
        {
            Id = m.Id,
            Name = m.Name,
            Slug = m.Slug,
            ExpeditionTypeId = m.ExpeditionTypeId,
            Destination = m.Destination,
            Region = m.Region,
            DurationDays = m.DurationDays,
            ShortDescription = m.ShortDescription,
            DifficultyLevel = Enum.TryParse<DifficultyLevel>(m.DifficultyLevel, true, out var difficultyLevel) ? difficultyLevel : null,
            BestSeason = Enum.TryParse<Season>(m.BestSeason, true, out var bestSeason) ? bestSeason : null,
            Featured = m.Featured,
            Status = Enum.TryParse<TravelStatus>(m.Status, true, out var status) ? status : TravelStatus.Draft,
            OverviewCountry = Enum.TryParse<Country>(m.OverviewCountry, true, out var country) ? country : Country.Nepal,
            PeakName = m.PeakName,
            Route = m.Route,
            Rank = m.Rank,
            Range = m.Range,
            CoordinatesText = m.CoordinatesText,
            WeatherReportUrl = m.WeatherReport,
            OverviewDuration = m.OverviewDuration,
            Overview = m.Overview,
            HeroImageUrl = m.HeroImageUrl
        };

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
                SeasonTitle = x.SeasonTitle,
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
