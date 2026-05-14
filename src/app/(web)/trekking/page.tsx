import Link from "next/link";
import { trekkingUseCases } from "@/core/application/use-cases/trekking.use-cases";
import { formatMoney } from "@/lib/utils";

export default async function TrekkingList({ searchParams }: { searchParams: Promise<{ q?: string; page?: string }> }) {
  const params = await searchParams;
  const result = await trekkingUseCases.listPublic({ q: params.q, page: Number(params.page ?? 1), pageSize: 12 });
  return <section className="mx-auto max-w-7xl px-4 py-12"><h1 className="text-4xl font-black">Trekking packages</h1><form className="mt-6"><input name="q" defaultValue={params.q} placeholder="Search treks, regions, destinations" className="w-full rounded-xl border px-4 py-3" /></form><div className="mt-8 grid gap-6 md:grid-cols-3">{result.data.map((trek) => <Link href={`/trekking/${trek.slug}`} key={trek.id} className="rounded-2xl border bg-white p-6 hover:shadow-lg"><div className="text-sm font-semibold text-primary">{trek.destination}</div><h2 className="mt-2 text-xl font-bold">{trek.name}</h2><p className="mt-3 line-clamp-3 text-sm text-muted-foreground">{trek.shortDescription}</p><div className="mt-4 text-sm font-semibold">{trek.durationDays} days · {formatMoney(trek.price, trek.currencyCode ?? "USD")}</div></Link>)}</div></section>;
}
