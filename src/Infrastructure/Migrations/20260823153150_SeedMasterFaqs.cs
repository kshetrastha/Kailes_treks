using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMasterFaqs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seeded only when a question with the same text is not already present,
            // so existing FAQs curated in the admin are never duplicated or overwritten.
            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'What is Kailash Mansarovar Trek?', 'The Kailash Mansarovar Trek is a sacred pilgrimage to Mount Kailash and Mansarovar Lake in Tibet, revered by multiple religions.', 1, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'What is Kailash Mansarovar Trek?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'How difficult is the trek?', 'It is a moderate to challenging trek due to high altitude and rugged terrain. Proper fitness and acclimatization are required.', 2, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'How difficult is the trek?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'What is the best time to visit?', 'The best time to visit is from May to September when weather conditions are favorable.', 3, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'What is the best time to visit?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'Do I need a permit?', 'Yes, special permits are required to enter Tibet, which are arranged by authorized travel agencies.', 4, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'Do I need a permit?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'How long does the trek take?', 'The full journey usually takes around 10-15 days depending on the itinerary.', 5, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'How long does the trek take?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'Is altitude sickness common?', 'Yes, due to high altitude. Proper acclimatization, hydration, and slow ascent are important.', 6, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'Is altitude sickness common?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'What is Kailash Parikrama?', 'It is a 52 km spiritual walk around Mount Kailash completed in 3 days by pilgrims.', 7, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'What is Kailash Parikrama?');");

            migrationBuilder.Sql(@"INSERT INTO ""master_faqs"" (""Question"", ""Answer"", ""Ordering"", ""IsPublished"", ""CreatedAtUtc"", ""UpdatedAtUtc"", ""IsActive"") SELECT 'What should I pack?', 'Pack warm clothes, trekking shoes, personal medicines, sunscreen, and essential travel documents.', 8, TRUE, TIMESTAMPTZ '2026-08-23 00:00:00+00', TIMESTAMPTZ '2026-08-23 00:00:00+00', TRUE WHERE NOT EXISTS (SELECT 1 FROM ""master_faqs"" WHERE ""Question"" = 'What should I pack?');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""master_faqs"" WHERE ""Question"" IN ('What is Kailash Mansarovar Trek?', 'How difficult is the trek?', 'What is the best time to visit?', 'Do I need a permit?', 'How long does the trek take?', 'Is altitude sickness common?', 'What is Kailash Parikrama?', 'What should I pack?');");
        }
    }
}
