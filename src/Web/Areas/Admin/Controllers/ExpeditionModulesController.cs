using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/expedition-modules")]
public sealed class ExpeditionModulesController(IExpeditionModuleService service, IExpeditionTypeService typeService, ICurrentUser currentUser) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct) => View(await service.ListBasicInfosAsync(ct));

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadTypes(ct);
        return View("Edit", new ExpeditionModuleFormViewModel());
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpeditionModuleFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadTypes(ct);
            return View("Edit", model);
        }

        var id = await service.CreateBasicInfoAsync(ToBasicDto(model), currentUser.UserId, ct);
        await SaveSections(id, model, ct);
        TempData["ok"] = "Expedition created.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await service.GetDetailsAsync(id, ct);
        if (item is null) return NotFound();
        await LoadTypes(ct);
        return View(MapForm(item));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpeditionModuleFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadTypes(ct);
            return View(model);
        }

        if (!await service.UpdateBasicInfoAsync(id, ToBasicDto(model), currentUser.UserId, ct)) return NotFound();
        await SaveSections(id, model, ct);
        TempData["ok"] = "Expedition updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteBasicInfoAsync(id, ct);
        return RedirectToAction(nameof(Index));
    }

    private async Task SaveSections(int id, ExpeditionModuleFormViewModel model, CancellationToken ct)
    {
        await service.UpsertOverviewAsync(id, new ExpeditionOverviewUpsertDto(model.Overview.Country, model.Overview.PeakName, model.Overview.Route, model.Overview.Rank, model.Overview.Range, model.Overview.Coordinates, model.Overview.WeatherInformation, model.Overview.FullDescription, model.Overview.MapEmbedCode), currentUser.UserId, ct);
        await service.ReplaceItinerariesAsync(id, model.Itineraries.Select(x => new ExpeditionItineraryUpsertDto(x.SeasonTitle, x.DayNumber, x.Title, x.ShortDescription, x.FullDescription, x.Accommodation, x.Meals, x.Elevation)).ToList(), currentUser.UserId, ct);
        await service.ReplaceInclusionExclusionsAsync(id, model.InclusionExclusions.Select(x => new ExpeditionInclusionExclusionUpsertDto(x.Type, x.Description, x.DisplayOrder)).ToList(), currentUser.UserId, ct);
        await service.ReplaceFixedDeparturesAsync(id, model.FixedDepartures.Select(x => new ExpeditionFixedDepartureUpsertDto(x.StartDate, x.EndDate, x.TotalSeats, x.BookedSeats, x.Price, x.Currency, x.Status, x.IsGuaranteed)).ToList(), currentUser.UserId, ct);
        await service.ReplaceGearsAsync(id, model.Gears.Select(x => new ExpeditionGearUpsertDto(x.Category, x.ItemName, x.IsMandatory, x.DisplayOrder)).ToList(), currentUser.UserId, ct);
        await service.ReplaceReviewsAsync(id, model.Reviews.Select(x => new ExpeditionReviewUpsertDto(x.ClientName, x.Country, x.Rating, x.Title, x.Comment, x.ImagePath, x.IsApproved)).ToList(), currentUser.UserId, ct);
        await service.ReplaceFaqsAsync(id, model.Faqs.Select(x => new ExpeditionFaqUpsertDto(x.Question, x.Answer, x.DisplayOrder)).ToList(), currentUser.UserId, ct);
    }

    private async Task LoadTypes(CancellationToken ct) => ViewBag.ExpeditionTypes = await typeService.ListAsync(true, ct);

    private static ExpeditionBasicInfoUpsertDto ToBasicDto(ExpeditionModuleFormViewModel m)
        => new(m.ExpeditionTypeId, m.Name, m.ShortDescription, m.DifficultyLevel, m.MaxElevation, m.Duration, m.WalkingHoursPerDay, m.Accommodation, m.BestSeason, m.GroupSize, m.IsFeatured, m.BannerImagePath, m.ThumbnailImagePath);

    private static ExpeditionModuleFormViewModel MapForm(ExpeditionModuleDetailsDto m)
        => new()
        {
            Id = m.Id,
            ExpeditionTypeId = m.ExpeditionTypeId,
            Name = m.Name,
            ShortDescription = m.ShortDescription,
            DifficultyLevel = m.DifficultyLevel,
            MaxElevation = m.MaxElevation,
            Duration = m.Duration,
            WalkingHoursPerDay = m.WalkingHoursPerDay,
            Accommodation = m.Accommodation,
            BestSeason = m.BestSeason,
            GroupSize = m.GroupSize,
            IsFeatured = m.IsFeatured,
            BannerImagePath = m.BannerImagePath,
            ThumbnailImagePath = m.ThumbnailImagePath,
            Overview = new ExpeditionOverviewFormViewModel
            {
                Country = m.Overview?.Country,
                PeakName = m.Overview?.PeakName,
                Route = m.Overview?.Route,
                Rank = m.Overview?.Rank,
                Range = m.Overview?.Range,
                Coordinates = m.Overview?.Coordinates,
                WeatherInformation = m.Overview?.WeatherInformation,
                FullDescription = m.Overview?.FullDescription,
                MapEmbedCode = m.Overview?.MapEmbedCode
            },
            Itineraries = m.Itineraries.Select(x => new ExpeditionItineraryFormRow { SeasonTitle = x.SeasonTitle, DayNumber = x.DayNumber, Title = x.Title, ShortDescription = x.ShortDescription, FullDescription = x.FullDescription, Accommodation = x.Accommodation, Meals = x.Meals, Elevation = x.Elevation }).ToList(),
            InclusionExclusions = m.InclusionExclusions.Select(x => new ExpeditionInclusionExclusionFormRow { Type = x.Type, Description = x.Description, DisplayOrder = x.DisplayOrder }).ToList(),
            FixedDepartures = m.FixedDepartures.Select(x => new ExpeditionFixedDepartureFormRow { StartDate = x.StartDate, EndDate = x.EndDate, TotalSeats = x.TotalSeats, BookedSeats = x.BookedSeats, Price = x.Price, Currency = x.Currency, Status = x.Status, IsGuaranteed = x.IsGuaranteed }).ToList(),
            Gears = m.Gears.Select(x => new ExpeditionGearFormRow { Category = x.Category, ItemName = x.ItemName, IsMandatory = x.IsMandatory, DisplayOrder = x.DisplayOrder }).ToList(),
            Reviews = m.Reviews.Select(x => new ExpeditionReviewFormRow { ClientName = x.ClientName, Country = x.Country, Rating = x.Rating, Title = x.Title, Comment = x.Comment, ImagePath = x.ImagePath, IsApproved = x.IsApproved }).ToList(),
            Faqs = m.Faqs.Select(x => new ExpeditionFaqFormRow { Question = x.Question, Answer = x.Answer, DisplayOrder = x.DisplayOrder }).ToList()
        };
}
