using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutUsPage : Migration
    {
        private static readonly DateTime SeededOnUtc = new(2026, 8, 23, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subtitle = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ContentHtml = table.Column<string>(type: "text", nullable: true),
                    PrimaryImagePath = table.Column<string>(type: "text", nullable: true),
                    SecondaryImagePath = table.Column<string>(type: "text", nullable: true),
                    BadgeText = table.Column<string>(type: "text", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: true),
                    ButtonText = table.Column<string>(type: "text", nullable: true),
                    ButtonUrl = table.Column<string>(type: "text", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsHighlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AboutUsPageId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsHighlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsHighlights_AboutUsPages_AboutUsPageId",
                        column: x => x.AboutUsPageId,
                        principalTable: "AboutUsPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsHighlights_AboutUsPageId",
                table: "AboutUsHighlights",
                column: "AboutUsPageId");

            migrationBuilder.InsertData(
                table: "AboutUsPages",
                columns: new[] { "Id", "Subtitle", "Title", "Description", "BadgeText", "ContactPhone", "ButtonText", "ButtonUrl", "IsPublished", "CreatedAtUtc", "UpdatedAtUtc", "IsActive" },
                values: new object[] { 1, @"About Our Company", @"Explore the Himalayas with Trusted Experts", @"Welcome to Visit Kailash Treks Nepal, a professional trekking and tour operator offering unforgettable adventures across Nepal, Tibet, India, and Bhutan. We specialize in organizing Kailash Manasarovar tours, Himalayan trekking, rafting adventures, and spiritual pilgrimage journeys. Our experienced team ensures every trip is safe, comfortable, and perfectly organized for travelers from around the world.", @"12+ Successful Years", @"+977-9851008008", @"Know More", @"/contact", true, SeededOnUtc, SeededOnUtc, true });

            migrationBuilder.InsertData(
                table: "AboutUsHighlights",
                columns: new[] { "Id", "AboutUsPageId", "Text", "Ordering", "IsPublished", "CreatedAtUtc", "UpdatedAtUtc", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, @"Expert trekking guides and local travel specialists", 1, true, SeededOnUtc, SeededOnUtc, true },
                    { 2, 1, @"Kailash Manasarovar and Himalayan pilgrimage tours", 2, true, SeededOnUtc, SeededOnUtc, true },
                    { 3, 1, @"Safe, reliable, and well-planned travel experiences", 3, true, SeededOnUtc, SeededOnUtc, true }
                });

            // Explicit ids were used for the seed rows, so move the identity sequences past them.
            migrationBuilder.Sql(@"ALTER TABLE ""AboutUsPages"" ALTER COLUMN ""Id"" RESTART WITH 2;");
            migrationBuilder.Sql(@"ALTER TABLE ""AboutUsHighlights"" ALTER COLUMN ""Id"" RESTART WITH 4;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsHighlights");

            migrationBuilder.DropTable(
                name: "AboutUsPages");
        }
    }
}
