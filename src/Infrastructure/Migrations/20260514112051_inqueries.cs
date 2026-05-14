using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inqueries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trekking_inquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Comment = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SourcePage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trekking_inquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trekking_inquiries_Trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "Trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_trekking_inquiries_SubmittedAtUtc",
                table: "trekking_inquiries",
                column: "SubmittedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_trekking_inquiries_TrekkingId",
                table: "trekking_inquiries",
                column: "TrekkingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trekking_inquiries");
        }
    }
}
