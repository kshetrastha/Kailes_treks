using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_serviceid_to_service_region_faq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                schema: "Service",
                table: "ServiceRegionFAQ",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE \"Service\".\"ServiceRegionFAQ\" faq
                SET \"ServiceId\" = sr.\"ServiceTypeId\"
                FROM \"Service\".\"ServiceRegions\" sr
                WHERE sr.\"Id\" = faq.\"ServiceRegionId\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceId",
                schema: "Service",
                table: "ServiceRegionFAQ");
        }
    }
}
