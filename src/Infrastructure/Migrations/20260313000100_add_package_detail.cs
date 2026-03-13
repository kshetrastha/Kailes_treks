using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class add_package_detail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Master");

            migrationBuilder.CreateTable(
                name: "PackageDetail",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    ServiceRegionId = table.Column<int>(type: "integer", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDiscounted = table.Column<bool>(type: "boolean", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Duration = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WalkingPerDay = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    StartingPoint = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EndingPoint = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BestSeller = table.Column<bool>(type: "boolean", nullable: false),
                    PopularityRank = table.Column<int>(type: "integer", nullable: false),
                    Availability = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TotalDistance = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxElevation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SlugURL = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageDetail_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Master",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageDetail_DifficultyLevel_DifficultyLevelId",
                        column: x => x.DifficultyLevelId,
                        principalSchema: "Master",
                        principalTable: "DifficultyLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageDetail_ServiceRegions_ServiceRegionId",
                        column: x => x.ServiceRegionId,
                        principalSchema: "Service",
                        principalTable: "ServiceRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageDetail_CategoryId",
                schema: "Master",
                table: "PackageDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageDetail_DifficultyLevelId",
                schema: "Master",
                table: "PackageDetail",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageDetail_ServiceRegionId",
                schema: "Master",
                table: "PackageDetail",
                column: "ServiceRegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageDetail",
                schema: "Master");
        }
    }
}
