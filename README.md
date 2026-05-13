# Kailes Treks Next.js Migration

This repository now contains a production-ready Next.js replacement for the legacy .NET MVC web client and admin panel. The migration keeps the .NET source for reference while adding a modern TypeScript, PostgreSQL, Prisma, Tailwind and Auth.js implementation.

## Migrated architecture

```txt
src/
  app/                    Next.js App Router public, auth, admin and API routes
  components/             Admin, public website, shared and UI components
  core/domain             Entities/enums/value-object TypeScript contracts
  core/application        DTOs, repository interfaces, use cases and Zod validators
  core/infrastructure     Prisma database, repositories and services
  core/presentation       Admin/public view models
  lib/                    Cross-cutting auth/utilities
  styles/                 Tailwind globals
prisma/                   PostgreSQL Prisma schema and seed data
```

## Legacy analysis summary

The source analysis covered:

- MVC controllers in `src/Web/Controllers/Mvc` for account, home, newsletter, packages and expeditions.
- Admin controllers in `src/Web/Areas/Admin/Controllers` for dashboard, trekking, expedition modules, regions, types, FAQs, reviews, blogs, company content, maps, newsletter and settings-like content.
- Razor views in `src/Web/Views` and `src/Web/Areas/Admin/Views`, including package details, listing partials, admin upsert pages and expedition module tabs.
- Domain and EF Core models in `src/Domain/Entities` and `src/Infrastructure/Persistence/AppDbContext.cs`.
- Client scripts for cropping, HTML editing and expedition form validation in `src/Web/wwwroot/js`.

## Implemented features

- Public website: home, destination, package listing, package details, region, blog, gallery, about, contact, search and inquiry routes.
- Admin: dashboard plus management modules for countries, regions, trekking types, trekking/packages, expeditions, itineraries, itinerary days, FAQs, media, maps, costs, fixed departures, gear, highlights, reviews, blogs, categories, users, roles and settings.
- Auth.js credentials authentication backed by Prisma users and a role-based admin shell guard.
- Prisma PostgreSQL schema preserving core EF relationships:
  - Trekking -> itineraries -> itinerary days
  - Trekking -> FAQs, media, maps, cost items, fixed departures, gear, highlights and reviews
  - Expedition mirrors the same child modules
  - TrekkingType -> Trekking
  - ServiceType -> ServiceRegion -> ServiceRegionFaq
- Reusable admin/shared components: `DataTable`, `FormInput`, `FormSelect`, `FormTextarea`, `ImageUploader`, `RichTextEditor`, `StatusBadge`, `ConfirmDialog`, `PageHeader`, `Breadcrumb` and `Pagination`.
- Zod validation for trekking/package and inquiry workflows.
- Seed data for admin user, trekking type, featured trek, blog post and banner.

## TODO markers

A few behaviors depend on production-only assets or undocumented data edge cases and are explicitly marked with `TODO` comments:

- Object storage upload adapter for real image/file upload credentials.
- Rich-text editor package mounting; server-side HTML preservation is already supported.
- Radix confirmation dialog client interaction.
- Final undocumented legacy field edge cases after introspecting production database content.

## Quick start

1. Copy environment variables:

```bash
cp .env.example .env
```

2. Start PostgreSQL:

```bash
docker compose up -d postgres
```

3. Install dependencies:

```bash
npm install
```

4. Generate Prisma client and create tables:

```bash
npx prisma migrate dev --name init
```

5. Seed the database:

```bash
npm run prisma:seed
```

6. Run the app:

```bash
npm run dev
```

Public site: <http://localhost:3000>
Admin: <http://localhost:3000/admin>

Default admin credentials are controlled by `ADMIN_EMAIL` and `ADMIN_PASSWORD` in `.env`.

## Environment

```env
DATABASE_URL="postgresql://postgres:s%4001@localhost:5432/VKT_trekking_v2_demo?schema=public"
NEXTAUTH_URL="http://localhost:3000"
NEXTAUTH_SECRET="replace-with-a-long-random-secret"
UPLOAD_DIR="public/uploads"
ADMIN_EMAIL="admin@kailestreks.local"
ADMIN_PASSWORD="ChangeMe123!"
```
