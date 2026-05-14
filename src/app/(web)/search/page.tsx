import { prisma } from "@/core/infrastructure/database/prisma";
export default async function Page() {
  const counts = await prisma.trekking.count({ where: { status: "Published", isActive: true } });
  return <section className="mx-auto max-w-5xl px-4 py-12"><h1 className="text-4xl font-black capitalize">search</h1><p className="mt-4 text-muted-foreground">Migrated public search screen. Content remains admin-managed and backed by PostgreSQL. Published packages available: {counts}.</p></section>;
}
