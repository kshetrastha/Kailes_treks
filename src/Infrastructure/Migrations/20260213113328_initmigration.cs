using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_awards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Issuer = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    AwardedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReferenceUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_awards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_blog_posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: false),
                    Slug = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ContentHtml = table.Column<string>(type: "character varying(40000)", maxLength: 40000, nullable: false),
                    HeroImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PublishedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_blog_posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_certificate_documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Category = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IssuedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_certificate_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_chairman_messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Heading = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ChairmanName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Designation = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    MessageHtml = table.Column<string>(type: "character varying(12000)", maxLength: 12000, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_chairman_messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_patrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Role = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Biography = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_patrons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReviewerName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ReviewerRole = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    ReviewText = table.Column<string>(type: "character varying(6000)", maxLength: 6000, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ReviewerImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SourceName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    SourceUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReviewedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_team_members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Role = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Biography = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_team_members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_who_we_are",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SubDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageCaption = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_who_we_are", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_who_we_are_hero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_who_we_are_hero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_why_with_us",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    IconCssClass = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_why_with_us", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_why_with_us_hero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_why_with_us_hero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "expedition_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "trekking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Destination = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    MaxAltitudeMeters = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BestSeason = table.Column<string>(type: "text", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    Inclusions = table.Column<string>(type: "text", nullable: true),
                    Exclusions = table.Column<string>(type: "text", nullable: true),
                    HeroImageUrl = table.Column<string>(type: "text", nullable: true),
                    Permits = table.Column<string>(type: "text", nullable: true),
                    MinGroupSize = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    AvailableDates = table.Column<string>(type: "text", nullable: true),
                    BookingCtaUrl = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Featured = table.Column<bool>(type: "boolean", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    TrailGrade = table.Column<string>(type: "text", nullable: true),
                    TeaHouseAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    AccommodationType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Meals = table.Column<string>(type: "text", nullable: true),
                    TransportMode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TrekPermitType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_who_we_are_images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WhoWeAreId = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_who_we_are_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_who_we_are_images_company_who_we_are_WhoWeAreId",
                        column: x => x.WhoWeAreId,
                        principalTable: "company_who_we_are",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_type_images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsCover = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_type_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_type_images_expedition_types_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "expedition_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expeditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    Destination = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    MaxAltitudeMeters = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DifficultyLevel = table.Column<int>(type: "integer", nullable: true),
                    BestSeason = table.Column<string>(type: "text", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    OverviewCountry = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    PeakName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    OverviewDuration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Route = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Rank = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(10,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(10,6)", nullable: true),
                    WeatherReport = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Range = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Inclusions = table.Column<string>(type: "text", nullable: true),
                    Exclusions = table.Column<string>(type: "text", nullable: true),
                    HeroImageUrl = table.Column<string>(type: "text", nullable: true),
                    Permits = table.Column<string>(type: "text", nullable: true),
                    MinGroupSize = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    GroupSizeText = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    AvailableDates = table.Column<string>(type: "text", nullable: true),
                    BookingCtaUrl = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Featured = table.Column<bool>(type: "boolean", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    SummitRoute = table.Column<string>(type: "text", nullable: true),
                    RequiresClimbingPermit = table.Column<bool>(type: "boolean", nullable: false),
                    ExpeditionStyle = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    OxygenSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SherpaSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SummitBonusUsd = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    WalkingPerDay = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Accommodation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expeditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expeditions_expedition_types_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "expedition_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "trekking_faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trekking_faqs_trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trekking_itinerary_days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OvernightLocation = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_itinerary_days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trekking_itinerary_days_trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trekking_media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    MediaType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trekking_media_trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_cost_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_cost_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_cost_items_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_faqs_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_fixed_departures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ForDays = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    GroupSize = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_fixed_departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_fixed_departures_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_gear_lists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_gear_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_gear_lists_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_highlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_highlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_highlights_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    SeasonTitle = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_itineraries_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_itinerary_days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OvernightLocation = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_itinerary_days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_itinerary_days_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    Notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_maps_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    MediaType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    MediaKind = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_media_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    UserPhotoPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ReviewText = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ModerationStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_reviews_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    SectionType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_sections_expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_itinerary_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItineraryId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Meals = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AccommodationType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_itinerary_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_itinerary_items_expedition_itineraries_Itinerary~",
                        column: x => x.ItineraryId,
                        principalTable: "expedition_itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_awards_IsPublished",
                table: "company_awards",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_awards_Ordering",
                table: "company_awards",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_blog_posts_IsFeatured",
                table: "company_blog_posts",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_company_blog_posts_IsPublished",
                table: "company_blog_posts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_blog_posts_Ordering",
                table: "company_blog_posts",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_blog_posts_Slug",
                table: "company_blog_posts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_certificate_documents_IsPublished",
                table: "company_certificate_documents",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_certificate_documents_Ordering",
                table: "company_certificate_documents",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_chairman_messages_IsPublished",
                table: "company_chairman_messages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_patrons_IsPublished",
                table: "company_patrons",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_patrons_Ordering",
                table: "company_patrons",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_reviews_IsPublished",
                table: "company_reviews",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_reviews_Ordering",
                table: "company_reviews",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_team_members_IsPublished",
                table: "company_team_members",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_team_members_Ordering",
                table: "company_team_members",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_IsPublished",
                table: "company_who_we_are",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_Ordering",
                table: "company_who_we_are",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_images_WhoWeAreId",
                table: "company_who_we_are_images",
                column: "WhoWeAreId");

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_images_WhoWeAreId_Ordering",
                table: "company_who_we_are_images",
                columns: new[] { "WhoWeAreId", "Ordering" });

            migrationBuilder.CreateIndex(
                name: "IX_company_why_with_us_IsPublished",
                table: "company_why_with_us",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_why_with_us_Ordering",
                table: "company_why_with_us",
                column: "Ordering");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_cost_items_ExpeditionId_Type_SortOrder",
                table: "expedition_cost_items",
                columns: new[] { "ExpeditionId", "Type", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_faqs_ExpeditionId",
                table: "expedition_faqs",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_fixed_departures_ExpeditionId_StartDate_EndDate",
                table: "expedition_fixed_departures",
                columns: new[] { "ExpeditionId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_gear_lists_ExpeditionId",
                table: "expedition_gear_lists",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_highlights_ExpeditionId_SortOrder",
                table: "expedition_highlights",
                columns: new[] { "ExpeditionId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_itineraries_ExpeditionId_SortOrder",
                table: "expedition_itineraries",
                columns: new[] { "ExpeditionId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_itinerary_days_ExpeditionId_DayNumber",
                table: "expedition_itinerary_days",
                columns: new[] { "ExpeditionId", "DayNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expedition_itinerary_items_ItineraryId_DayNumber",
                table: "expedition_itinerary_items",
                columns: new[] { "ItineraryId", "DayNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expedition_maps_ExpeditionId",
                table: "expedition_maps",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_media_ExpeditionId",
                table: "expedition_media",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_reviews_ExpeditionId",
                table: "expedition_reviews",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_sections_ExpeditionId_SectionType_Ordering",
                table: "expedition_sections",
                columns: new[] { "ExpeditionId", "SectionType", "Ordering" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_type_images_ExpeditionTypeId_IsCover",
                table: "expedition_type_images",
                columns: new[] { "ExpeditionTypeId", "IsCover" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_type_images_ExpeditionTypeId_SortOrder",
                table: "expedition_type_images",
                columns: new[] { "ExpeditionTypeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_types_IsPublished_Ordering",
                table: "expedition_types",
                columns: new[] { "IsPublished", "Ordering" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_types_Title",
                table: "expedition_types",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_Destination",
                table: "expeditions",
                column: "Destination");

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_ExpeditionTypeId",
                table: "expeditions",
                column: "ExpeditionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_Featured",
                table: "expeditions",
                column: "Featured");

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_Slug",
                table: "expeditions",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_Status",
                table: "expeditions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_Destination",
                table: "trekking",
                column: "Destination");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_Featured",
                table: "trekking",
                column: "Featured");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_Slug",
                table: "trekking",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trekking_Status",
                table: "trekking",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_faqs_TrekkingId",
                table: "trekking_faqs",
                column: "TrekkingId");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_itinerary_days_TrekkingId_DayNumber",
                table: "trekking_itinerary_days",
                columns: new[] { "TrekkingId", "DayNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trekking_media_TrekkingId",
                table: "trekking_media",
                column: "TrekkingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "company_awards");

            migrationBuilder.DropTable(
                name: "company_blog_posts");

            migrationBuilder.DropTable(
                name: "company_certificate_documents");

            migrationBuilder.DropTable(
                name: "company_chairman_messages");

            migrationBuilder.DropTable(
                name: "company_patrons");

            migrationBuilder.DropTable(
                name: "company_reviews");

            migrationBuilder.DropTable(
                name: "company_team_members");

            migrationBuilder.DropTable(
                name: "company_who_we_are_hero");

            migrationBuilder.DropTable(
                name: "company_who_we_are_images");

            migrationBuilder.DropTable(
                name: "company_why_with_us");

            migrationBuilder.DropTable(
                name: "company_why_with_us_hero");

            migrationBuilder.DropTable(
                name: "expedition_cost_items");

            migrationBuilder.DropTable(
                name: "expedition_faqs");

            migrationBuilder.DropTable(
                name: "expedition_fixed_departures");

            migrationBuilder.DropTable(
                name: "expedition_gear_lists");

            migrationBuilder.DropTable(
                name: "expedition_highlights");

            migrationBuilder.DropTable(
                name: "expedition_itinerary_days");

            migrationBuilder.DropTable(
                name: "expedition_itinerary_items");

            migrationBuilder.DropTable(
                name: "expedition_maps");

            migrationBuilder.DropTable(
                name: "expedition_media");

            migrationBuilder.DropTable(
                name: "expedition_reviews");

            migrationBuilder.DropTable(
                name: "expedition_sections");

            migrationBuilder.DropTable(
                name: "expedition_type_images");

            migrationBuilder.DropTable(
                name: "trekking_faqs");

            migrationBuilder.DropTable(
                name: "trekking_itinerary_days");

            migrationBuilder.DropTable(
                name: "trekking_media");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "company_who_we_are");

            migrationBuilder.DropTable(
                name: "expedition_itineraries");

            migrationBuilder.DropTable(
                name: "trekking");

            migrationBuilder.DropTable(
                name: "expeditions");

            migrationBuilder.DropTable(
                name: "expedition_types");
        }
    }
}
