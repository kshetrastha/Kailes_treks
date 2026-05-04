using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class NewsletterController : Controller
{
    private readonly AppDbContext _dbContext;

    public NewsletterController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            TempData["NewsletterError"] = "Please provide a valid email address.";
            return Redirect(Request.Headers.Referer.ToString());
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var exists = await _dbContext.NewsletterSubscriptions.AnyAsync(x => x.Email == normalizedEmail);
        if (!exists)
        {
            _dbContext.NewsletterSubscriptions.Add(new NewsletterSubscription
            {
                Email = normalizedEmail,
                SubscribedAtUtc = DateTime.UtcNow,
                SourcePage = Request.Headers.Referer.ToString(),
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
        }

        TempData["NewsletterSuccess"] = exists
            ? "You are already subscribed to our mailing list."
            : "Thanks for subscribing to our mailing list.";

        return Redirect(Request.Headers.Referer.ToString());
    }
}
