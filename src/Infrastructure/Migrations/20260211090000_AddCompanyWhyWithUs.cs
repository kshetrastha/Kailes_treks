using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class AddCompanyWhyWithUs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_why_with_us",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    IconCssClass = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_why_with_us", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_why_with_us_IsPublished",
                table: "company_why_with_us",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_why_with_us_Ordering",
                table: "company_why_with_us",
                column: "Ordering");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_why_with_us");
        }
    }
}
