using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Models.Package;

namespace TravelCleanArch.Web.Controllers.Mvc
{
    public class PackageController(IUnitOfWork uow, AppDbContext db) : Controller
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

        [HttpPost("packages/{slug}/reviews")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrekkingReview(string slug, TrekkingReviewFormViewModel model, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var trekkingPackage = await uow.TrekkingService.GetPublicBySlugAsync(slug.Trim(), ct);
            if (trekkingPackage is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ReviewErrorMessage"] = "Please fill all required review fields.";
                return Redirect($"{Url.Action(nameof(TrekkingDetails), new { slug = trekkingPackage.Slug })}#nav-feedback");
            }

            var review = new TrekkingReview
            {
                TrekkingId = trekkingPackage.Id,
                FullName = model.Name.Trim(),
                EmailAddress = model.Email.Trim(),
                Rating = model.Rating,
                ReviewText = model.Comment.Trim(),
                ModerationStatus = ReviewModerationStatus.Pending
            };

            db.TrekkingReviews.Add(review);
            await db.SaveChangesAsync(ct);

            TempData["ReviewSuccessMessage"] = "Thanks for your review. It has been submitted for moderation.";
            return Redirect($"{Url.Action(nameof(TrekkingDetails), new { slug = trekkingPackage.Slug })}#nav-feedback");
        }
    }
}
