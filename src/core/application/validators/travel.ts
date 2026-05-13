import { z } from "zod";

const nullableNumber = z.coerce.number().optional().nullable();

export const trekkingSchema = z.object({
  name: z.string().min(2, "Name is required"),
  slug: z.string().min(2, "Slug is required"),
  shortDescription: z.string().min(10, "Short description must be at least 10 characters"),
  destination: z.string().min(2, "Destination is required"),
  trekkingTypeId: z.coerce.number().optional().nullable(),
  region: z.string().optional().nullable(),
  durationDays: z.coerce.number().int().min(1, "Duration must be at least 1 day"),
  maxAltitudeMeters: z.coerce.number().int().min(0),
  difficultyLevel: z.enum(["Easy", "Moderate", "Challenging", "Difficult", "Extreme"]).optional().nullable(),
  bestSeason: z.enum(["Spring", "Summer", "Autumn", "Winter", "AllYear"]).optional().nullable(),
  overview: z.string().optional().nullable(),
  heroImageUrl: z.string().optional().nullable(),
  priceOnRequest: z.coerce.boolean().default(false),
  price: nullableNumber,
  currencyCode: z.string().default("USD"),
  seoTitle: z.string().optional().nullable(),
  seoDescription: z.string().optional().nullable(),
  status: z.enum(["Draft", "Published", "Archived"]).default("Draft"),
  featured: z.coerce.boolean().default(false),
  ordering: z.coerce.number().int().default(0)
});

export const inquirySchema = z.object({
  name: z.string().min(2, "Name is required"),
  email: z.string().email("Enter a valid email"),
  phone: z.string().optional(),
  travelSlug: z.string().optional(),
  preferredDate: z.coerce.date().optional(),
  people: z.coerce.number().int().positive().optional(),
  message: z.string().min(10, "Please provide trip details")
});

export const contentSchema = z.object({
  title: z.string().min(2, "Title is required"),
  slug: z.string().optional(),
  summary: z.string().optional(),
  contentHtml: z.string().min(1, "Content is required"),
  isPublished: z.coerce.boolean().default(true),
  ordering: z.coerce.number().int().default(0)
});
