import Link from "next/link";
import { prisma } from "@/core/infrastructure/database/prisma";
import { formatMoney } from "@/lib/utils";

export default async function HomePage() {
  const [banners, treks, blogs] = await Promise.all([
    prisma.banner.findMany({ where: { isPublished: true, isActive: true }, orderBy: { ordering: "asc" }, take: 3 }),
    prisma.trekking.findMany({ where: { status: "Published", isActive: true, featured: true }, orderBy: { ordering: "asc" }, take: 6 }),
    prisma.blogPost.findMany({ where: { isPublished: true, isActive: true }, orderBy: [{ isFeatured: "desc" }, { publishedOnUtc: "desc" }], take: 3 })
  ]);
  return <div><section className="bg-slate-950 text-white"><div className="mx-auto grid max-w-7xl gap-10 px-4 py-24 md:grid-cols-2"><div><p className="text-sm font-semibold uppercase tracking-[0.3em] text-orange-300">Himalayan journeys</p><h1 className="mt-4 text-5xl font-black tracking-tight md:text-7xl">Treks and expeditions crafted with expert care.</h1><p className="mt-6 max-w-xl text-lg text-slate-300">{banners[0]?.description ?? "Explore destination listings, package details, blogs, galleries, inquiry workflows and admin-managed content."}</p><div className="mt-8 flex gap-3"><Link href="/trekking" className="rounded-full bg-primary px-5 py-3 font-semibold">Explore treks</Link><Link href="/contact" className="rounded-full border border-white/30 px-5 py-3 font-semibold">Contact us</Link></div></div><div className="rounded-3xl bg-white/10 p-6"><h2 className="text-2xl font-bold">Featured packages</h2><div className="mt-6 grid gap-4">{treks.slice(0, 3).map((trek) => <Link key={trek.id} href={`/trekking/${trek.slug}`} className="rounded-2xl bg-white p-4 text-slate-950"><div className="font-bold">{trek.name}</div><div className="text-sm text-slate-600">{trek.durationDays} days · {formatMoney(trek.price, trek.currencyCode ?? "USD")}</div></Link>)}</div></div></div></section><section className="mx-auto max-w-7xl px-4 py-16"><h2 className="text-3xl font-bold">Latest journal</h2><div className="mt-8 grid gap-6 md:grid-cols-3">{blogs.map((blog) => <Link href={`/blog/${blog.slug}`} key={blog.id} className="rounded-2xl border p-6 hover:shadow-lg"><h3 className="font-bold">{blog.title}</h3><p className="mt-3 text-sm text-muted-foreground">{blog.summary}</p></Link>)}</div></section></div>;
}
