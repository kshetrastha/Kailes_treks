using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/kailash")]
public sealed class KailashController(IUnitOfWork uow) : Controller
{
    // ── Booking list ─────────────────────────────────────────────────────────

    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? status, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var result = await uow.KailashService.ListBookingsAsync(search, status, page, pageSize, ct);
        ViewBag.Search = search;
        ViewBag.Status = status;
        return View(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BookingDetail(int id, CancellationToken ct)
    {
        var booking = await uow.KailashService.GetBookingByIdAsync(id, ct);
        return booking is null ? NotFound() : View(booking);
    }

    [HttpPost("{id:int}/status")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, [FromForm] string status, CancellationToken ct)
    {
        await uow.KailashService.UpdateBookingStatusAsync(id, status, ct);
        TempData["Success"] = "Booking status updated.";
        return RedirectToAction(nameof(BookingDetail), new { id });
    }

    // ── Package management ───────────────────────────────────────────────────

    [HttpGet("packages")]
    public async Task<IActionResult> Packages(CancellationToken ct)
    {
        var packages = await uow.KailashService.GetAllPackagesAsync(ct);
        return View(packages);
    }

    [HttpPost("packages/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePackage([FromForm] KailashYatraPackageUpsertDto dto, CancellationToken ct)
    {
        dto.AvailableMonths = ParseMonths(Request.Form["AvailableMonths"].ToList());
        dto.FullMoonMonths  = ParseMonths(Request.Form["FullMoonMonths"].ToList());
        await uow.KailashService.CreatePackageAsync(dto, ct);
        TempData["Success"] = "Package created.";
        return RedirectToAction(nameof(Packages));
    }

    [HttpPost("packages/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPackage(int id, [FromForm] KailashYatraPackageUpsertDto dto, CancellationToken ct)
    {
        dto.AvailableMonths = ParseMonths(Request.Form["AvailableMonths"].ToList());
        dto.FullMoonMonths  = ParseMonths(Request.Form["FullMoonMonths"].ToList());
        await uow.KailashService.UpdatePackageAsync(id, dto, ct);
        TempData["Success"] = "Package updated.";
        return RedirectToAction(nameof(Packages));
    }

    [HttpPost("packages/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePackage(int id, CancellationToken ct)
    {
        await uow.KailashService.DeletePackageAsync(id, ct);
        TempData["Success"] = "Package deleted.";
        return RedirectToAction(nameof(Packages));
    }

    private static List<int> ParseMonths(List<string?> raw) =>
        raw.SelectMany(s => (s ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
           .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
           .Where(n => n is >= 1 and <= 12)
           .Distinct()
           .ToList();
}
