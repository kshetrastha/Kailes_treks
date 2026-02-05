using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class AddExpeditionsAndTrekking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expeditions",
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
                    SummitRoute = table.Column<string>(type: "text", nullable: true),
                    RequiresClimbingPermit = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expeditions", x => x.Id);
                    table.ForeignKey("FK_expeditions_AspNetUsers_CreatedBy", x => x.CreatedBy, "AspNetUsers", "Id");
                    table.ForeignKey("FK_expeditions_AspNetUsers_UpdatedBy", x => x.UpdatedBy, "AspNetUsers", "Id");
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
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking", x => x.Id);
                    table.ForeignKey("FK_trekking_AspNetUsers_CreatedBy", x => x.CreatedBy, "AspNetUsers", "Id");
                    table.ForeignKey("FK_trekking_AspNetUsers_UpdatedBy", x => x.UpdatedBy, "AspNetUsers", "Id");
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
                    Ordering = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_faqs", x => x.Id);
                    table.ForeignKey("FK_expedition_faqs_expeditions_ExpeditionId", x => x.ExpeditionId, "expeditions", "Id", onDelete: ReferentialAction.Cascade);
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
                    OvernightLocation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_itinerary_days", x => x.Id);
                    table.ForeignKey("FK_expedition_itinerary_days_expeditions_ExpeditionId", x => x.ExpeditionId, "expeditions", "Id", onDelete: ReferentialAction.Cascade);
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
                    Ordering = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expedition_media", x => x.Id);
                    table.ForeignKey("FK_expedition_media_expeditions_ExpeditionId", x => x.ExpeditionId, "expeditions", "Id", onDelete: ReferentialAction.Cascade);
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
                    Ordering = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_faqs", x => x.Id);
                    table.ForeignKey("FK_trekking_faqs_trekking_TrekkingId", x => x.TrekkingId, "trekking", "Id", onDelete: ReferentialAction.Cascade);
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
                    OvernightLocation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_itinerary_days", x => x.Id);
                    table.ForeignKey("FK_trekking_itinerary_days_trekking_TrekkingId", x => x.TrekkingId, "trekking", "Id", onDelete: ReferentialAction.Cascade);
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
                    Ordering = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_media", x => x.Id);
                    table.ForeignKey("FK_trekking_media_trekking_TrekkingId", x => x.TrekkingId, "trekking", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_expeditions_CreatedBy", table: "expeditions", column: "CreatedBy");
            migrationBuilder.CreateIndex(name: "IX_expeditions_Destination", table: "expeditions", column: "Destination");
            migrationBuilder.CreateIndex(name: "IX_expeditions_Featured", table: "expeditions", column: "Featured");
            migrationBuilder.CreateIndex(name: "IX_expeditions_Slug", table: "expeditions", column: "Slug", unique: true);
            migrationBuilder.CreateIndex(name: "IX_expeditions_Status", table: "expeditions", column: "Status");
            migrationBuilder.CreateIndex(name: "IX_expeditions_UpdatedBy", table: "expeditions", column: "UpdatedBy");

            migrationBuilder.CreateIndex(name: "IX_expedition_faqs_ExpeditionId", table: "expedition_faqs", column: "ExpeditionId");
            migrationBuilder.CreateIndex(name: "IX_expedition_itinerary_days_ExpeditionId_DayNumber", table: "expedition_itinerary_days", columns: new[] { "ExpeditionId", "DayNumber" }, unique: true);
            migrationBuilder.CreateIndex(name: "IX_expedition_media_ExpeditionId", table: "expedition_media", column: "ExpeditionId");

            migrationBuilder.CreateIndex(name: "IX_trekking_CreatedBy", table: "trekking", column: "CreatedBy");
            migrationBuilder.CreateIndex(name: "IX_trekking_Destination", table: "trekking", column: "Destination");
            migrationBuilder.CreateIndex(name: "IX_trekking_Featured", table: "trekking", column: "Featured");
            migrationBuilder.CreateIndex(name: "IX_trekking_Slug", table: "trekking", column: "Slug", unique: true);
            migrationBuilder.CreateIndex(name: "IX_trekking_Status", table: "trekking", column: "Status");
            migrationBuilder.CreateIndex(name: "IX_trekking_UpdatedBy", table: "trekking", column: "UpdatedBy");

            migrationBuilder.CreateIndex(name: "IX_trekking_faqs_TrekkingId", table: "trekking_faqs", column: "TrekkingId");
            migrationBuilder.CreateIndex(name: "IX_trekking_itinerary_days_TrekkingId_DayNumber", table: "trekking_itinerary_days", columns: new[] { "TrekkingId", "DayNumber" }, unique: true);
            migrationBuilder.CreateIndex(name: "IX_trekking_media_TrekkingId", table: "trekking_media", column: "TrekkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "expedition_faqs");
            migrationBuilder.DropTable(name: "expedition_itinerary_days");
            migrationBuilder.DropTable(name: "expedition_media");
            migrationBuilder.DropTable(name: "trekking_faqs");
            migrationBuilder.DropTable(name: "trekking_itinerary_days");
            migrationBuilder.DropTable(name: "trekking_media");
            migrationBuilder.DropTable(name: "expeditions");
            migrationBuilder.DropTable(name: "trekking");
        }
    }
}
