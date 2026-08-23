using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivacyPolicySections : Migration
    {
        private static readonly DateTime SeededOnUtc = new(2026, 8, 23, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivacyPolicySections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ContentHtml = table.Column<string>(type: "text", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsContactBlock = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyPolicySections", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PrivacyPolicySections",
                columns: new[] { "Title", "ContentHtml", "Ordering", "IsContactBlock", "IsPublished", "CreatedAtUtc", "UpdatedAtUtc", "IsActive" },
                values: new object[,]
                {
                    { @"Introduction", @"<p>Welcome to Visit Kailash Treks. Your privacy is important to us. This Privacy Policy explains how we collect, use, and protect your personal information when you use our website and services related to Kailash Mansarovar Yatra and other travel packages.</p>", 1, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Information We Collect", @"<p>We may collect personal information such as your name, phone number, email address, passport details, and payment information when you make inquiries or book a tour with us.</p>", 2, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"How We Use Your Information", @"<p class=""mb-10"">We use your information to:</p><ul><li>Process your bookings and travel arrangements</li><li>Provide customer support and travel updates</li><li>Arrange permits and documentation for Kailash Mansarovar Yatra</li><li>Improve our website and services</li><li>Send important notifications related to your trip</li></ul>", 3, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Sharing Your Information", @"<p>We do not sell your personal information. However, we may share it with government authorities, travel partners, and service providers when necessary for permits, transportation, accommodation, and legal compliance.</p>", 4, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Cookies & Tracking", @"<p>Our website uses cookies to enhance user experience, analyze traffic, and improve functionality. You can choose to disable cookies through your browser settings.</p>", 5, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Data Security", @"<p>We take appropriate security measures to protect your personal data. However, no online system is completely secure, and we cannot guarantee absolute security.</p>", 6, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Third-Party Services", @"<p>Our website may include links to third-party services such as payment gateways or partner sites. We are not responsible for their privacy practices.</p>", 7, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Changes to This Policy", @"<p>We may update this Privacy Policy from time to time. Any changes will be posted on this page with updated information.</p>", 8, false, true, SeededOnUtc, SeededOnUtc, true },
                    { @"Contact Us", @"<p class=""mb-10"">If you have any questions regarding this Privacy Policy, feel free to contact us:</p><ul><li>Email: <span><a href=""mailto:visitkailashtreks@gmail.com"">visitkailashtreks@gmail.com</a></span></li><li>Phone: <span><a href=""tel:+97715912501"">+977 1 5912501</a></span></li></ul><div class=""policy-address""><p class=""mb-0"">Kathmandu, Nepal</p></div>", 9, true, true, SeededOnUtc, SeededOnUtc, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivacyPolicySections");
        }
    }
}
