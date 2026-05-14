import type { Metadata } from "next";
import { notFound } from "next/navigation";
import { trekkingUseCases } from "@/core/application/use-cases/trekking.use-cases";
import { formatMoney } from "@/lib/utils";
import Link from "next/link";

export async function generateMetadata({ params }: { params: Promise<{ slug: string }> }): Promise<Metadata> {
  const { slug } = await params;
  const trek = await trekkingUseCases.details(slug);
  return { title: trek?.seoTitle ?? trek?.name, description: trek?.seoDescription ?? trek?.shortDescription };
}

export default async function TrekkingDetails({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;
  const trek = await trekkingUseCases.details(slug);
  if (!trek || trek.status !== "Published") notFound();
  return <article><section className="bg-slate-950 text-white"><div className="mx-auto max-w-7xl px-4 py-20"><p className="text-primary">{trek.destination} {trek.region ? `· ${trek.region}` : ""}</p><h1 className="mt-3 max-w-4xl text-5xl font-black">{trek.name}</h1><p className="mt-5 max-w-2xl text-slate-300">{trek.shortDescription}</p><div className="mt-8 grid gap-4 md:grid-cols-4"><Fact label="Duration" value={`${trek.durationDays} days`} /><Fact label="Altitude" value={`${trek.maxAltitudeMeters}m`} /><Fact label="Difficulty" value={trek.difficultyLevel ?? "Custom"} /><Fact label="Price" value={formatMoney(trek.price, trek.currencyCode ?? "USD")} /></div></div></section><section className="mx-auto grid max-w-7xl gap-10 px-4 py-12 lg:grid-cols-[1fr_320px]"><div className="prose max-w-none"><h2>Overview</h2><p>{trek.overview}</p><h2>Highlights</h2><ul>{trek.highlights.map((h) => <li key={h.id}>{h.text}</li>)}</ul><h2>Itinerary</h2>{trek.itineraries.map((itinerary) => <div key={itinerary.id}><h3>{itinerary.seasonTitle}</h3>{itinerary.days.map((day) => <div key={day.id} className="rounded-xl border p-4"><strong>Day {day.dayNumber}: {day.shortDescription}</strong><p>{day.description}</p></div>)}</div>)}<h2>FAQs</h2>{trek.faqs.map((faq) => <details key={faq.id} className="rounded-xl border p-4"><summary className="font-semibold">{faq.question}</summary><p>{faq.answer}</p></details>)}</div><aside className="h-fit rounded-2xl border p-6"><h2 className="text-xl font-bold">Book this trip</h2><p className="mt-2 text-sm text-muted-foreground">Send an inquiry and the team will confirm availability.</p><Link href={`/inquiry?travelSlug=${trek.slug}`} className="mt-5 block rounded-xl bg-primary px-4 py-3 text-center font-semibold text-white">Inquiry / booking</Link></aside></section></article>;
}
function Fact({ label, value }: { label: string; value: string }) { return <div className="rounded-2xl bg-white/10 p-4"><div className="text-xs uppercase text-slate-400">{label}</div><div className="mt-1 font-bold">{value}</div></div>; }
