using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Web.Contracts.Travel;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.Web.Controllers.Admin;

[ApiController]
[Route("api/admin/expeditions")]
[ApiExplorerSettings(GroupName = "admin")]
[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Editor)]
public sealed class ExpeditionController(IExpeditionService service, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public Task<ExpeditionPagedResult> List([FromQuery] string? search, [FromQuery] string? status, [FromQuery] string? destination, [FromQuery] bool? featured, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => service.ListAsync(search, status, destination, featured, page, pageSize, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpeditionUpsertRequest request, CancellationToken ct)
    {
        var id = await service.CreateAsync(Map(request), currentUser.UserId, ct);
        return CreatedAtAction(nameof(List), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ExpeditionUpsertRequest request, CancellationToken ct)
        => await service.UpdateAsync(id, Map(request), currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();

    private static ExpeditionUpsertDto Map(ExpeditionUpsertRequest request) => new(
        request.Name, request.Slug, request.Destination, request.Region, request.DurationDays, request.MaxAltitudeMeters,
        request.Difficulty, request.BestSeason, request.Overview, request.Inclusions, request.Exclusions,
        request.HeroImageUrl, request.Permits, request.MinGroupSize, request.MaxGroupSize, request.Price,
        request.AvailableDates, request.BookingCtaUrl, request.SeoTitle, request.SeoDescription, request.NormalizedStatus,
        request.Featured, request.Ordering, request.SummitRoute, request.RequiresClimbingPermit,
        request.ExpeditionStyle, request.OxygenSupport, request.SherpaSupport, request.SummitBonusUsd,
        (request.ItineraryDays ?? []).Select(i => new ExpeditionItineraryDayDto(i.DayNumber, i.Title, i.Description, i.OvernightLocation)).ToArray(),
        (request.Faqs ?? []).Select(f => new ExpeditionFaqDto(f.Question, f.Answer, f.Ordering)).ToArray(),
        (request.MediaItems ?? []).Select(m => new ExpeditionMediaDto(m.Url, m.Caption, m.MediaType, m.Ordering)).ToArray());
}
