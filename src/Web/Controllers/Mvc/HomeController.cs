using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Models.Home;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController(AppDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
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

        var model = new HomeIndexViewModel
        {
            WhyWithUsItems = whyWithUsItems
        };

        return View(model);
    }
}
