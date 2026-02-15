using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("admin/api/expedition-modules")]
public sealed class ExpeditionModulesApiController(IExpeditionModuleService service, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct) => Ok(await service.ListBasicInfosAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
        => (await service.GetDetailsAsync(id, ct)) is { } m ? Ok(m) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpeditionBasicInfoUpsertDto request, CancellationToken ct)
    {
        var id = await service.CreateBasicInfoAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(Details), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpeditionBasicInfoUpsertDto request, CancellationToken ct)
        => await service.UpdateBasicInfoAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await service.DeleteBasicInfoAsync(id, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/overview")]
    public async Task<IActionResult> UpsertOverview(int id, [FromBody] ExpeditionOverviewUpsertDto request, CancellationToken ct)
        => await service.UpsertOverviewAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/itineraries")]
    public async Task<IActionResult> ReplaceItineraries(int id, [FromBody] IReadOnlyCollection<ExpeditionItineraryUpsertDto> request, CancellationToken ct)
        => await service.ReplaceItinerariesAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/inclusion-exclusions")]
    public async Task<IActionResult> ReplaceInclusionExclusions(int id, [FromBody] IReadOnlyCollection<ExpeditionInclusionExclusionUpsertDto> request, CancellationToken ct)
        => await service.ReplaceInclusionExclusionsAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/fixed-departures")]
    public async Task<IActionResult> ReplaceFixedDepartures(int id, [FromBody] IReadOnlyCollection<ExpeditionFixedDepartureUpsertDto> request, CancellationToken ct)
        => await service.ReplaceFixedDeparturesAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/gear")]
    public async Task<IActionResult> ReplaceGear(int id, [FromBody] IReadOnlyCollection<ExpeditionGearUpsertDto> request, CancellationToken ct)
        => await service.ReplaceGearsAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/reviews")]
    public async Task<IActionResult> ReplaceReviews(int id, [FromBody] IReadOnlyCollection<ExpeditionReviewUpsertDto> request, CancellationToken ct)
        => await service.ReplaceReviewsAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpPut("{id:int}/faqs")]
    public async Task<IActionResult> ReplaceFaqs(int id, [FromBody] IReadOnlyCollection<ExpeditionFaqUpsertDto> request, CancellationToken ct)
        => await service.ReplaceFaqsAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();
}
