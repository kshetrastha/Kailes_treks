using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Travel;

namespace TravelCleanArch.API.Controllers.Public;

[ApiController]
[Route("api/expeditions")]
[ApiExplorerSettings(GroupName = "customer")]
public sealed class ExpeditionsController(IExpeditionService service) : ControllerBase
{
    [HttpGet("{slug}")]
    public async Task<IActionResult> BySlug([FromRoute] string slug, CancellationToken ct)
    {
        var item = await service.GetPublicBySlugAsync(slug, ct);
        return item is null ? NotFound() : Ok(item);
    }
}
