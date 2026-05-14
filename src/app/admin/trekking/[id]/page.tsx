import { notFound } from "next/navigation";
import { PageHeader } from "@/components/admin/page-header";
import { TrekkingForm } from "@/components/admin/trekking-form";
import { prisma } from "@/core/infrastructure/database/prisma";
export default async function EditTrekkingPage({ params }: { params: Promise<{ id: string }> }) { const { id } = await params; const trek = await prisma.trekking.findUnique({ where: { id: Number(id) } }); if (!trek) notFound(); return <div className="space-y-6"><PageHeader title={`Edit ${trek.name}`} description="Use child admin modules for itinerary days, gallery, maps, cost, fixed departures, gear, highlights and reviews." /><TrekkingForm trek={trek} /></div>; }
