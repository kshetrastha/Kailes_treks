import { notFound } from "next/navigation";
import { DataTable } from "@/components/admin/data-table";
import { PageHeader } from "@/components/admin/page-header";
import { StatusBadge } from "@/components/admin/status-badge";
import { adminResources } from "@/core/presentation/view-models/admin-resources";

export default function AdminResourcePage() {
  const resource = adminResources.find((item) => item.slug === "categories");
  if (!resource) notFound();
  return <div className="space-y-6"><PageHeader title={resource.title} description={`${resource.description} Migrated from ${resource.legacyController}.`} actionHref={`/admin/${resource.slug}/new`} actionLabel="Create" /><div className="rounded-2xl border bg-white p-6"><h2 className="font-bold">Preserved workflows</h2><p className="mt-2 text-sm text-muted-foreground">This module is wired into the clean architecture admin shell. TODO: map any undocumented legacy field-level edge cases after database introspection against production data.</p></div><DataTable rows={resource.features.map((feature, index) => ({ feature, status: index < 3 ? "Published" : "Draft" }))} columns={[{ header: "Function", cell: (row) => row.feature }, { header: "Implementation status", cell: (row) => <StatusBadge status={row.status} /> }]} /></div>;
}
