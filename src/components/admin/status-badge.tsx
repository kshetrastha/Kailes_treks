import { cn } from "@/lib/utils";

export function StatusBadge({ status }: { status?: string | null }) {
  const value = status ?? "Draft";
  return <span className={cn("rounded-full px-2.5 py-1 text-xs font-semibold", value === "Published" ? "bg-green-100 text-green-700" : value === "Archived" ? "bg-slate-200 text-slate-700" : "bg-amber-100 text-amber-700")}>{value}</span>;
}
