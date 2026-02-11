using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpGet("who-we-are")]
    public async Task<IActionResult> WhoWeAre(CancellationToken ct)
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

        var whoWeAreItems = await TryGetWhoWeAreItemsAsync(ct);

        var whoWeAreHero = await TryGetWhoWeAreHeroAsync(ct);

        return new HomeIndexViewModel
        {
            WhyWithUsHeader = whyWithUsHero?.Header ?? "Because we are the best",
            WhyWithUsTitle = whyWithUsHero?.Title ?? "Why with us?",
            WhyWithUsDescription = whyWithUsHero?.Description ??
                "Amongst the crowd of new adventure companies sprouting every day, we are committed to responsible and sustainable tourism and have something of a history, culture, and experience that stands out for the technical infallibility, excellent management, and sincerity in providing services.",
            WhyWithUsBackgroundImagePath = whyWithUsHero?.BackgroundImagePath,
            WhyWithUsItems = whyWithUsItems,
            WhoWeAreHeader = whoWeAreHero?.Header ?? "Leading Expedition Operator",
            WhoWeAreTitle = whoWeAreHero?.Title ?? "Who we are?",
            WhoWeAreDescription = whoWeAreHero?.Description ?? "Seven Summit Treks is a registered Nepali trek and expedition operator specializing in Himalayan climbs and personalized adventures.",
            WhoWeAreBackgroundImagePath = whoWeAreHero?.BackgroundImagePath,
            WhoWeAreItems = whoWeAreItems
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

    private async Task<List<WhoWeAreItemViewModel>> TryGetWhoWeAreItemsAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhoWeAreService.Query()
                .AsNoTracking()
                .Where(x => x.IsPublished)
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new WhoWeAreItemViewModel
                {
                    Title = x.Title,
                    SubDescription = x.SubDescription,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    ImageCaption = x.ImageCaption,
                    Images = x.Images
                        .OrderBy(img => img.Ordering)
                        .ThenBy(img => img.Id)
                        .Select(img => new WhoWeAreImageItemViewModel
                        {
                            ImagePath = img.ImagePath,
                            Caption = img.Caption
                        })
                        .ToList()
                })
                .ToListAsync(ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return [];
        }
    }

    private async Task<WhoWeAreHero?> TryGetWhoWeAreHeroAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhoWeAreHeroService.GetFirstAsync(asNoTracking: true, ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return null;
        }
    }

}
