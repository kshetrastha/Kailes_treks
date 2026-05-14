import { submitInquiry } from "./actions";
import { FormInput, FormTextarea } from "@/components/shared/form-controls";
import { Button } from "@/components/ui/button";

export default async function InquiryPage({ searchParams }: { searchParams: Promise<{ travelSlug?: string; sent?: string }> }) {
  const params = await searchParams;
  return <section className="mx-auto max-w-3xl px-4 py-12"><h1 className="text-4xl font-black">Inquiry / booking</h1>{params.sent ? <div className="mt-6 rounded-xl bg-green-50 p-4 text-green-700">Thanks. We received your inquiry.</div> : null}<form action={submitInquiry} className="mt-8 grid gap-5 rounded-2xl border p-6"><FormInput label="Name" name="name" required /><FormInput label="Email" name="email" type="email" required /><FormInput label="Phone" name="phone" /><FormInput label="Trip slug" name="travelSlug" defaultValue={params.travelSlug ?? ""} /><FormInput label="Preferred date" name="preferredDate" type="date" /><FormInput label="People" name="people" type="number" min={1} /><FormTextarea label="Message" name="message" required /><Button>Submit inquiry</Button></form></section>;
}
