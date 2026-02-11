using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TravelCleanArch.Infrastructure.Persistence;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260212090000_add_who_we_are")]
    public partial class add_who_we_are : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_who_we_are",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageCaption = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_who_we_are_hero", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_IsPublished",
                table: "company_who_we_are",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_company_who_we_are_Ordering",
                table: "company_who_we_are",
                column: "Ordering");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_who_we_are");

            migrationBuilder.DropTable(
                name: "company_who_we_are_hero");
        }
    }
}
