import { prisma } from "@/core/infrastructure/database/prisma";
import { PageHeader } from "@/components/admin/page-header";

export default async function AdminDashboard() {
  const [treks, inquiries, reviews, blogs] = await Promise.all([prisma.trekking.count(), prisma.inquiry.count(), prisma.trekkingReview.count({ where: { moderationStatus: "Pending" } }), prisma.blogPost.count()]);
  return <div className="space-y-8"><PageHeader title="Dashboard" description="Operational overview migrated from the legacy admin panel." /><div className="grid gap-4 md:grid-cols-4">{[["Treks", treks], ["Inquiries", inquiries], ["Pending reviews", reviews], ["Blogs", blogs]].map(([label, value]) => <div key={label} className="rounded-2xl border bg-white p-6"><div className="text-sm text-muted-foreground">{label}</div><div className="mt-2 text-3xl font-black">{value}</div></div>)}</div></div>;
}
