using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Travel;

namespace TravelCleanArch.Web.Controllers.Public;

[ApiController]
[Route("api/trekking")]
[ApiExplorerSettings(GroupName = "customer")]
public sealed class TrekkingPublicController(ITrekkingService service) : ControllerBase
{
    [HttpGet("{slug}")]
    public async Task<IActionResult> BySlug([FromRoute] string slug, CancellationToken ct)
    {
        var item = await service.GetPublicBySlugAsync(slug, ct);
        return item is null ? NotFound() : Ok(item);
    }
}
