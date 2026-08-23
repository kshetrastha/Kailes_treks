using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/bookings")]
public sealed class PackageBookingsController(IUnitOfWork uow) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search,
        string? status,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        PackageBookingStatus? parsedStatus = Enum.TryParse<PackageBookingStatus>(status, true, out var s) ? s : null;

        var result = await uow.PackageBookingService.ListAsync(
            new PackageBookingFilter(search, parsedStatus, page, pageSize), ct);

        ViewBag.Search = search;
        ViewBag.Status = parsedStatus?.ToString();

        return View(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detail(int id, CancellationToken ct)
    {
        var booking = await uow.PackageBookingService.GetDetailAsync(id, ct);
        if (booking is null) return NotFound();

        return View(booking);
    }

    [HttpPost("{id:int}/status"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status, string? adminNotes, CancellationToken ct)
    {
        var booking = await uow.PackageBookingService.GetByIdAsync(id, ct);
        if (booking is null) return NotFound();

        if (!Enum.TryParse<PackageBookingStatus>(status, true, out var parsed))
        {
            TempData["ErrorMessage"] = "Unknown booking status.";
            return RedirectToAction(nameof(Detail), new { id });
        }

        var now = DateTime.UtcNow;

        if (booking.Status != parsed)
        {
            booking.Status = parsed;
            booking.StatusChangedAtUtc = now;
        }

        booking.AdminNotes = string.IsNullOrWhiteSpace(adminNotes) ? null : adminNotes.Trim();
        booking.UpdatedAtUtc = now;

        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = $"Booking {booking.Reference} marked as {parsed}.";
        return RedirectToAction(nameof(Detail), new { id });
    }
}
