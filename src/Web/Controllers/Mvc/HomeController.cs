using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Web.Models.Home;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController(IUnitOfWork uow) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    [HttpGet("why-with-us")]
    public async Task<IActionResult> WhyWithUs(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    [HttpGet("who-we-are")]
    public async Task<IActionResult> WhoWeAre(CancellationToken ct)
    {
        var model = await BuildHomeIndexViewModelAsync(ct);

        return View(model);
    }

    [HttpGet("our-blogs")]
    public async Task<IActionResult> OurBlogs(CancellationToken ct)
    {
        var posts = await TryGetBlogPostsAsync(ct);
        var featured = posts.FirstOrDefault(x => x.IsFeatured) ?? posts.FirstOrDefault();
        var model = new BlogListViewModel
        {
            FeaturedPost = featured,
            OtherPosts = posts.Where(x => x.Slug != featured?.Slug).ToList()
        };

        return View(model);
    }

    [HttpGet("our-blogs/{slug}")]
    public async Task<IActionResult> BlogDetails(string slug, CancellationToken ct)
    {
        var post = await TryGetBlogBySlugAsync(slug, ct);
        if (post is null) return NotFound();

        var latest = (await TryGetBlogPostsAsync(ct))
            .Where(x => x.Slug != slug)
            .Take(8)
            .ToList();

        var model = new BlogDetailViewModel
        {
            Title = post.Title,
            Slug = post.Slug,
            Summary = post.Summary,
            ContentHtml = post.ContentHtml,
            HeroImagePath = post.HeroImagePath ?? post.ThumbnailImagePath,
            PublishedOnUtc = post.PublishedOnUtc,
            LatestPosts = latest
        };

        return View(model);
    }

    [HttpGet("expeditions/type/{typeId:int}")]
    public async Task<IActionResult> ExpeditionsByType(
        int typeId,
        [FromServices] IExpeditionTypeService expeditionTypeService,
        [FromServices] IExpeditionService expeditionService,
        CancellationToken ct)
    {
        var expeditionType = await expeditionTypeService.GetByIdAsync(typeId, ct);
        if (expeditionType is null || !expeditionType.IsPublished)
        {
            return NotFound();
        }

        var publishedExpeditions = (await expeditionService.ListAsync(
            search: null,
            status: "published",
            destination: null,
            featured: null,
            page: 1,
            pageSize: 200,
            ct)).Items;

        var model = new ExpeditionsByTypeViewModel
        {
            TypeId = expeditionType.Id,
            TypeTitle = expeditionType.Title,
            ShortDescription = expeditionType.ShortDescription,
            TypeDescription = expeditionType.Description,
            ImagePath = expeditionType.ImagePath,
            Expeditions = publishedExpeditions
                .Where(x => x.ExpeditionTypeId == expeditionType.Id)
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Name)
                .Select(x => new ExpeditionTypeCardViewModel
                {
                    Name = x.Name,
                    Destination = x.Destination,
                    DurationDays = x.DurationDays,
                    ShortDescription = x.ShortDescription,
                    ImagePath =x.HeroImageUrl
                })
                .ToList()
        };

        return View(model);
    }

    [HttpGet("expeditions/{id:int}")]
    public async Task<IActionResult> ExpeditionDetails(int id, [FromServices] IExpeditionModuleService expeditionModuleService, CancellationToken ct)
    {
        var item = await expeditionModuleService.GetDetailsAsync(id, ct);
        if (item is null) return NotFound();

        var vm = new ExpeditionModuleDetailsViewModel
        {
            Id = item.Id,
            Name = item.Name,
            ShortDescription = item.ShortDescription,
            DifficultyLevel = item.DifficultyLevel,
            Duration = item.Duration,
            MaxElevation = item.MaxElevation,
            BestSeason = item.BestSeason,
            Accommodation = item.Accommodation,
            WalkingHoursPerDay = item.WalkingHoursPerDay,
            GroupSize = item.GroupSize,
            BannerImagePath = item.BannerImagePath,
            ThumbnailImagePath = item.ThumbnailImagePath,
            Country = item.Overview?.Country,
            PeakName = item.Overview?.PeakName,
            Route = item.Overview?.Route,
            Rank = item.Overview?.Rank,
            Range = item.Overview?.Range,
            Coordinates = item.Overview?.Coordinates,
            WeatherInformation = item.Overview?.WeatherInformation,
            FullDescription = item.Overview?.FullDescription,
            MapEmbedCode = item.Overview?.MapEmbedCode,
            ItinerariesBySeason = item.Itineraries.GroupBy(x => x.SeasonTitle).OrderBy(g => g.Key).ToDictionary(g => g.Key, g => g.OrderBy(x => x.DayNumber).Select(x => new ExpeditionItineraryRowViewModel { DayNumber = x.DayNumber, Title = x.Title, ShortDescription = x.ShortDescription, FullDescription = x.FullDescription, Accommodation = x.Accommodation, Meals = x.Meals, Elevation = x.Elevation }).ToList()),
            Inclusions = item.InclusionExclusions.Where(x => x.Type == "Inclusion").OrderBy(x => x.DisplayOrder).Select(x => x.Description).ToList(),
            Exclusions = item.InclusionExclusions.Where(x => x.Type == "Exclusion").OrderBy(x => x.DisplayOrder).Select(x => x.Description).ToList(),
            FixedDepartures = item.FixedDepartures.OrderBy(x => x.StartDate).Select(x => new ExpeditionDepartureRowViewModel { StartDate = x.StartDate, EndDate = x.EndDate, TotalSeats = x.TotalSeats, BookedSeats = x.BookedSeats, RemainingSeats = x.TotalSeats - x.BookedSeats, Price = x.Price, Currency = x.Currency, Status = x.Status, IsGuaranteed = x.IsGuaranteed }).ToList(),
            GearByCategory = item.Gears.GroupBy(x => x.Category).OrderBy(g => g.Key).ToDictionary(g => g.Key, g => g.OrderBy(x => x.DisplayOrder).Select(x => new ExpeditionGearRowViewModel { ItemName = x.ItemName, IsMandatory = x.IsMandatory }).ToList()),
            Reviews = item.Reviews.Where(x => x.IsApproved).Select(x => new ExpeditionReviewRowViewModel { ClientName = x.ClientName, Country = x.Country, Rating = x.Rating, Title = x.Title, Comment = x.Comment, ImagePath = x.ImagePath }).ToList(),
            Faqs = item.Faqs.OrderBy(x => x.DisplayOrder).Select(x => new ExpeditionFaqRowViewModel { Question = x.Question, Answer = x.Answer }).ToList()
        };

        return View(vm);
    }

    private async Task<HomeIndexViewModel> BuildHomeIndexViewModelAsync(CancellationToken ct)
    {
        var whyWithUsItems = (await uow.WhyWithUsService.ListOrderedAsync(publishedOnly: true, ct))
            .Select(x => new WhyWithUsItemViewModel
            {
                Title = x.Title,
                Description = x.Description,
                IconCssClass = x.IconCssClass
            })
            .ToList();

        var whyWithUsHero = await TryGetWhyWithUsHeroAsync(ct);

        var whoWeAreItems = await TryGetWhoWeAreItemsAsync(ct);

        var whoWeAreHero = await TryGetWhoWeAreHeroAsync(ct);

        var recentBlogs = (await TryGetBlogPostsAsync(ct)).Take(2).ToList();

        return new HomeIndexViewModel
        {
            WhyWithUsHeader = whyWithUsHero?.Header ?? "Because we are the best",
            WhyWithUsTitle = whyWithUsHero?.Title ?? "Why with us?",
            WhyWithUsDescription = whyWithUsHero?.Description ??
                "Amongst the crowd of new adventure companies sprouting every day, we are committed to responsible and sustainable tourism and have something of a history, culture, and experience that stands out for the technical infallibility, excellent management, and sincerity in providing services.",
            WhyWithUsBackgroundImagePath = whyWithUsHero?.BackgroundImagePath,
            WhyWithUsItems = whyWithUsItems,
            WhoWeAreHeader = whoWeAreHero?.Header ?? "Leading Expedition Operator",
            WhoWeAreTitle = whoWeAreHero?.Title ?? "Who we are?",
            WhoWeAreDescription = whoWeAreHero?.Description ?? "Seven Summit Treks is a registered Nepali trek and expedition operator specializing in Himalayan climbs and personalized adventures.",
            WhoWeAreBackgroundImagePath = whoWeAreHero?.BackgroundImagePath,
            WhoWeAreItems = whoWeAreItems,
            RecentBlogs = recentBlogs
        };
    }

    private async Task<WhyWithUsHero?> TryGetWhyWithUsHeroAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhyWithUsHeroService.GetFirstAsync(asNoTracking: true, ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return null;
        }
    }

    private async Task<List<WhoWeAreItemViewModel>> TryGetWhoWeAreItemsAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhoWeAreService.Query()
                .AsNoTracking()
                .Where(x => x.IsPublished)
                .OrderBy(x => x.Ordering)
                .ThenBy(x => x.Id)
                .Select(x => new WhoWeAreItemViewModel
                {
                    Title = x.Title,
                    SubDescription = x.SubDescription,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    ImageCaption = x.ImageCaption,
                    Images = x.Images
                        .OrderBy(img => img.Ordering)
                        .ThenBy(img => img.Id)
                        .Select(img => new WhoWeAreImageItemViewModel
                        {
                            ImagePath = img.ImagePath,
                            Caption = img.Caption
                        })
                        .ToList()
                })
                .ToListAsync(ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return [];
        }
    }

    private async Task<WhoWeAreHero?> TryGetWhoWeAreHeroAsync(CancellationToken ct)
    {
        try
        {
            return await uow.WhoWeAreHeroService.GetFirstAsync(asNoTracking: true, ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return null;
        }
    }

    private async Task<List<BlogCardViewModel>> TryGetBlogPostsAsync(CancellationToken ct)
    {
        try
        {
            return (await uow.BlogPostService.ListOrderedAsync(true, ct))
                .Select(x => new BlogCardViewModel
                {
                    Title = x.Title,
                    Slug = x.Slug,
                    Summary = x.Summary,
                    ThumbnailImagePath = x.ThumbnailImagePath ?? x.HeroImagePath,
                    PublishedOnUtc = x.PublishedOnUtc,
                    IsFeatured = x.IsFeatured
                })
                .ToList();
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return [];
        }
    }

    private async Task<BlogPost?> TryGetBlogBySlugAsync(string slug, CancellationToken ct)
    {
        try
        {
            return await uow.BlogPostService.GetBySlugAsync(slug, true, ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            return null;
        }
    }
}
