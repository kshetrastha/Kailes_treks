import type { Prisma } from "@prisma/client";
import type { ListQuery } from "@/core/application/dto/pagination";
import type { ITrekkingRepository } from "@/core/application/interfaces/repositories";
import { PrismaTrekkingRepository } from "@/core/infrastructure/repositories/trekking.repository";
import { trekkingSchema } from "@/core/application/validators/travel";
import { slugify } from "@/lib/utils";

export class TrekkingUseCases {
  constructor(private readonly trekkings: ITrekkingRepository) {}

  listPublic(query: ListQuery) {
    return this.trekkings.list({ ...query, status: "Published" });
  }

  listAdmin(query: ListQuery) {
    return this.trekkings.list(query);
  }

  details(slug: string) {
    return this.trekkings.findBySlug(slug);
  }

  async save(formData: FormData, id?: number) {
    const parsed = trekkingSchema.parse(Object.fromEntries(formData));
    const data: Prisma.TrekkingUncheckedCreateInput = { ...parsed, slug: parsed.slug || slugify(parsed.name) } as Prisma.TrekkingUncheckedCreateInput;
    if (id) return this.trekkings.update(id, data);
    return this.trekkings.create(data as Prisma.TrekkingCreateInput);
  }

  delete(id: number) {
    return this.trekkings.softDelete(id);
  }

  publish(id: number, published: boolean) {
    return this.trekkings.publish(id, published);
  }
}

export const trekkingUseCases = new TrekkingUseCases(new PrismaTrekkingRepository());
