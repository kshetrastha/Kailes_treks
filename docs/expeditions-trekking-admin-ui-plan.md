# Admin UI Integration Plan (for separate front-end/admin app)

This backend repository does not contain a web admin UI project. To preserve module separation, implement the UI in the existing front-end/admin codebase using these distinct route groups and components.

## Admin Routes
- `/admin/expeditions`
- `/admin/expeditions/create`
- `/admin/expeditions/:id/edit`
- `/admin/trekking`
- `/admin/trekking/create`
- `/admin/trekking/:id/edit`

## Component Trees

### Expeditions
- `AdminExpeditionsPage`
  - `ExpeditionsFilters`
  - `ExpeditionsTable`
  - `ExpeditionsBulkActions`
- `ExpeditionFormPage`
  - Tabs:
    - `ExpeditionBasicTab`
    - `ExpeditionLogisticsTab`
    - `ExpeditionItineraryTab`
    - `ExpeditionPricingDatesTab`
    - `ExpeditionInclusionsExclusionsTab`
    - `ExpeditionFaqTab`
    - `ExpeditionMediaTab`
    - `ExpeditionSeoTab`
    - `ExpeditionPublishTab`

### Trekking
- `AdminTrekkingPage`
  - `TrekkingFilters`
  - `TrekkingTable`
  - `TrekkingBulkActions`
- `TrekkingFormPage`
  - Tabs:
    - `TrekkingBasicTab`
    - `TrekkingLogisticsTab`
    - `TrekkingItineraryTab`
    - `TrekkingPricingDatesTab`
    - `TrekkingInclusionsExclusionsTab`
    - `TrekkingFaqTab`
    - `TrekkingMediaTab`
    - `TrekkingSeoTab`
    - `TrekkingPublishTab`

## Validation Schema Names
- `expeditionFormSchema`
- `trekkingFormSchema`

## Menu Structure
- Admin
  - Expeditions
  - Trekking

## Backend API Bindings
- Expeditions admin list/create/update/delete -> `/api/admin/expeditions`
- Trekking admin list/create/update/delete -> `/api/admin/trekking`
- Public expedition detail -> `/api/expeditions/{slug}`
- Public trekking detail -> `/api/trekking/{slug}`


## Field ownership for forms
- Expedition-specific fields: `summitRoute`, `requiresClimbingPermit`, `expeditionStyle`, `oxygenSupport`, `sherpaSupport`, `summitBonusUsd`
- Trekking-specific fields: `trailGrade`, `teaHouseAvailable`, `accommodationType`, `meals`, `transportMode`, `trekPermitType`
