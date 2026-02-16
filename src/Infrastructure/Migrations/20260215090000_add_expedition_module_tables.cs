using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class add_expedition_module_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expedition_basic_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    DifficultyLevel = table.Column<int>(type: "integer", nullable: false),
                    MaxElevation = table.Column<int>(type: "integer", nullable: true),
                    Duration = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    WalkingHoursPerDay = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Accommodation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BestSeason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GroupSize = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    BannerImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_basic_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_basic_info_expedition_types_ExpeditionTypeId",
                        column: x => x.ExpeditionTypeId,
                        principalTable: "expedition_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "expedition_faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Answer = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_faqs_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
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
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    BookedSeats = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsGuaranteed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_fixed_departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_fixed_departures_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_gear_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    ItemName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_gear_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_gear_items_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_inclusion_exclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_inclusion_exclusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_inclusion_exclusions_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
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
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    Accommodation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Meals = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Elevation = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_itineraries_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expedition_overviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpeditionId = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    PeakName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Route = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    Rank = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Range = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Coordinates = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    WeatherInformation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    MapEmbedCode = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_overviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_overviews_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
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
                    ClientName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Country = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    Comment = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expedition_reviews_expedition_basic_info_ExpeditionId",
                        column: x => x.ExpeditionId,
                        principalTable: "expedition_basic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_expedition_basic_info_ExpeditionTypeId", table: "expedition_basic_info", column: "ExpeditionTypeId");
            migrationBuilder.CreateIndex(name: "IX_expedition_faqs_ExpeditionId", table: "expedition_faqs", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_faqs_ExpeditionId_DisplayOrder", table: "expedition_faqs", columns: new[] { "ExpeditionId", "DisplayOrder" });
            migrationBuilder.CreateIndex(name: "IX_expedition_fixed_departures_ExpeditionId", table: "expedition_fixed_departures", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_gear_items_ExpeditionId", table: "expedition_gear_items", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_gear_items_ExpeditionId_Category_DisplayOrder", table: "expedition_gear_items", columns: new[] { "ExpeditionId", "Category", "DisplayOrder" });
            migrationBuilder.CreateIndex(name: "IX_expedition_inclusion_exclusions_ExpeditionId", table: "expedition_inclusion_exclusions", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_inclusion_exclusions_ExpeditionId_Type_DisplayOrder", table: "expedition_inclusion_exclusions", columns: new[] { "ExpeditionId", "Type", "DisplayOrder" });
            migrationBuilder.CreateIndex(name: "IX_expedition_itineraries_ExpeditionId", table: "expedition_itineraries", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_itineraries_ExpeditionId_SeasonTitle_DayNumber", table: "expedition_itineraries", columns: new[] { "ExpeditionId", "SeasonTitle", "DayNumber" }, unique: true);
            migrationBuilder.CreateIndex(name: "IX_expedition_overviews_ExpeditionId", table: "expedition_overviews", column: "ExpeditionId", unique: true);
            migrationBuilder.CreateIndex(name: "IX_expedition_reviews_ExpeditionId", table: "expedition_reviews", column: "ExpeditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "expedition_faqs");
            migrationBuilder.DropTable(name: "expedition_fixed_departures");
            migrationBuilder.DropTable(name: "expedition_gear_items");
            migrationBuilder.DropTable(name: "expedition_inclusion_exclusions");
            migrationBuilder.DropTable(name: "expedition_itineraries");
            migrationBuilder.DropTable(name: "expedition_overviews");
            migrationBuilder.DropTable(name: "expedition_reviews");
            migrationBuilder.DropTable(name: "expedition_basic_info");
        }
    }
}
