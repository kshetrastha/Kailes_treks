using Microsoft.AspNetCore.Mvc;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Booking([FromForm] KailashBookingCreateDto dto, CancellationToken ct)
    {
        // Remove health declaration binding error (it's sent as raw JSON, not as a Dictionary)
        ModelState.Remove(nameof(dto.HealthDeclarationJson));

        if (dto.PackageId <= 0)
            ModelState.AddModelError("PackageId", "Please select a Yatra Package and Month.");

        if (!ModelState.IsValid)
        {
            var packages = await uow.KailashService.GetActivePackagesAsync(ct);
            return View(packages);
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var bookingId = await uow.KailashService.CreateBookingAsync(dto, ip, ct);

        return RedirectToAction(nameof(BookingConfirmation), new { id = bookingId });
    }

    [HttpGet("confirmation/{id:int}")]
    public IActionResult BookingConfirmation(int id)
    {
        ViewBag.BookingId = id;
        return View();
    }
}
