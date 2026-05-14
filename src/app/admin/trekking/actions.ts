"use server";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { trekkingUseCases } from "@/core/application/use-cases/trekking.use-cases";

export async function saveTrekking(formData: FormData) {
  const id = formData.get("id") ? Number(formData.get("id")) : undefined;
  await trekkingUseCases.save(formData, id);
  revalidatePath("/admin/trekking");
  redirect("/admin/trekking");
}

export async function deleteTrekking(formData: FormData) {
  await trekkingUseCases.delete(Number(formData.get("id")));
  revalidatePath("/admin/trekking");
}

export async function publishTrekking(formData: FormData) {
  await trekkingUseCases.publish(Number(formData.get("id")), formData.get("published") === "true");
  revalidatePath("/admin/trekking");
}
