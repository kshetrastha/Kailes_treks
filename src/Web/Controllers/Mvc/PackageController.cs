using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Models.Package;

namespace TravelCleanArch.Web.Controllers.Mvc
{
    public class PackageController(IUnitOfWork uow, AppDbContext db, IWebHostEnvironment environment) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? destination,
            string? tourType,
            int page = 1,
            bool partial = false,
            CancellationToken ct = default)
        {
            const int pageSize = 12;

            page = page < 1 ? 1 : page;
            var selectedLocation = destination?.ToString();
            var selectedTourTypeId = int.TryParse(tourType, out var parsedTourTypeId) ? parsedTourTypeId : (int?)null;

            var result = await uow.TrekkingService.ListAsync(
                search,
                "published",
                selectedLocation,
                selectedTourTypeId?.ToString(),
                null,
                page,
                pageSize,
                ct);

            ViewBag.Search = search;
            ViewBag.Location = destination;
            ViewBag.TourType = tourType;

            if (partial)
            {
                return PartialView("_PackageListingResults", result);
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTourTypesByDestination(string? destination, CancellationToken ct = default)
        {
            var query = db.Trekking
                .AsNoTracking()
                .Where(x => x.Status == TravelStatus.published);

            if (!string.IsNullOrWhiteSpace(destination))
            {
                query = query.Where(x => x.Destination == destination);
            }

            var tourTypes = await query
                .Where(x => x.TrekkingTypeId.HasValue && x.TrekkingType != null)
                .Select(x => new { Id = x.TrekkingTypeId!.Value, Title = x.TrekkingType!.Title })
                .Distinct()
                .OrderBy(x => x.Title)
                .ToListAsync(ct);

            return Json(tourTypes);
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
                UserPhotoPath = "/" + await SaveProfileImageAsync(model.ProfileImage, ct),
                Rating = model.Rating,
                ReviewText = model.Comment.Trim(),
                ModerationStatus = ReviewModerationStatus.Pending
            };

            db.TrekkingReviews.Add(review);
            await db.SaveChangesAsync(ct);

            TempData["ReviewSuccessMessage"] = "Thanks for your review. It has been submitted for moderation.";
            return Redirect($"{Url.Action(nameof(TrekkingDetails), new { slug = trekkingPackage.Slug })}#nav-feedback");
        }

        private async Task<string?> SaveProfileImageAsync(IFormFile? image, CancellationToken ct)
        {
            if (image is null || image.Length == 0)
            {
                return null;
            }

            var extension = Path.GetExtension(image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "trekking", "reviews");
            Directory.CreateDirectory(uploadsDirectory);

            var filePath = Path.Combine(uploadsDirectory, fileName);
            await using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream, ct);
            return Path.Combine("uploads", "trekking", "reviews", fileName).Replace('\\', '/');
        }
    }
}
