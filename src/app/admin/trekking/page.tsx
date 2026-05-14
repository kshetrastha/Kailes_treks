import Link from "next/link";
import { DataTable } from "@/components/admin/data-table";
import { PageHeader } from "@/components/admin/page-header";
import { StatusBadge } from "@/components/admin/status-badge";
import { ConfirmDialog } from "@/components/shared/confirm-dialog";
import { deleteTrekking, publishTrekking } from "./actions";
import { trekkingUseCases } from "@/core/application/use-cases/trekking.use-cases";

export default async function AdminTrekkingPage({ searchParams }: { searchParams: Promise<{ q?: string; page?: string }> }) {
  const params = await searchParams;
  const result = await trekkingUseCases.listAdmin({ q: params.q, page: Number(params.page ?? 1), pageSize: 20 });
  return <div className="space-y-6"><PageHeader title="Trekking / package management" description="Listing, details, create, edit, delete, publish/unpublish, SEO, sorting, search and pagination." actionHref="/admin/trekking/new" actionLabel="Create trek" /><form><input name="q" defaultValue={params.q ?? ""} className="w-full rounded-xl border bg-white px-4 py-3" placeholder="Search packages" /></form><DataTable rows={result.data} columns={[{ header: "Name", cell: (row) => <Link className="font-semibold text-primary" href={`/admin/trekking/${row.id}`}>{row.name}</Link> }, { header: "Destination", cell: (row) => row.destination }, { header: "Status", cell: (row) => <StatusBadge status={row.status} /> }, { header: "Actions", cell: (row) => <div className="flex gap-2"><form action={publishTrekking}><input type="hidden" name="id" value={row.id} /><input type="hidden" name="published" value={row.status === "Published" ? "false" : "true"} /><button className="rounded-md border px-3 py-1 text-xs">{row.status === "Published" ? "Unpublish" : "Publish"}</button></form><form action={deleteTrekking}><input type="hidden" name="id" value={row.id} /><ConfirmDialog /></form></div> }]} /></div>;
}
