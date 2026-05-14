import Link from "next/link";
import { redirect } from "next/navigation";
import { getServerSession } from "next-auth";
import { authOptions } from "@/lib/auth";
import { ADMIN_ROLES } from "@/core/domain/enums/travel";

const nav = [
  ["Dashboard", "/admin"], ["Treks", "/admin/trekking"], ["Expeditions", "/admin/expeditions"], ["Regions", "/admin/service-regions"], ["Types", "/admin/trekking-types"], ["FAQs", "/admin/faqs"], ["Media", "/admin/media"], ["Blogs", "/admin/blogs"], ["Reviews", "/admin/reviews"], ["Users", "/admin/users"], ["Settings", "/admin/settings"]
];

export default async function AdminLayout({ children }: { children: React.ReactNode }) {
  const session = await getServerSession(authOptions);
  if (!session?.user || !ADMIN_ROLES.includes(session.user.role as never)) redirect("/auth/login");
  return <div className="min-h-screen bg-slate-100"><aside className="fixed inset-y-0 left-0 hidden w-72 border-r bg-slate-950 p-6 text-white lg:block"><Link href="/admin" className="text-2xl font-black">Kailes Admin</Link><nav className="mt-8 grid gap-1">{nav.map(([label, href]) => <Link className="rounded-lg px-3 py-2 text-sm text-slate-200 hover:bg-white/10" key={href} href={href}>{label}</Link>)}</nav></aside><main className="lg:pl-72"><div className="mx-auto max-w-7xl p-6 lg:p-10">{children}</div></main></div>;
}
