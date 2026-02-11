using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Models.Home;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController(IUnitOfWork uow) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    [HttpGet("why-with-us")]
    public async Task<IActionResult> WhyWithUs(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    private async Task<HomeIndexViewModel> BuildHomeIndexViewModelAsync(CancellationToken ct)
    {
        var whyWithUsItems = (await uow.WhyWithUsService.ListOrderedAsync(publishedOnly: true, ct))
            .Select(x => new WhyWithUsItemViewModel
            {
                Title = x.Title,
                Description = x.Description,
                IconCssClass = x.IconCssClass
            })
            .ToList();

        var whyWithUsHero = await TryGetWhyWithUsHeroAsync(ct);

        return new HomeIndexViewModel
        {
            WhyWithUsHeader = whyWithUsHero?.Header ?? "Because we are the best",
            WhyWithUsTitle = whyWithUsHero?.Title ?? "Why with us?",
            WhyWithUsDescription = whyWithUsHero?.Description ??
                "Amongst the crowd of new adventure companies sprouting every day, we are committed to responsible and sustainable tourism and have something of a history, culture, and experience that stands out for the technical infallibility, excellent management, and sincerity in providing services.",
            WhyWithUsBackgroundImagePath = whyWithUsHero?.BackgroundImagePath,
            WhyWithUsItems = whyWithUsItems
        };
    }

    private async Task<WhyWithUsHero?> TryGetWhyWithUsHeroAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhyWithUsHeroService.GetFirstAsync(asNoTracking: true, ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return null;
        }
    }
}
