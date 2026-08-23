using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Application.Common;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Models.Package;

namespace TravelCleanArch.Web.Controllers.Mvc
{
    public class PackageController(
        IUnitOfWork uow,
        AppDbContext db,
        IWebHostEnvironment environment,
        IIpGeolocationService ipGeolocation) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            DateTime? dates,
            string? destination,
            string? tourType,
            int page = 1,
            bool partial = false,
            CancellationToken ct = default)
        {
            const int pageSize = 12;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var parts = search.Split(" to ", StringSplitOptions.TrimEntries);

                if (parts.Length == 2)
                {
                    var year = DateTime.Now.Year;

                    fromDate = DateTime.ParseExact(
                        $"{parts[0]}-{year}",
                        "MM-dd-yyyy",
                        null);

                    toDate = DateTime.ParseExact(
                        $"{parts[1]}-{year}",
                        "MM-dd-yyyy",
                        null);
                }
            }


            page = page < 1 ? 1 : page;
            var selectedLocation = destination?.ToString();

            var result = await uow.TrekkingService.ListAsync(
                search,
                "published",
                selectedLocation,
                tourType,
                null,
                page,
                pageSize,
                ct);

            //if (!string.IsNullOrWhiteSpace(tourType))
            //{
            //    var filteredItems = result.Items
            //        .Where(x => string.Equals(x.TrekkingTypeTitle, tourType, StringComparison.OrdinalIgnoreCase))
            //        .ToList();

            //    result = new TrekkingPagedResult(
            //        filteredItems,
            //        result.Page,
            //        result.PageSize,
            //        filteredItems.Count,
            //        result.Locations,
            //        result.TrekkingTypes
            //    );
            //}

            ViewBag.Search = search;
            ViewBag.Location = destination;
            ViewBag.TourType = tourType;
            if (partial)
                return PartialView("_PackageListingResults", result);
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetTourTypesByDestination(string destination, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return Json(new List<SelectOptionDto>());
            var tourTypes = await uow.TrekkingService
                .GetOptionsByDestinationAsync(destination);
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

            ViewBag.RelatedTours = await uow.TrekkingService.GetRelatedPublicToursAsync(
                trekkingPackage.Id,
                trekkingPackage.TrekkingTypeId,
                trekkingPackage.Destination,
                2,
                ct);
            ViewBag.RecentTours = await uow.TrekkingService.GetRecentPublicToursAsync(trekkingPackage.Id, 3, ct);

            return View(trekkingPackage);
        }

        [HttpPost("packages/{slug}/inquiries")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrekkingInquiry(string slug, TrekkingInquiryFormViewModel model, CancellationToken ct)
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
                TempData["InquiryErrorMessage"] = "Please fill all required inquiry fields.";
                return Redirect($"{Url.Action(nameof(TrekkingDetails), new { slug = trekkingPackage.Slug })}#trekking-inquiry");
            }

            var now = DateTime.UtcNow;
            var inquiry = new TrekkingInquiry
            {
                TrekkingId = trekkingPackage.Id,
                FullName = model.Name.Trim(),
                EmailAddress = model.Email.Trim(),
                Comment = model.Comment.Trim(),
                SubmittedAtUtc = now,
                SourcePage = HttpContext.Request.Path.Value,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            db.TrekkingInquiries.Add(inquiry);
            await db.SaveChangesAsync(ct);

            TempData["InquirySuccessMessage"] = "Thanks for your inquiry. Our team will contact you soon.";
            return Redirect($"{Url.Action(nameof(TrekkingDetails), new { slug = trekkingPackage.Slug })}#trekking-inquiry");
        }

        [HttpGet("packages/{slug}/book")]
        public async Task<IActionResult> Book(string slug, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();

            var trekkingPackage = await uow.TrekkingService.GetPublicBySlugAsync(slug.Trim(), ct);
            if (trekkingPackage is null) return NotFound();

            return View(new PackageBookingFormViewModel { Package = BuildBookingSummary(trekkingPackage) });
        }

        [HttpPost("packages/{slug}/book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(string slug, PackageBookingFormViewModel model, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();

            var trekkingPackage = await uow.TrekkingService.GetPublicBySlugAsync(slug.Trim(), ct);
            if (trekkingPackage is null) return NotFound();

            // The summary is display-only, so always rebuild it from the package rather than the post.
            model.Package = BuildBookingSummary(trekkingPackage);
            ModelState.Remove("Package");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var now = DateTime.UtcNow;
            var request = HttpContext.Request;
            var ipAddress = ResolveClientIpAddress();
            var geo = await ipGeolocation.LookupAsync(ipAddress, ct);

            var totalAmount = trekkingPackage.PriceOnRequest || trekkingPackage.Price is null
                ? (decimal?)null
                : trekkingPackage.Price.Value * model.NumberOfTravellers;

            var booking = new PackageBooking
            {
                Reference = await uow.PackageBookingService.GenerateReferenceAsync(now, ct),

                TrekkingId = trekkingPackage.Id,
                PackageName = trekkingPackage.Name,
                PackageSlug = trekkingPackage.Slug,
                PackageDestination = trekkingPackage.Destination,
                PackageRegion = trekkingPackage.Region,
                PackageTrekkingType = trekkingPackage.TrekkingTypeTitle,
                PackageDifficulty = trekkingPackage.Difficulty,
                PackageDurationDays = trekkingPackage.DurationDays,
                PackageMaxAltitudeMeters = trekkingPackage.MaxAltitudeMeters,
                PriceOnRequest = trekkingPackage.PriceOnRequest,
                PricePerPerson = trekkingPackage.Price,
                CurrencyCode = trekkingPackage.CurrencyCode,
                TotalAmount = totalAmount,

                PreferredStartDate = model.PreferredStartDate,
                FixedDepartureId = model.FixedDepartureId,
                NumberOfTravellers = model.NumberOfTravellers,

                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.Trim(),
                Phone = model.Phone.Trim(),
                AlternatePhone = model.AlternatePhone?.Trim(),
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender?.Trim(),
                Nationality = model.Nationality?.Trim(),
                PassportNumber = model.PassportNumber?.Trim(),
                Address = model.Address?.Trim(),
                City = model.City?.Trim(),
                PostalCode = model.PostalCode?.Trim(),
                Country = model.Country?.Trim(),
                EmergencyContactName = model.EmergencyContactName?.Trim(),
                EmergencyContactPhone = model.EmergencyContactPhone?.Trim(),
                SpecialRequests = model.SpecialRequests?.Trim(),

                IpAddress = ipAddress,
                IpCountry = geo?.Country,
                IpCountryCode = geo?.CountryCode,
                IpRegion = geo?.Region,
                IpCity = geo?.City,
                IpPostalCode = geo?.PostalCode,
                IpTimeZone = geo?.TimeZone,
                IpOrganisation = geo?.Organisation,
                IpLatitude = geo?.Latitude,
                IpLongitude = geo?.Longitude,
                UserAgent = Truncate(request.Headers.UserAgent.ToString(), 500),
                BrowserLanguage = Truncate(request.Headers.AcceptLanguage.ToString(), 200),
                Referrer = Truncate(request.Headers.Referer.ToString(), 500),
                SourcePage = request.Path.Value,

                Status = PackageBookingStatus.Pending,
                SubmittedAtUtc = now,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            await uow.PackageBookingService.AddAsync(booking, ct);
            await uow.SaveChangesAsync(ct);

            return RedirectToAction(nameof(BookingConfirmation), new { reference = booking.Reference });
        }

        [HttpGet("packages/booking/confirmation/{reference}")]
        public async Task<IActionResult> BookingConfirmation(string reference, CancellationToken ct)
        {
            var booking = await uow.PackageBookingService.GetByReferenceAsync(reference, ct);
            if (booking is null) return NotFound();

            return View(new PackageBookingConfirmationViewModel
            {
                Reference = booking.Reference,
                PackageName = booking.PackageName,
                PackageSlug = booking.PackageSlug,
                FullName = booking.FullName,
                Email = booking.Email,
                NumberOfTravellers = booking.NumberOfTravellers,
                PreferredStartDate = booking.PreferredStartDate,
                TotalAmount = booking.TotalAmount,
                CurrencyCode = booking.CurrencyCode,
                PriceOnRequest = booking.PriceOnRequest
            });
        }

        private static PackageBookingSummaryViewModel BuildBookingSummary(TrekkingDetailsDto p)
            => new()
            {
                TrekkingId = p.Id,
                Slug = p.Slug,
                Name = p.Name,
                Destination = p.Destination,
                Region = p.Region,
                TrekkingType = p.TrekkingTypeTitle,
                Difficulty = p.Difficulty,
                DurationDays = p.DurationDays,
                MaxAltitudeMeters = p.MaxAltitudeMeters,
                BestSeason = p.BestSeason,
                PriceOnRequest = p.PriceOnRequest,
                Price = p.Price,
                CurrencyCode = string.IsNullOrWhiteSpace(p.CurrencyCode) ? "USD" : p.CurrencyCode,
                HeroImageUrl = p.HeroImageUrl,
                MinGroupSize = p.MinGroupSize,
                MaxGroupSize = p.MaxGroupSize,
                FixedDepartures = p.FixedDepartures
                    .Where(d => d.StartDate.Date >= DateTime.UtcNow.Date)
                    .OrderBy(d => d.StartDate)
                    .ToList()
            };

        /// <summary>Prefers the left-most X-Forwarded-For entry so proxied deployments log the real visitor.</summary>
        private string? ResolveClientIpAddress()
        {
            var forwarded = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrWhiteSpace(forwarded))
            {
                var first = forwarded
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(first)) return first;
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        private static string? Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return value.Length <= maxLength ? value : value[..maxLength];
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
