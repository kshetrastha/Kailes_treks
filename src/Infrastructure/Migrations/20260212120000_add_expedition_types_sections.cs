using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class add_expedition_types_sections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpeditionTypeId",
                table: "expeditions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "expeditions",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "expedition_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
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
                name: "expedition_sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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

            migrationBuilder.CreateIndex(
                name: "IX_expeditions_ExpeditionTypeId",
                table: "expeditions",
                column: "ExpeditionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_expedition_sections_ExpeditionId_SectionType_Ordering",
                table: "expedition_sections",
                columns: new[] { "ExpeditionId", "SectionType", "Ordering" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_types_IsPublished_Ordering",
                table: "expedition_types",
                columns: new[] { "IsPublished", "Ordering" });

            migrationBuilder.CreateIndex(
                name: "IX_expedition_types_Title",
                table: "expedition_types",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_expeditions_expedition_types_ExpeditionTypeId",
                table: "expeditions",
                column: "ExpeditionTypeId",
                principalTable: "expedition_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expeditions_expedition_types_ExpeditionTypeId",
                table: "expeditions");

            migrationBuilder.DropTable(
                name: "expedition_sections");

            migrationBuilder.DropTable(
                name: "expedition_types");

            migrationBuilder.DropIndex(
                name: "IX_expeditions_ExpeditionTypeId",
                table: "expeditions");

            migrationBuilder.DropColumn(
                name: "ExpeditionTypeId",
                table: "expeditions");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "expeditions");
        }
    }
}
