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
                name: "Awards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Issuer = table.Column<string>(type: "text", nullable: true),
                    AwardedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ReferenceUrl = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    ContentHtml = table.Column<string>(type: "text", nullable: false),
                    HeroImagePath = table.Column<string>(type: "text", nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_CertificateDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChairmanMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Heading = table.Column<string>(type: "text", nullable: false),
                    ChairmanName = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: true),
                    MessageHtml = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChairmanMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Biography = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReviewerName = table.Column<string>(type: "text", nullable: false),
                    ReviewerRole = table.Column<string>(type: "text", nullable: true),
                    ReviewText = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ReviewerImagePath = table.Column<string>(type: "text", nullable: true),
                    SourceName = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Biography = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trekking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    MaxAltitudeMeters = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<string>(type: "text", nullable: false),
                    BestSeason = table.Column<string>(type: "text", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    Inclusions = table.Column<string>(type: "text", nullable: true),
                    Exclusions = table.Column<string>(type: "text", nullable: true),
                    HeroImageUrl = table.Column<string>(type: "text", nullable: true),
                    Permits = table.Column<string>(type: "text", nullable: true),
                    MinGroupSize = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    AvailableDates = table.Column<string>(type: "text", nullable: true),
                    BookingCtaUrl = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Featured = table.Column<bool>(type: "boolean", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    TrailGrade = table.Column<string>(type: "text", nullable: true),
                    TeaHouseAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    AccommodationType = table.Column<string>(type: "text", nullable: true),
                    Meals = table.Column<string>(type: "text", nullable: true),
                    TransportMode = table.Column<string>(type: "text", nullable: true),
                    TrekPermitType = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trekking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhoWeAre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    SubDescription = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImageCaption = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoWeAre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhoWeAreHeroes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoWeAreHeroes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhyWithUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IconCssClass = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhyWithUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhyWithUsHeroes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhyWithUsHeroes", x => x.Id);
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
                name: "ExpeditionBasicInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    DifficultyLevel = table.Column<int>(type: "integer", nullable: false),
                    MaxElevation = table.Column<int>(type: "integer", nullable: true),
                    Duration = table.Column<string>(type: "text", nullable: false),
                    WalkingHoursPerDay = table.Column<string>(type: "text", nullable: true),
                    Accommodation = table.Column<string>(type: "text", nullable: true),
                    BestSeason = table.Column<string>(type: "text", nullable: true),
                    GroupSize = table.Column<string>(type: "text", nullable: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    BannerImagePath = table.Column<string>(type: "text", nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionBasicInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionBasicInfos_ExpeditionTypes_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "ExpeditionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expeditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    MaxAltitudeMeters = table.Column<int>(type: "integer", nullable: false),
                    MaxAltitudeFeet = table.Column<int>(type: "integer", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "integer", nullable: true),
                    BestSeason = table.Column<int>(type: "integer", nullable: true),
                    WalkingPerDay = table.Column<string>(type: "text", nullable: true),
                    Accommodation = table.Column<string>(type: "text", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    OverviewCountry = table.Column<int>(type: "integer", nullable: false),
                    PeakName = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "text", nullable: true),
                    Rank = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordinatesText = table.Column<string>(type: "text", nullable: true),
                    WeatherReportUrl = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<string>(type: "text", nullable: true),
                    HeroImageUrl = table.Column<string>(type: "text", nullable: true),
                    HeroVideoUrl = table.Column<string>(type: "text", nullable: true),
                    MinGroupSize = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    GroupSizeText = table.Column<string>(type: "text", nullable: true),
                    PriceOnRequest = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    PriceNotesUrl = table.Column<string>(type: "text", nullable: true),
                    TripPdfUrl = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    AverageRating = table.Column<decimal>(type: "numeric", nullable: true),
                    RatingLabel = table.Column<string>(type: "text", nullable: true),
                    ReviewCount = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Featured = table.Column<bool>(type: "boolean", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    ExpeditionStyle = table.Column<string>(type: "text", nullable: true),
                    BoardBasis = table.Column<string>(type: "text", nullable: true),
                    OxygenSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SherpaSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SummitBonusUsd = table.Column<decimal>(type: "numeric", nullable: true),
                    Permits = table.Column<string>(type: "text", nullable: true),
                    RequiresClimbingPermit = table.Column<bool>(type: "boolean", nullable: false),
                    Difficulty = table.Column<string>(type: "text", nullable: false),
                    Inclusions = table.Column<string>(type: "text", nullable: true),
                    Exclusions = table.Column<string>(type: "text", nullable: true),
                    AvailableDates = table.Column<string>(type: "text", nullable: true),
                    BookingCtaUrl = table.Column<string>(type: "text", nullable: true),
                    SummitRoute = table.Column<string>(type: "text", nullable: true),
                    OverviewDuration = table.Column<string>(type: "text", nullable: true),
                    WeatherReport = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expeditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expeditions_ExpeditionTypes_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "ExpeditionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionTypeImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    AltText = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsCover = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionTypeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionTypeImages_ExpeditionTypes_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "ExpeditionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrekkingFaqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrekkingFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrekkingFaqs_Trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "Trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrekkingItineraryDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OvernightLocation = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrekkingItineraryDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrekkingItineraryDays_Trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "Trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrekkingMedia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    MediaType = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrekkingMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrekkingMedia_Trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "Trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhoWeAreImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WhoWeAreId = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoWeAreImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhoWeAreImages_WhoWeAre_WhoWeAreId",
                        column: x => x.WhoWeAreId,
                        principalTable: "WhoWeAre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionFaqItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionFaqItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionFaqItems_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionFixedDepartures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    BookedSeats = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsGuaranteed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionFixedDepartures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionFixedDepartures_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionGears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionGears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionGears_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionInclusionExclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionInclusionExclusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionInclusionExclusions_ExpeditionBasicInfos_Expediti~",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionItineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    SeasonTitle = table.Column<string>(type: "text", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    Accommodation = table.Column<string>(type: "text", nullable: true),
                    Meals = table.Column<string>(type: "text", nullable: true),
                    Elevation = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionItineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionItineraries_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionOverviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PeakName = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "text", nullable: true),
                    Rank = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<string>(type: "text", nullable: true),
                    Coordinates = table.Column<string>(type: "text", nullable: true),
                    WeatherInformation = table.Column<string>(type: "text", nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    MapEmbedCode = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionOverviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionOverviews_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionReviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionReviewItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionReviewItems_ExpeditionBasicInfos_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "ExpeditionBasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CostItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_CostItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostItems_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionFaqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionFaqs_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionHighlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionHighlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionHighlights_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionMaps_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionMedia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    MediaType = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_ExpeditionMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionMedia_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    UserPhotoPath = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ReviewText = table.Column<string>(type: "text", nullable: false),
                    ModerationStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionReviews_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixedDepartures",
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
                    table.PrimaryKey("PK_FixedDepartures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedDepartures_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GearLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GearLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GearLists_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    SeasonTitle = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itineraries_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItineraryId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Meals = table.Column<string>(type: "text", nullable: true),
                    AccommodationType = table.Column<string>(type: "text", nullable: true),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryDays_Expeditions_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "Expeditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItineraryDays_Itineraries_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itineraries",
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
                name: "IX_CostItems_ExpeditionId",
                table: "CostItems",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionBasicInfos_ExpeditionTypeId",
                table: "ExpeditionBasicInfos",
                column: "ExpeditionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionFaqItems_ExpeditionId",
                table: "ExpeditionFaqItems",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionFaqs_ExpeditionId",
                table: "ExpeditionFaqs",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionFixedDepartures_ExpeditionId",
                table: "ExpeditionFixedDepartures",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionGears_ExpeditionId",
                table: "ExpeditionGears",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionHighlights_ExpeditionId",
                table: "ExpeditionHighlights",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionInclusionExclusions_ExpeditionId",
                table: "ExpeditionInclusionExclusions",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionItineraries_ExpeditionId",
                table: "ExpeditionItineraries",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionMaps_ExpeditionId",
                table: "ExpeditionMaps",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionMedia_ExpeditionId",
                table: "ExpeditionMedia",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionOverviews_ExpeditionId",
                table: "ExpeditionOverviews",
                column: "ExpeditionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionReviewItems_ExpeditionId",
                table: "ExpeditionReviewItems",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionReviews_ExpeditionId",
                table: "ExpeditionReviews",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Expeditions_ExpeditionTypeId",
                table: "Expeditions",
                column: "ExpeditionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionTypeImages_ExpeditionTypeId",
                table: "ExpeditionTypeImages",
                column: "ExpeditionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDepartures_ExpeditionId",
                table: "FixedDepartures",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_GearLists_ExpeditionId",
                table: "GearLists",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_ExpeditionId",
                table: "Itineraries",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_ExpeditionId",
                table: "ItineraryDays",
                column: "ExpeditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_ItineraryId",
                table: "ItineraryDays",
                column: "ItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrekkingFaqs_TrekkingId",
                table: "TrekkingFaqs",
                column: "TrekkingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrekkingItineraryDays_TrekkingId",
                table: "TrekkingItineraryDays",
                column: "TrekkingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrekkingMedia_TrekkingId",
                table: "TrekkingMedia",
                column: "TrekkingId");

            migrationBuilder.CreateIndex(
                name: "IX_WhoWeAreImages_WhoWeAreId",
                table: "WhoWeAreImages",
                column: "WhoWeAreId");
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
                name: "Awards");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "CertificateDocuments");

            migrationBuilder.DropTable(
                name: "ChairmanMessages");

            migrationBuilder.DropTable(
                name: "CostItems");

            migrationBuilder.DropTable(
                name: "ExpeditionFaqItems");

            migrationBuilder.DropTable(
                name: "ExpeditionFaqs");

            migrationBuilder.DropTable(
                name: "ExpeditionFixedDepartures");

            migrationBuilder.DropTable(
                name: "ExpeditionGears");

            migrationBuilder.DropTable(
                name: "ExpeditionHighlights");

            migrationBuilder.DropTable(
                name: "ExpeditionInclusionExclusions");

            migrationBuilder.DropTable(
                name: "ExpeditionItineraries");

            migrationBuilder.DropTable(
                name: "ExpeditionMaps");

            migrationBuilder.DropTable(
                name: "ExpeditionMedia");

            migrationBuilder.DropTable(
                name: "ExpeditionOverviews");

            migrationBuilder.DropTable(
                name: "ExpeditionReviewItems");

            migrationBuilder.DropTable(
                name: "ExpeditionReviews");

            migrationBuilder.DropTable(
                name: "ExpeditionTypeImages");

            migrationBuilder.DropTable(
                name: "FixedDepartures");

            migrationBuilder.DropTable(
                name: "GearLists");

            migrationBuilder.DropTable(
                name: "ItineraryDays");

            migrationBuilder.DropTable(
                name: "Patrons");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "TrekkingFaqs");

            migrationBuilder.DropTable(
                name: "TrekkingItineraryDays");

            migrationBuilder.DropTable(
                name: "TrekkingMedia");

            migrationBuilder.DropTable(
                name: "WhoWeAreHeroes");

            migrationBuilder.DropTable(
                name: "WhoWeAreImages");

            migrationBuilder.DropTable(
                name: "WhyWithUs");

            migrationBuilder.DropTable(
                name: "WhyWithUsHeroes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ExpeditionBasicInfos");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "Trekking");

            migrationBuilder.DropTable(
                name: "WhoWeAre");

            migrationBuilder.DropTable(
                name: "Expeditions");

            migrationBuilder.DropTable(
                name: "ExpeditionTypes");
        }
    }
}
