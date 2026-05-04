using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/newsletter-subscriptions")]
public sealed class NewsletterSubscriptionsController : Controller
{
    private readonly AppDbContext _dbContext;

    public NewsletterSubscriptionsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var items = await _dbContext.NewsletterSubscriptions
            .OrderByDescending(x => x.SubscribedAtUtc)
            .ToListAsync();
        return View(items);
    }
}
