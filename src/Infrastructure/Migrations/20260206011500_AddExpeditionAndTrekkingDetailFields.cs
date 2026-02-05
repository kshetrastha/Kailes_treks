using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    public partial class AddExpeditionAndTrekkingDetailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "ExpeditionStyle", table: "expeditions", type: "character varying(120)", maxLength: 120, nullable: true);
            migrationBuilder.AddColumn<bool>(name: "OxygenSupport", table: "expeditions", type: "boolean", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "SherpaSupport", table: "expeditions", type: "boolean", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<decimal>(name: "SummitBonusUsd", table: "expeditions", type: "numeric(12,2)", nullable: true);

            migrationBuilder.AddColumn<string>(name: "AccommodationType", table: "trekking", type: "character varying(100)", maxLength: 100, nullable: true);
            migrationBuilder.AddColumn<string>(name: "Meals", table: "trekking", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(name: "TransportMode", table: "trekking", type: "character varying(100)", maxLength: 100, nullable: true);
            migrationBuilder.AddColumn<string>(name: "TrekPermitType", table: "trekking", type: "character varying(150)", maxLength: 150, nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ExpeditionStyle", table: "expeditions");
            migrationBuilder.DropColumn(name: "OxygenSupport", table: "expeditions");
            migrationBuilder.DropColumn(name: "SherpaSupport", table: "expeditions");
            migrationBuilder.DropColumn(name: "SummitBonusUsd", table: "expeditions");

            migrationBuilder.DropColumn(name: "AccommodationType", table: "trekking");
            migrationBuilder.DropColumn(name: "Meals", table: "trekking");
            migrationBuilder.DropColumn(name: "TransportMode", table: "trekking");
            migrationBuilder.DropColumn(name: "TrekPermitType", table: "trekking");
        }
    }
}
