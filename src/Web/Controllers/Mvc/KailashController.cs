using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;

namespace TravelCleanArch.Web.Controllers.Mvc;

[Route("kailash-booking")]
public sealed class KailashController(IUnitOfWork uow) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Booking(CancellationToken ct)
    
    {
        var packages = await uow.KailashService.GetActivePackagesAsync(ct);
        return View(packages);
    }

    [HttpPost("")]
    public async Task<IActionResult> Booking(
        [FromForm] KailashBookingCreateDto dto,
        [FromForm(Name = "HealthDeclaration")] string? healthJson,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var packages = await uow.KailashService.GetActivePackagesAsync(ct);
            return View(packages);
        }

        if (!string.IsNullOrWhiteSpace(healthJson))
        {
            try { dto.HealthDeclaration = JsonSerializer.Deserialize<Dictionary<string, bool>>(healthJson) ?? []; }
            catch { /* ignore malformed JSON */ }
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var bookingId = await uow.KailashService.CreateBookingAsync(dto, ip, ct);

        TempData["BookingSuccess"] = bookingId;
        return RedirectToAction(nameof(BookingConfirmation), new { id = bookingId });
    }

    [HttpGet("confirmation/{id:int}")]
    public IActionResult BookingConfirmation(int id)
    {
        ViewBag.BookingId = id;
        return View();
    }
}
