using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reference = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TrekkingId = table.Column<int>(type: "integer", nullable: false),
                    PackageName = table.Column<string>(type: "text", nullable: false),
                    PackageSlug = table.Column<string>(type: "text", nullable: false),
                    PackageDestination = table.Column<string>(type: "text", nullable: true),
                    PackageRegion = table.Column<string>(type: "text", nullable: true),
                    PackageTrekkingType = table.Column<string>(type: "text", nullable: true),
                    PackageDifficulty = table.Column<string>(type: "text", nullable: true),
                    PackageDurationDays = table.Column<int>(type: "integer", nullable: false),
                    PackageMaxAltitudeMeters = table.Column<int>(type: "integer", nullable: false),
                    PriceOnRequest = table.Column<bool>(type: "boolean", nullable: false),
                    PricePerPerson = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PreferredStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FixedDepartureId = table.Column<int>(type: "integer", nullable: true),
                    NumberOfTravellers = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    AlternatePhone = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    PassportNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "text", nullable: true),
                    SpecialRequests = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    IpCountry = table.Column<string>(type: "text", nullable: true),
                    IpCountryCode = table.Column<string>(type: "text", nullable: true),
                    IpRegion = table.Column<string>(type: "text", nullable: true),
                    IpCity = table.Column<string>(type: "text", nullable: true),
                    IpPostalCode = table.Column<string>(type: "text", nullable: true),
                    IpTimeZone = table.Column<string>(type: "text", nullable: true),
                    IpOrganisation = table.Column<string>(type: "text", nullable: true),
                    IpLatitude = table.Column<double>(type: "double precision", nullable: true),
                    IpLongitude = table.Column<double>(type: "double precision", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    BrowserLanguage = table.Column<string>(type: "text", nullable: true),
                    Referrer = table.Column<string>(type: "text", nullable: true),
                    SourcePage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AdminNotes = table.Column<string>(type: "text", nullable: true),
                    SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusChangedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageBookings_Trekking_TrekkingId",
                        column: x => x.TrekkingId,
                        principalTable: "Trekking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageBookings_Reference",
                table: "PackageBookings",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageBookings_Status",
                table: "PackageBookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PackageBookings_SubmittedAtUtc",
                table: "PackageBookings",
                column: "SubmittedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_PackageBookings_TrekkingId",
                table: "PackageBookings",
                column: "TrekkingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageBookings");
        }
    }
}
