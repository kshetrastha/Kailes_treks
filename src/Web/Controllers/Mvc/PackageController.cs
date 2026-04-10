using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;

namespace TravelCleanArch.Web.Controllers.Mvc
{
    public class PackageController(IUnitOfWork uow) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("packages/{slug}")]
        public async Task<IActionResult> TrekkingDetails(string slug, CancellationToken ct)
        {
            var trekkingPacakges = await uow.TrekkingService.GetPublicBySlugAsync(slug, ct);
            if (trekkingPacakges is null)
                return NotFound();
            return View(trekkingPacakges);


        }
    }
}
