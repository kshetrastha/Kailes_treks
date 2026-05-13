import { hash } from "bcryptjs";
import { prisma } from "../src/core/infrastructure/database/prisma";

async function main() {
  const adminEmail = process.env.ADMIN_EMAIL ?? "admin@kailestreks.local";
  const adminPassword = process.env.ADMIN_PASSWORD ?? "ChangeMe123!";
  await prisma.user.upsert({
    where: { email: adminEmail },
    update: { role: "SUPER_ADMIN", isActive: true },
    create: { email: adminEmail, fullName: "Super Admin", name: "Super Admin", role: "SUPER_ADMIN", passwordHash: await hash(adminPassword, 12) }
  });

  const trekkingType = await prisma.trekkingType.upsert({
    where: { id: 1 },
    update: {},
    create: { title: "Classic Nepal Treks", shortDescription: "Everest, Annapurna and iconic Himalayan routes.", description: "Seeded trekking type migrated from legacy master data.", ordering: 1 }
  });

  await prisma.trekking.upsert({
    where: { slug: "everest-base-camp-trek" },
    update: {},
    create: {
      trekkingTypeId: trekkingType.id,
      name: "Everest Base Camp Trek",
      slug: "everest-base-camp-trek",
      shortDescription: "A classic trek through Sherpa villages to the foot of the world's highest mountain.",
      destination: "Nepal",
      region: "Everest",
      durationDays: 14,
      maxAltitudeMeters: 5364,
      difficultyLevel: "Challenging",
      bestSeason: "Autumn",
      overview: "Follow the Dudh Koshi valley, visit Namche Bazaar and reach Everest Base Camp with experienced local guides.",
      minGroupSize: 1,
      maxGroupSize: 12,
      price: 1495,
      currencyCode: "USD",
      status: "Published",
      featured: true,
      ordering: 1,
      highlights: { create: [{ text: "Everest Base Camp and Kala Patthar viewpoints", sortOrder: 1 }, { text: "Namche Bazaar and Tengboche Monastery", sortOrder: 2 }] },
      faqs: { create: [{ question: "When is the best time?", answer: "Spring and autumn offer stable weather and clear mountain views.", ordering: 1 }] },
      itineraries: { create: [{ seasonTitle: "Standard itinerary", sortOrder: 1, days: { create: [{ dayNumber: 1, shortDescription: "Arrive in Kathmandu", description: "Airport pickup and trek briefing." }, { dayNumber: 2, shortDescription: "Fly to Lukla and trek to Phakding", description: "Begin the trail in the Everest region." }] } }] }
    }
  });

  await prisma.blogPost.upsert({
    where: { slug: "how-to-prepare-for-himalayan-trekking" },
    update: {},
    create: { title: "How to prepare for Himalayan trekking", slug: "how-to-prepare-for-himalayan-trekking", summary: "Training, packing and acclimatization guidance for first-time trekkers.", contentHtml: "<p>Start cardio training early, break in your boots and plan acclimatization days.</p>", isFeatured: true, isPublished: true, publishedOnUtc: new Date(), ordering: 1 }
  });

  await prisma.banner.upsert({
    where: { id: 1 },
    update: {},
    create: { title: "Explore the Himalayas", description: "Admin-managed hero banner content from PostgreSQL.", ordering: 1, isPublished: true }
  });
}

main().finally(async () => prisma.$disconnect());
