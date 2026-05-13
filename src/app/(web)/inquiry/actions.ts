"use server";
import { redirect } from "next/navigation";
import { prisma } from "@/core/infrastructure/database/prisma";
import { inquirySchema } from "@/core/application/validators/travel";

export async function submitInquiry(formData: FormData) {
  const data = inquirySchema.parse(Object.fromEntries(formData));
  await prisma.inquiry.create({ data });
  redirect("/inquiry?sent=1");
}
