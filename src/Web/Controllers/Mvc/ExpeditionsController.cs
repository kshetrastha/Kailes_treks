using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;

namespace TravelCleanArch.Web.Controllers.Mvc
{
    public sealed class ExpeditionsController(IUnitOfWork uow) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("expeditions/{slug}")]
        public async Task<IActionResult> ExpeditionDetails(string slug, CancellationToken ct)
        {
            var expedition = await uow.ExpeditionService.GetPublicBySlugAsync(slug, ct);
            if (expedition is null)
                return NotFound();
            return View(expedition);
        }

    }
}
