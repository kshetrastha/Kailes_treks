import Link from "next/link";

const links = [
  ["Destinations", "/destinations"], ["Treks", "/trekking"], ["Journal", "/blog"], ["Gallery", "/gallery"], ["About", "/about"], ["Contact", "/contact"]
];

export function SiteHeader() {
  return <header className="sticky top-0 z-30 border-b bg-white/90 backdrop-blur"><div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4"><Link href="/" className="text-xl font-black tracking-tight">Kailes Treks</Link><nav className="hidden gap-6 text-sm font-medium md:flex">{links.map(([label, href]) => <Link key={href} href={href} className="hover:text-primary">{label}</Link>)}</nav><Link href="/inquiry" className="rounded-full bg-primary px-4 py-2 text-sm font-semibold text-white">Plan a trip</Link></div></header>;
}
