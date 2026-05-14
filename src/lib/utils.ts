import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function slugify(value: string) {
  return value.toLowerCase().trim().replace(/[^a-z0-9]+/g, "-").replace(/(^-|-$)+/g, "");
}

export function formatMoney(value?: number | string | null, currency = "USD") {
  if (value === null || value === undefined || value === "") return "On request";
  return new Intl.NumberFormat("en-US", { style: "currency", currency }).format(Number(value));
}
