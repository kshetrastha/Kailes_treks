export const ADMIN_ROLES = ["SUPER_ADMIN", "ADMIN", "EDITOR"] as const;
export type AdminRole = (typeof ADMIN_ROLES)[number];

export const travelStatuses = ["Draft", "Published", "Archived"] as const;
export type TravelStatus = (typeof travelStatuses)[number];
