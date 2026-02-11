using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Models.Home;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController(AppDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    [HttpGet("why-with-us.html")]
    public async Task<IActionResult> WhyWithUs(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    private async Task<HomeIndexViewModel> BuildHomeIndexViewModelAsync(CancellationToken ct)
    {
        var whyWithUsItems = await dbContext.WhyWithUs
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.Ordering)
            .ThenBy(x => x.Id)
            .Select(x => new WhyWithUsItemViewModel
            {
                Title = x.Title,
                Description = x.Description,
                IconCssClass = x.IconCssClass
            })
            .ToListAsync(ct);

        var whyWithUsHero = await dbContext.WhyWithUsHeroes
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(ct);

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
}
