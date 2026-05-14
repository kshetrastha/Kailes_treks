export type AdminResource = {
  slug: string;
  title: string;
  description: string;
  legacyController: string;
  features: string[];
};

export const adminResources: AdminResource[] = [
  { slug: "countries", title: "Country management", legacyController: "TrekkingType Country enum / ServiceRegion", description: "Manage countries used by trek, expedition and regional content.", features: ["listing", "details", "create", "edit", "delete", "publish/unpublish", "ordering"] },
  { slug: "service-regions", title: "Region management", legacyController: "ServiceRegionsController", description: "Destination/region pages, FAQs and ordering.", features: ["search/filter/pagination", "SEO metadata", "region FAQs", "publish/unpublish"] },
  { slug: "trekking-types", title: "Trekking type management", legacyController: "TrekkingTypesPageController", description: "Trekking type landing pages and gallery images.", features: ["image upload", "gallery management", "sorting", "status badges"] },
  { slug: "expeditions", title: "Expedition management", legacyController: "ExpeditionsController", description: "Expedition packages with itinerary, media, maps and departures.", features: ["listing", "details", "create", "edit", "delete", "publish/unpublish"] },
  { slug: "itineraries", title: "Itinerary management", legacyController: "ItinerariesController", description: "Seasonal itinerary groups for treks and expeditions.", features: ["ordering/sorting", "nested days", "validation"] },
  { slug: "itinerary-days", title: "Itinerary days", legacyController: "ItineraryDaysController", description: "Daily trip plans, meals and accommodation.", features: ["create", "edit", "delete", "sorting"] },
  { slug: "faqs", title: "FAQ management", legacyController: "MasterFaqsController / ServiceRegionFaqsController", description: "Master, region, trekking and expedition FAQs.", features: ["publish/unpublish", "ordering", "search"] },
  { slug: "media", title: "Media / gallery management", legacyController: "ExpeditionModulesController media partials", description: "Images, videos, maps and documents.", features: ["image upload", "gallery management", "captions", "cover selection"] },
  { slug: "maps", title: "Map management", legacyController: "MapDestinationsController", description: "Destination maps, coordinates and map images.", features: ["coordinates", "image upload", "details"] },
  { slug: "cost-items", title: "Cost includes/excludes", legacyController: "ExpeditionModulesController inclusion partial", description: "Included and excluded cost lines.", features: ["include/exclude grouping", "sorting"] },
  { slug: "fixed-departures", title: "Fixed departure management", legacyController: "ExpeditionModulesController fixed departures partial", description: "Scheduled departure dates and availability states.", features: ["date validation", "status", "group size"] },
  { slug: "gear-lists", title: "Gear list management", legacyController: "ExpeditionModulesController gear partial", description: "Gear documents and image cards.", features: ["file upload", "image upload"] },
  { slug: "highlights", title: "Highlight management", legacyController: "ExpeditionModulesController overview partial", description: "Sortable selling points for detail pages.", features: ["ordering", "inline edit"] },
  { slug: "reviews", title: "Review management", legacyController: "ReviewsController", description: "Company and package reviews with moderation.", features: ["approve/reject", "rating", "video URL"] },
  { slug: "blogs", title: "Blog / journal management", legacyController: "BlogsController", description: "Journal posts, SEO excerpts and rich HTML content.", features: ["rich text", "SEO", "publish/unpublish"] },
  { slug: "categories", title: "Category management", legacyController: "CategoriesController", description: "Content/package categories.", features: ["listing", "create", "edit", "delete"] },
  { slug: "users", title: "User management", legacyController: "AccountController / IdentityService", description: "Users, active state and admin access.", features: ["roles", "permissions", "active/inactive"] },
  { slug: "roles", title: "Role / permission management", legacyController: "AppRoles", description: "SUPER_ADMIN, ADMIN, EDITOR, VIEWER and CUSTOMER permissions.", features: ["role-based authorization", "admin guards"] },
  { slug: "settings", title: "Settings management", legacyController: "appsettings / company pages", description: "Company, SEO and operational settings.", features: ["environment config", "site metadata"] }
];
