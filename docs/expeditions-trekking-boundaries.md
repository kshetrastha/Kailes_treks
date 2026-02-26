# Expeditions vs Trekking Boundaries

This document clarifies strict module separation and field ownership.

## Shared/Common fields
- name, slug, destination, region, duration, max altitude, difficulty
- best season, overview, itinerary, inclusions, exclusions
- hero image, media gallery, FAQ
- permits, group size, price, dates, booking CTA
- SEO title/description, status, featured, ordering, audit timestamps/users

## Expedition-only fields
- summitRoute
- requiresClimbingPermit
- expeditionStyle
- oxygenSupport
- sherpaSupport
- summitBonusUsd

## Trekking-only fields
- trailGrade
- teaHouseAvailable
- accommodationType
- meals
- transportMode
- trekPermitType

## Separation rules in code
- Separate tables: `expeditions` and `trekking` (+ separate child tables)
- Separate services/controllers/routes/contracts
- No shared generic `trip` table/entity

## Note about reference pages
The provided sevensummittreks detail pages were used as domain references conceptually. Direct fetch from this environment was blocked by remote 403, so fields were modeled from common expedition/trekking detail patterns and can be adjusted after final content extraction.
