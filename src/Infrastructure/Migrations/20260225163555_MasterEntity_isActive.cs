using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MasterEntity_isActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WhyWithUsHeroes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WhyWithUs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WhoWeAreImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WhoWeAreHeroes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WhoWeAre",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingMedia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingItineraryDays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingItineraries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingHighlights",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingGearLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingFixedDepartures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TrekkingFaqs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Trekking",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TermsAndConditions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TeamMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Master",
                table: "ServiceTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patrons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ItineraryDays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Itineraries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GearLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FixedDepartures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Expeditions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionReviewItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionOverviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionMedia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionItineraries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionInclusionExclusions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionHighlights",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionGears",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionFixedDepartures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionFaqs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionFaqItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpeditionBasicInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Master",
                table: "DifficultyLevel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChairmanMessages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CertificateDocuments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Master",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlogPosts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Awards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Master",
                table: "Accomodation",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WhyWithUsHeroes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WhyWithUs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WhoWeAreImages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WhoWeAreHeroes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WhoWeAre");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingReviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingMedia");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingMaps");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingItineraryDays");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingItineraries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingHighlights");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingGearLists");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingFixedDepartures");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TrekkingFaqs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Trekking");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TermsAndConditions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Master",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ItineraryDays");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Itineraries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GearLists");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FixedDepartures");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Expeditions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionReviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionReviewItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionOverviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionMedia");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionMaps");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionItineraries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionInclusionExclusions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionHighlights");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionGears");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionFixedDepartures");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionFaqs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionFaqItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpeditionBasicInfos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Master",
                table: "DifficultyLevel");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChairmanMessages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CertificateDocuments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Master",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Awards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Master",
                table: "Accomodation");
        }
    }
}
