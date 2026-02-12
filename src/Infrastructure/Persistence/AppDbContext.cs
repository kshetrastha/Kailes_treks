using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Domain.Entities;
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
    public DbSet<ExpeditionSection> ExpeditionSections => Set<ExpeditionSection>();
    public DbSet<ExpeditionItineraryDay> ExpeditionItineraryDays => Set<ExpeditionItineraryDay>();
    public DbSet<ExpeditionFaq> ExpeditionFaqs => Set<ExpeditionFaq>();
    public DbSet<ExpeditionMedia> ExpeditionMedia => Set<ExpeditionMedia>();

    public DbSet<Trekking> Trekking => Set<Trekking>();
    public DbSet<TrekkingItineraryDay> TrekkingItineraryDays => Set<TrekkingItineraryDay>();
    public DbSet<TrekkingFaq> TrekkingFaqs => Set<TrekkingFaq>();
    public DbSet<TrekkingMedia> TrekkingMedia => Set<TrekkingMedia>();
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

        ConfigureExpeditions(builder);
        ConfigureTrekking(builder);
        ConfigureWhyWithUs(builder);
        ConfigureWhoWeAre(builder);
        ConfigureCompanyPages(builder);
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

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Destination);
            b.HasIndex(x => x.Featured);

            b.HasOne(x => x.ExpeditionType)
                .WithMany(x => x.Expeditions)
                .HasForeignKey(x => x.ExpeditionTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasMany(x => x.Sections).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.ItineraryDays).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Faqs).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.MediaItems).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
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
        });

        builder.Entity<ExpeditionSection>(b =>
        {
            b.ToTable("expedition_sections");
            b.Property(x => x.SectionType).HasMaxLength(80).IsRequired();
            b.Property(x => x.Title).HasMaxLength(220).IsRequired();
            b.HasIndex(x => new { x.ExpeditionId, x.SectionType, x.Ordering });
        });

        builder.Entity<ExpeditionItineraryDay>(b =>
        {
            b.ToTable("expedition_itinerary_days");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.HasIndex(x => new { x.ExpeditionId, x.DayNumber }).IsUnique();
        });

        builder.Entity<ExpeditionFaq>(b =>
        {
            b.ToTable("expedition_faqs");
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
    }

    private static void ConfigureTrekking(ModelBuilder builder)
    {
        builder.Entity<Trekking>(b =>
        {
            b.ToTable("trekking");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(220).IsRequired();
            b.Property(x => x.Destination).HasMaxLength(200).IsRequired();
            b.Property(x => x.Difficulty).HasMaxLength(100).IsRequired();
            b.Property(x => x.Status).HasMaxLength(32).IsRequired();
            b.Property(x => x.Price).HasColumnType("numeric(12,2)");
            b.Property(x => x.AccommodationType).HasMaxLength(100);
            b.Property(x => x.TransportMode).HasMaxLength(100);
            b.Property(x => x.TrekPermitType).HasMaxLength(150);

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Destination);
            b.HasIndex(x => x.Featured);

            b.HasMany(x => x.ItineraryDays).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Faqs).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.MediaItems).WithOne(x => x.Trekking).HasForeignKey(x => x.TrekkingId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TrekkingItineraryDay>(b =>
        {
            b.ToTable("trekking_itinerary_days");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.HasIndex(x => new { x.TrekkingId, x.DayNumber }).IsUnique();
        });

        builder.Entity<TrekkingFaq>(b =>
        {
            b.ToTable("trekking_faqs");
            b.Property(x => x.Question).HasMaxLength(300).IsRequired();
            b.HasIndex(x => x.TrekkingId);
        });

        builder.Entity<TrekkingMedia>(b =>
        {
            b.ToTable("trekking_media");
            b.Property(x => x.Url).HasMaxLength(500).IsRequired();
            b.Property(x => x.MediaType).HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.TrekkingId);
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
