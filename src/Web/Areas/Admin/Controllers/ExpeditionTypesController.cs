using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("admin/api/expedition-types")]
public sealed class ExpeditionTypesApiController(IExpeditionTypeService service, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] bool includeUnpublished = true, CancellationToken ct = default)
        => Ok(await service.ListAsync(includeUnpublished, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpeditionTypeUpsertDto request, CancellationToken ct)
    {
        var id = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpeditionTypeUpsertDto request, CancellationToken ct)
        => await service.UpdateAsync(id, request, currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await service.DeleteAsync(id, ct)
            ? NoContent()
            : Conflict(new { message = "Unable to delete expedition type. It may not exist or is in use by one or more expeditions." });
}
