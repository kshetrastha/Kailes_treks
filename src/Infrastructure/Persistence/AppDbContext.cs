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
    public DbSet<ExpeditionItineraryDay> ExpeditionItineraryDays => Set<ExpeditionItineraryDay>();
    public DbSet<ExpeditionFaq> ExpeditionFaqs => Set<ExpeditionFaq>();
    public DbSet<ExpeditionMedia> ExpeditionMedia => Set<ExpeditionMedia>();

    public DbSet<Trekking> Trekking => Set<Trekking>();
    public DbSet<TrekkingItineraryDay> TrekkingItineraryDays => Set<TrekkingItineraryDay>();
    public DbSet<TrekkingFaq> TrekkingFaqs => Set<TrekkingFaq>();
    public DbSet<TrekkingMedia> TrekkingMedia => Set<TrekkingMedia>();
    public DbSet<WhyWithUs> WhyWithUs => Set<WhyWithUs>();
    public DbSet<WhyWithUsHero> WhyWithUsHeroes => Set<WhyWithUsHero>();

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
    }

    private static void ConfigureExpeditions(ModelBuilder builder)
    {
        builder.Entity<Expedition>(b =>
        {
            b.ToTable("expeditions");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(220).IsRequired();
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

            b.HasMany(x => x.ItineraryDays).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Faqs).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.MediaItems).WithOne(x => x.Expedition).HasForeignKey(x => x.ExpeditionId).OnDelete(DeleteBehavior.Cascade);
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

    private static void ConfigureWhyWithUs(ModelBuilder builder)
    {
        builder.Entity<WhyWithUs>(b =>
        {
            b.ToTable("company_why_with_us");
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            b.Property(x => x.IconCssClass).HasMaxLength(80);
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
}
