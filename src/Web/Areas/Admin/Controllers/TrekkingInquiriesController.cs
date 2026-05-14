using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Infrastructure.Persistence;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/trekking-inquiries")]
public sealed class TrekkingInquiriesController : Controller
{
    private readonly AppDbContext _dbContext;

    public TrekkingInquiriesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await _dbContext.TrekkingInquiries
            .AsNoTracking()
            .Include(x => x.Trekking)
            .OrderByDescending(x => x.SubmittedAtUtc)
            .ToListAsync(ct);

        return View(items);
    }
}
