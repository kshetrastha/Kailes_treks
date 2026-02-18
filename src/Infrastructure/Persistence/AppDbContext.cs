using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Domain.Entities.Expeditions;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Infrastructure.Identity;

namespace TravelCleanArch.Infrastructure.Persistence;

public sealed class AppDbContext:
    IdentityDbContext<AppUser, AppRole, int>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Expedition> Expeditions => Set<Expedition>();
    public DbSet<ExpeditionType> ExpeditionTypes => Set<ExpeditionType>();
    public DbSet<TrekkingType> TrekkingTypes => Set<TrekkingType>();

    public DbSet<ExpeditionFaq> ExpeditionFaqs => Set<ExpeditionFaq>();
    public DbSet<ExpeditionMedia> ExpeditionMedia => Set<ExpeditionMedia>();
    public DbSet<ExpeditionTypeImage> ExpeditionTypeImages => Set<ExpeditionTypeImage>();
    public DbSet<TrekkingTypeImage> TrekkingTypeImages => Set<TrekkingTypeImage>();
    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<ItineraryDay> ItineraryDays => Set<ItineraryDay>();
    public DbSet<ExpeditionMap> ExpeditionMaps => Set<ExpeditionMap>();
    public DbSet<CostItem> CostItems => Set<CostItem>();
    public DbSet<FixedDeparture> FixedDepartures => Set<FixedDeparture>();
    public DbSet<GearList> GearLists => Set<GearList>();
    public DbSet<ExpeditionHighlight> ExpeditionHighlights => Set<ExpeditionHighlight>();
    public DbSet<ExpeditionReview> ExpeditionReviews => Set<ExpeditionReview>();
    public DbSet<ExpeditionBasicInfo> ExpeditionBasicInfos => Set<ExpeditionBasicInfo>();
    public DbSet<ExpeditionOverview> ExpeditionOverviews => Set<ExpeditionOverview>();
    public DbSet<ExpeditionItinerary> ExpeditionItineraries => Set<ExpeditionItinerary>();
    public DbSet<ExpeditionInclusionExclusion> ExpeditionInclusionExclusions => Set<ExpeditionInclusionExclusion>();
    public DbSet<ExpeditionFixedDeparture> ExpeditionFixedDepartures => Set<ExpeditionFixedDeparture>();
    public DbSet<ExpeditionGear> ExpeditionGears => Set<ExpeditionGear>();
    public DbSet<ExpeditionReviewItem> ExpeditionReviewItems => Set<ExpeditionReviewItem>();
    public DbSet<ExpeditionFaqItem> ExpeditionFaqItems => Set<ExpeditionFaqItem>();

    public DbSet<Trekking> Trekking => Set<Trekking>();
    public DbSet<TrekkingItinerary> TrekkingItineraries => Set<TrekkingItinerary>();
    public DbSet<TrekkingItineraryDay> TrekkingItineraryDays => Set<TrekkingItineraryDay>();
    public DbSet<TrekkingFaq> TrekkingFaqs => Set<TrekkingFaq>();
    public DbSet<TrekkingMedia> TrekkingMedia => Set<TrekkingMedia>();
    public DbSet<TrekkingMap> TrekkingMaps => Set<TrekkingMap>();
    public DbSet<TrekkingCostItem> TrekkingCostItems => Set<TrekkingCostItem>();
    public DbSet<TrekkingFixedDeparture> TrekkingFixedDepartures => Set<TrekkingFixedDeparture>();
    public DbSet<TrekkingGearList> TrekkingGearLists => Set<TrekkingGearList>();
    public DbSet<TrekkingHighlight> TrekkingHighlights => Set<TrekkingHighlight>();
    public DbSet<TrekkingReview> TrekkingReviews => Set<TrekkingReview>();
    public DbSet<WhyWithUs> WhyWithUs => Set<WhyWithUs>();
    public DbSet<WhyWithUsHero> WhyWithUsHeroes => Set<WhyWithUsHero>();
    public DbSet<WhoWeAre> WhoWeAre => Set<WhoWeAre>();
    public DbSet<WhoWeAreImage> WhoWeAreImages => Set<WhoWeAreImage>();
    public DbSet<WhoWeAreHero> WhoWeAreHeroes => Set<WhoWeAreHero>();
    public DbSet<Award> Awards => Set<Award>();
    public DbSet<Patron> Patrons => Set<Patron>();
    public DbSet<ChairmanMessage> ChairmanMessages => Set<ChairmanMessage>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<CertificateDocument> CertificateDocuments => Set<CertificateDocument>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(b =>
        {
            b.Property(x => x.FullName).HasMaxLength(200);
        });

        //ConfigureExpeditions(builder);
        //ConfigureTrekking(builder);
        //ConfigureWhyWithUs(builder);
        //ConfigureWhoWeAre(builder);
        //ConfigureCompanyPages(builder);
    }

    private static void ConfigureExpeditions(ModelBuilder builder)
    {
        builder.Entity<Expedition>(b =>
        {
            b.ToTable("expeditions");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(600).IsRequired();
            b.Property(x => x.Destination).HasMaxLength(200).IsRequired();
            b.Property(x => x.Difficulty).HasMaxLength(100).IsRequired();
            b.Property(x => x.Status).HasMaxLength(32).IsRequired();
            b.Property(x => x.Price).HasColumnType("numeric(12,2)");
            b.Property(x => x.SummitBonusUsd).HasColumnType("numeric(12,2)");
            b.Property(x => x.ExpeditionStyle).HasMaxLength(120);
            b.Property(x => x.OverviewCountry).HasMaxLength(150);
            b.Property(x => x.PeakName).HasMaxLength(160);
            b.Property(x => x.OverviewDuration).HasMaxLength(100);
            b.Property(x => x.Route).HasMaxLength(160);
            b.Property(x => x.Rank).HasMaxLength(80);
            b.Property(x => x.WeatherReport).HasMaxLength(500);
            b.Property(x => x.Range).HasMaxLength(150);
            b.Property(x => x.WalkingPerDay).HasMaxLength(120);
            b.Property(x => x.Accommodation).HasMaxLength(200);
            b.Property(x => x.GroupSizeText).HasMaxLength(80);
            b.Property(x => x.Latitude).HasColumnType("numeric(10,6)");
            b.Property(x => x.Longitude).HasColumnType("numeric(10,6)");

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Destination);
            b.HasIndex(x => x.Featured);

            b.HasOne(x => x.ExpeditionType)
                .WithMany(x => x.Expeditions)
                .HasForeignKey(x => x.ExpeditionTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasMany(x => x.Faqs).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.MediaItems).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Itineraries).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Maps).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.CostItems).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.FixedDepartures).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.GearLists).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Highlights).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Reviews).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ExpeditionType>(b =>
        {
            b.ToTable("expedition_types");
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(600).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000);
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.HasIndex(x => x.Title).IsUnique();
            b.HasIndex(x => new { x.IsPublished, x.Ordering });
            b.HasMany(x => x.Images).WithOne(x => x.ExpeditionType).HasForeignKey(x => x.ExpeditionTypeId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ExpeditionTypeImage>(b =>
        {
            b.ToTable("expedition_type_images");
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.AltText).HasMaxLength(220);
            b.HasIndex(x => new { x.ExpeditionTypeId, x.SortOrder });
            b.HasIndex(x => new { x.ExpeditionTypeId, x.IsCover });
        });

        builder.Entity<ExpeditionFaq>(b =>
        {
            b.ToTable("expedition_faqs_legacy");
            b.Property(x => x.Question).HasMaxLength(300).IsRequired();
            b.HasIndex(x => x.ExpeditionId);
        });

        builder.Entity<ExpeditionMedia>(b =>
        {
            b.ToTable("expedition_media");
            b.Property(x => x.Url).HasMaxLength(500).IsRequired();
            b.Property(x => x.MediaType).HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.ExpeditionId);
        });

        builder.Entity<Itinerary>(b =>
        {
            b.ToTable("expedition_itineraries_legacy");
            b.Property(x => x.SeasonTitle).HasMaxLength(120).IsRequired();
            b.HasMany(x => x.Days).WithOne(x => x.Itinerary).HasForeignKey(x => x.ItineraryId).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(x => new { x.ExpeditionId, x.SortOrder });
        });

        builder.Entity<ItineraryDay>(b =>
        {
            b.ToTable("expedition_itinerary_items");
            b.Property(x => x.ShortDescription).HasMaxLength(400);
            b.Property(x => x.Meals).HasMaxLength(50);
            b.Property(x => x.AccommodationType).HasMaxLength(100);
            b.HasIndex(x => new { x.ItineraryId, x.DayNumber }).IsUnique();
        });

        builder.Entity<ExpeditionMap>(b =>
        {
            b.ToTable("expedition_maps");
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.Title).HasMaxLength(220);
            b.Property(x => x.Notes).HasMaxLength(4000);
        });

        builder.Entity<CostItem>(b =>
        {
            b.ToTable("expedition_cost_items");
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(800);
            b.HasIndex(x => new { x.ExpeditionId, x.Type, x.SortOrder });
        });

        builder.Entity<FixedDeparture>(b =>
        {
            b.ToTable("expedition_fixed_departures_legacy");
            b.HasIndex(x => new { x.ExpeditionId, x.StartDate, x.EndDate });
        });

        builder.Entity<GearList>(b =>
        {
            b.ToTable("expedition_gear_lists");
            b.Property(x => x.ShortDescription).HasMaxLength(800);
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
        });

        builder.Entity<ExpeditionHighlight>(b =>
        {
            b.ToTable("expedition_highlights");
            b.Property(x => x.Text).HasMaxLength(500).IsRequired();
            b.HasIndex(x => new { x.ExpeditionId, x.SortOrder });
        });

        builder.Entity<ExpeditionReview>(b =>
        {
            b.ToTable("expedition_reviews_legacy");
            b.Property(x => x.FullName).HasMaxLength(220).IsRequired();
            b.Property(x => x.EmailAddress).HasMaxLength(220).IsRequired();
            b.Property(x => x.UserPhotoPath).HasMaxLength(500);
            b.Property(x => x.VideoUrl).HasMaxLength(500);
            b.Property(x => x.ReviewText).HasMaxLength(4000).IsRequired();
        });

        builder.Entity<ExpeditionBasicInfo>(b =>
        {
            b.ToTable("expedition_basic_info");
            b.Property(x => x.Name).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(800).IsRequired();
            b.Property(x => x.Duration).HasMaxLength(120).IsRequired();
            b.Property(x => x.WalkingHoursPerDay).HasMaxLength(120);
            b.Property(x => x.Accommodation).HasMaxLength(200);
            b.Property(x => x.BestSeason).HasMaxLength(200);
            b.Property(x => x.GroupSize).HasMaxLength(80);
            b.Property(x => x.BannerImagePath).HasMaxLength(500);
            b.Property(x => x.ThumbnailImagePath).HasMaxLength(500);
            b.HasIndex(x => x.ExpeditionTypeId);
            b.HasOne(x => x.ExpeditionType).WithMany().HasForeignKey(x => x.ExpeditionTypeId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.Overview).WithOne(x => x.Expedition).HasForeignKey<ExpeditionOverview>(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Itineraries).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.InclusionExclusions).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.FixedDepartures).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.GearItems).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Reviews).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Faqs).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ExpeditionOverview>(b =>
        {
            b.ToTable("expedition_overviews");
            b.Property(x => x.Country).HasMaxLength(120);
            b.Property(x => x.PeakName).HasMaxLength(160);
            b.Property(x => x.Route).HasMaxLength(220);
            b.Property(x => x.Rank).HasMaxLength(80);
            b.Property(x => x.Range).HasMaxLength(160);
            b.Property(x => x.Coordinates).HasMaxLength(120);
            b.Property(x => x.WeatherInformation).HasMaxLength(500);
            b.HasIndex(x => x.ExpeditionId).IsUnique();
        });

        builder.Entity<ExpeditionItinerary>(b =>
        {
            b.ToTable("expedition_itineraries");
            b.Property(x => x.SeasonTitle).HasMaxLength(120).IsRequired();
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(800);
            b.Property(x => x.Accommodation).HasMaxLength(200);
            b.Property(x => x.Meals).HasMaxLength(100);
            b.Property(x => x.Elevation).HasMaxLength(120);
            b.HasIndex(x => x.ExpeditionId);
            b.HasIndex(x => new { x.ExpeditionId, x.SeasonTitle, x.DayNumber }).IsUnique();
        });

        builder.Entity<ExpeditionInclusionExclusion>(b =>
        {
            b.ToTable("expedition_inclusion_exclusions");
            b.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            b.HasIndex(x => x.ExpeditionId);
            b.HasIndex(x => new { x.ExpeditionId, x.Type, x.DisplayOrder });
        });

        builder.Entity<ExpeditionFixedDeparture>(b =>
        {
            b.ToTable("expedition_fixed_departures");
            b.Property(x => x.Price).HasColumnType("numeric(18,2)");
            b.Property(x => x.Currency).HasMaxLength(10).HasDefaultValue("USD");
            b.HasIndex(x => x.ExpeditionId);
        });

        builder.Entity<ExpeditionGear>(b =>
        {
            b.ToTable("expedition_gear_items");
            b.Property(x => x.ItemName).HasMaxLength(220).IsRequired();
            b.HasIndex(x => x.ExpeditionId);
            b.HasIndex(x => new { x.ExpeditionId, x.Category, x.DisplayOrder });
        });

        builder.Entity<ExpeditionReviewItem>(b =>
        {
            b.ToTable("expedition_reviews");
            b.Property(x => x.ClientName).HasMaxLength(220).IsRequired();
            b.Property(x => x.Country).HasMaxLength(120);
            b.Property(x => x.Title).HasMaxLength(220);
            b.Property(x => x.Comment).HasMaxLength(4000).IsRequired();
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.HasIndex(x => x.ExpeditionId);
        });

        builder.Entity<ExpeditionFaqItem>(b =>
        {
            b.ToTable("expedition_faqs");
            b.Property(x => x.Question).HasMaxLength(400).IsRequired();
            b.Property(x => x.Answer).HasMaxLength(4000).IsRequired();
            b.HasIndex(x => x.ExpeditionId);
            b.HasIndex(x => new { x.ExpeditionId, x.DisplayOrder });
        });
    }

    private static void ConfigureTrekking(ModelBuilder builder)
    {
        builder.Entity<TrekkingType>(b =>
        {
            b.ToTable("trekking_types");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(600).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000);
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.HasMany(x => x.Images).WithOne(x => x.TrekkingType).HasForeignKey(x => x.TrekkingTypeId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.TrekkingItems).WithOne(x => x.TrekkingType).HasForeignKey(x => x.TrekkingTypeId).OnDelete(DeleteBehavior.SetNull);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<TrekkingTypeImage>(b =>
        {
            b.ToTable("trekking_type_images");
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.AltText).HasMaxLength(200);
            b.HasIndex(x => x.TrekkingTypeId);
            b.HasIndex(x => new { x.TrekkingTypeId, x.SortOrder });
        });

        builder.Entity<Trekking>(b =>
        {
            b.ToTable("trekking");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(220).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(600).IsRequired();
            b.Property(x => x.Destination).HasMaxLength(200).IsRequired();
            b.Property(x => x.Price).HasColumnType("numeric(12,2)");
            b.Property(x => x.SummitBonusUsd).HasColumnType("numeric(12,2)");
            b.Property(x => x.ExpeditionStyle).HasMaxLength(120);
            b.Property(x => x.PeakName).HasMaxLength(160);
            b.Property(x => x.OverviewDuration).HasMaxLength(100);
            b.Property(x => x.Route).HasMaxLength(160);
            b.Property(x => x.Rank).HasMaxLength(80);
            b.Property(x => x.WeatherReport).HasMaxLength(500);
            b.Property(x => x.Range).HasMaxLength(150);
            b.Property(x => x.WalkingPerDay).HasMaxLength(120);
            b.Property(x => x.Accommodation).HasMaxLength(200);
            b.Property(x => x.GroupSizeText).HasMaxLength(80);
            b.Property(x => x.Latitude).HasColumnType("numeric(10,6)");
            b.Property(x => x.Longitude).HasColumnType("numeric(10,6)");

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Destination);
            b.HasIndex(x => x.Featured);

            b.HasMany(x => x.Faqs).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.MediaItems).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Itineraries).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Maps).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.CostItems).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.FixedDepartures).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.GearLists).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Highlights).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Reviews).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TrekkingFaq>(b =>
        {
            b.ToTable("trekking_faqs");
            b.Property(x => x.Question).HasMaxLength(400).IsRequired();
            b.Property(x => x.Answer).HasMaxLength(4000).IsRequired();
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.Ordering });
        });

        builder.Entity<TrekkingMedia>(b =>
        {
            b.ToTable("trekking_media");
            b.Property(x => x.Url).HasMaxLength(500).IsRequired();
            b.Property(x => x.MediaType).HasMaxLength(50).IsRequired();
            b.Property(x => x.FilePath).HasMaxLength(500);
            b.Property(x => x.VideoUrl).HasMaxLength(500);
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.Ordering });
        });

        builder.Entity<TrekkingItinerary>(b =>
        {
            b.ToTable("trekking_itineraries");
            b.Property(x => x.SeasonTitle).HasMaxLength(200).IsRequired();
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.SortOrder });
            b.HasMany(x => x.Days).WithOne(x => x.TrekkingItinerary).HasForeignKey(x => x.TrekkingItineraryId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TrekkingItineraryDay>(b =>
        {
            b.ToTable("trekking_itinerary_days");
            b.Property(x => x.ShortDescription).HasMaxLength(500);
            b.Property(x => x.Meals).HasMaxLength(200);
            b.Property(x => x.AccommodationType).HasMaxLength(200);
            b.HasIndex(x => x.TrekkingItineraryId);
            b.HasIndex(x => new { x.TrekkingItineraryId, x.DayNumber }).IsUnique();
        });

        builder.Entity<TrekkingMap>(b =>
        {
            b.ToTable("trekking_maps");
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.Title).HasMaxLength(300);
            b.HasIndex(x => x.TrekkingId);
        });

        builder.Entity<TrekkingCostItem>(b =>
        {
            b.ToTable("trekking_cost_items");
            b.Property(x => x.Title).HasMaxLength(300).IsRequired();
            b.Property(x => x.ShortDescription).HasMaxLength(1000);
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.Type, x.SortOrder });
        });

        builder.Entity<TrekkingFixedDeparture>(b =>
        {
            b.ToTable("trekking_fixed_departures");
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.StartDate });
        });

        builder.Entity<TrekkingGearList>(b =>
        {
            b.ToTable("trekking_gear_lists");
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.HasIndex(x => x.TrekkingId);
        });

        builder.Entity<TrekkingHighlight>(b =>
        {
            b.ToTable("trekking_highlights");
            b.Property(x => x.Text).HasMaxLength(1000).IsRequired();
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.SortOrder });
        });

        builder.Entity<TrekkingReview>(b =>
        {
            b.ToTable("trekking_reviews");
            b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            b.Property(x => x.EmailAddress).HasMaxLength(320).IsRequired();
            b.Property(x => x.UserPhotoPath).HasMaxLength(500);
            b.Property(x => x.VideoUrl).HasMaxLength(500);
            b.Property(x => x.ReviewText).HasMaxLength(4000).IsRequired();
            b.HasIndex(x => x.TrekkingId);
            b.HasIndex(x => new { x.TrekkingId, x.ModerationStatus });
        });
    }



    private static void ConfigureWhoWeAre(ModelBuilder builder)
    {
        builder.Entity<WhoWeAre>(b =>
        {
            b.ToTable("company_who_we_are");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.SubDescription).HasMaxLength(500);
            b.Property(x => x.Description).IsRequired();
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.Property(x => x.ImageCaption).HasMaxLength(300);
            b.HasMany(x => x.Images)
                .WithOne(x => x.WhoWeAre)
                .HasForeignKey(x => x.WhoWeAreId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<WhoWeAreImage>(b =>
        {
            b.ToTable("company_who_we_are_images");
            b.Property(x => x.ImagePath).HasMaxLength(500).IsRequired();
            b.Property(x => x.Caption).HasMaxLength(300);
            b.HasIndex(x => x.WhoWeAreId);
            b.HasIndex(x => new { x.WhoWeAreId, x.Ordering });
        });

        builder.Entity<WhoWeAreHero>(b =>
        {
            b.ToTable("company_who_we_are_hero");
            b.Property(x => x.Header).HasMaxLength(200).IsRequired();
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            b.Property(x => x.BackgroundImagePath).HasMaxLength(500);
        });
    }

    private static void ConfigureWhyWithUs(ModelBuilder builder)
    {
        builder.Entity<WhyWithUs>(b =>
        {
            b.ToTable("company_why_with_us");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            b.Property(x => x.IconCssClass).HasMaxLength(80);
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<WhyWithUsHero>(b =>
        {
            b.ToTable("company_why_with_us_hero");
            b.Property(x => x.Header).HasMaxLength(200).IsRequired();
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            b.Property(x => x.BackgroundImagePath).HasMaxLength(500);
        });
    }

    private static void ConfigureCompanyPages(ModelBuilder builder)
    {


        builder.Entity<BlogPost>(b =>
        {
            b.ToTable("company_blog_posts");
            b.Property(x => x.Title).HasMaxLength(280).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(320).IsRequired();
            b.Property(x => x.Summary).HasMaxLength(2000);
            b.Property(x => x.ContentHtml).HasMaxLength(40000).IsRequired();
            b.Property(x => x.HeroImagePath).HasMaxLength(500);
            b.Property(x => x.ThumbnailImagePath).HasMaxLength(500);
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsFeatured);
            b.HasIndex(x => x.IsPublished);
        });
        builder.Entity<Award>(b =>
        {
            b.ToTable("company_awards");
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.Property(x => x.Issuer).HasMaxLength(220);
            b.Property(x => x.Description).HasMaxLength(4000);
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.Property(x => x.ReferenceUrl).HasMaxLength(500);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<Patron>(b =>
        {
            b.ToTable("company_patrons");
            b.Property(x => x.Name).HasMaxLength(220).IsRequired();
            b.Property(x => x.Role).HasMaxLength(220).IsRequired();
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.Property(x => x.Biography).HasMaxLength(4000);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<ChairmanMessage>(b =>
        {
            b.ToTable("company_chairman_messages");
            b.Property(x => x.Heading).HasMaxLength(220).IsRequired();
            b.Property(x => x.ChairmanName).HasMaxLength(220).IsRequired();
            b.Property(x => x.Designation).HasMaxLength(220);
            b.Property(x => x.MessageHtml).HasMaxLength(12000).IsRequired();
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.Property(x => x.VideoUrl).HasMaxLength(500);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<TeamMember>(b =>
        {
            b.ToTable("company_team_members");
            b.Property(x => x.FullName).HasMaxLength(220).IsRequired();
            b.Property(x => x.Role).HasMaxLength(220).IsRequired();
            b.Property(x => x.Biography).HasMaxLength(4000);
            b.Property(x => x.ImagePath).HasMaxLength(500);
            b.Property(x => x.Email).HasMaxLength(220);
            b.Property(x => x.LinkedInUrl).HasMaxLength(500);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<CertificateDocument>(b =>
        {
            b.ToTable("company_certificate_documents");
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.Property(x => x.Category).HasMaxLength(160);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.FilePath).HasMaxLength(500);
            b.Property(x => x.ThumbnailImagePath).HasMaxLength(500);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });

        builder.Entity<Review>(b =>
        {
            b.ToTable("company_reviews");
            b.Property(x => x.ReviewerName).HasMaxLength(220).IsRequired();
            b.Property(x => x.ReviewerRole).HasMaxLength(220);
            b.Property(x => x.ReviewText).HasMaxLength(6000).IsRequired();
            b.Property(x => x.ReviewerImagePath).HasMaxLength(500);
            b.Property(x => x.SourceName).HasMaxLength(220);
            b.Property(x => x.SourceUrl).HasMaxLength(500);
            b.HasIndex(x => x.Ordering);
            b.HasIndex(x => x.IsPublished);
        });
    }

}
