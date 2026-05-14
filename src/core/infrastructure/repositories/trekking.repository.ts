import type { Prisma } from "@prisma/client";
import { prisma } from "@/core/infrastructure/database/prisma";
import type { ListQuery } from "@/core/application/dto/pagination";
import type { ITrekkingRepository } from "@/core/application/interfaces/repositories";

export class PrismaTrekkingRepository implements ITrekkingRepository {
  async list(query: ListQuery) {
    const page = Math.max(1, query.page ?? 1);
    const pageSize = Math.min(100, Math.max(1, query.pageSize ?? 12));
    const where: Prisma.TrekkingWhereInput = {
      isActive: true,
      ...(query.q
        ? { OR: [{ name: { contains: query.q, mode: "insensitive" } }, { destination: { contains: query.q, mode: "insensitive" } }, { region: { contains: query.q, mode: "insensitive" } }] }
        : {}),
      ...(query.status ? { status: query.status as Prisma.EnumTravelStatusFilter["equals"] } : {})
    };
    const [data, total] = await prisma.$transaction([
      prisma.trekking.findMany({ where, skip: (page - 1) * pageSize, take: pageSize, orderBy: [{ ordering: "asc" }, { updatedAtUtc: "desc" }], include: { trekkingType: true } }),
      prisma.trekking.count({ where })
    ]);
    return { data, total, page, pageSize, pageCount: Math.ceil(total / pageSize) };
  }

  findBySlug(slug: string) {
    return prisma.trekking.findFirst({ where: { slug, isActive: true }, include: { trekkingType: true, faqs: { orderBy: { ordering: "asc" } }, mediaItems: { orderBy: { ordering: "asc" } }, itineraries: { orderBy: { sortOrder: "asc" }, include: { days: { orderBy: { dayNumber: "asc" } } } }, costItems: { orderBy: { sortOrder: "asc" } }, fixedDepartures: { orderBy: { startDate: "asc" } }, gearLists: true, highlights: { orderBy: { sortOrder: "asc" } }, reviews: true, maps: true } });
  }

  findById(id: number) {
    return prisma.trekking.findUnique({ where: { id } });
  }

  create(data: Prisma.TrekkingCreateInput) {
    return prisma.trekking.create({ data });
  }

  update(id: number, data: Prisma.TrekkingUpdateInput) {
    return prisma.trekking.update({ where: { id }, data });
  }

  softDelete(id: number) {
    return prisma.trekking.update({ where: { id }, data: { isActive: false } });
  }

  publish(id: number, published: boolean) {
    return prisma.trekking.update({ where: { id }, data: { status: published ? "Published" : "Draft" } });
  }
}
