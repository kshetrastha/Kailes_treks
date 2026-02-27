using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class service_region_faqs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceRegionFAQ",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceRegionId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRegionFAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRegionFAQ_ServiceRegions_ServiceRegionId",
                        column: x => x.ServiceRegionId,
                        principalSchema: "Service",
                        principalTable: "ServiceRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRegionFAQ_ServiceRegionId",
                schema: "Service",
                table: "ServiceRegionFAQ",
                column: "ServiceRegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRegionFAQ",
                schema: "Service");
        }
    }
}
