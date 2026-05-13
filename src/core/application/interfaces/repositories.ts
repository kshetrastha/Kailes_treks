import type { Prisma, Trekking } from "@prisma/client";
import type { ListQuery, PaginatedResult } from "@/core/application/dto/pagination";

export interface ITrekkingRepository {
  list(query: ListQuery): Promise<PaginatedResult<Trekking>>;
  findBySlug(slug: string): Promise<Trekking | null>;
  findById(id: number): Promise<Trekking | null>;
  create(data: Prisma.TrekkingCreateInput): Promise<Trekking>;
  update(id: number, data: Prisma.TrekkingUpdateInput): Promise<Trekking>;
  softDelete(id: number): Promise<Trekking>;
  publish(id: number, published: boolean): Promise<Trekking>;
}
