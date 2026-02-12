using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TravelCleanArch.Infrastructure.Persistence;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260212000000_add_company_blog_posts")]
public partial class add_company_blog_posts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "company_blog_posts",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Title = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: false),
                Slug = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                ContentHtml = table.Column<string>(type: "character varying(40000)", maxLength: 40000, nullable: false),
                HeroImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                ThumbnailImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                table.PrimaryKey("PK_company_blog_posts", x => x.Id);
            });

        migrationBuilder.CreateIndex(name: "IX_company_blog_posts_IsFeatured", table: "company_blog_posts", column: "IsFeatured");
        migrationBuilder.CreateIndex(name: "IX_company_blog_posts_IsPublished", table: "company_blog_posts", column: "IsPublished");
        migrationBuilder.CreateIndex(name: "IX_company_blog_posts_Ordering", table: "company_blog_posts", column: "Ordering");
        migrationBuilder.CreateIndex(name: "IX_company_blog_posts_Slug", table: "company_blog_posts", column: "Slug", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "company_blog_posts");
    }
}
