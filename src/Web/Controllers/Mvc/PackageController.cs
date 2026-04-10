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
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var trekkingPackage = await uow.TrekkingService.GetPublicBySlugAsync(slug.Trim(), ct);
            if (trekkingPackage is null)
                return NotFound();

            return View(trekkingPackage);
        }
    }
}
