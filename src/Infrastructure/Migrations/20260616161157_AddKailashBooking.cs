using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKailashBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kailash_yatra_packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    AvailableMonths = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullMoonMonths = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Ordering = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kailash_yatra_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kailash_bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PackageId = table.Column<int>(type: "integer", nullable: false),
                    SelectedMonth = table.Column<int>(type: "integer", nullable: false),
                    ArrivalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DepartureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BookingAmount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GivenName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TelCountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TelAreaCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TelNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Occupation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PassportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlaceOfIssue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateOfIssue = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Insurance = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    EmergencyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmergencyRelationship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmergencyTelCountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    EmergencyTelAreaCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    EmergencyTelNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EmergencyMobile = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    HealthDeclaration = table.Column<string>(type: "text", nullable: false),
                    OtherHealthConcerns = table.Column<string>(type: "text", nullable: true),
                    SpecialRequests = table.Column<string>(type: "text", nullable: true),
                    NumberOfTravellers = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kailash_bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kailash_bookings_kailash_yatra_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "kailash_yatra_packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kailash_bookings_Email",
                table: "kailash_bookings",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_kailash_bookings_PackageId",
                table: "kailash_bookings",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_kailash_bookings_Status",
                table: "kailash_bookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_kailash_bookings_SubmittedAtUtc",
                table: "kailash_bookings",
                column: "SubmittedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_kailash_yatra_packages_Ordering",
                table: "kailash_yatra_packages",
                column: "Ordering");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kailash_bookings");

            migrationBuilder.DropTable(
                name: "kailash_yatra_packages");
        }
    }
}
