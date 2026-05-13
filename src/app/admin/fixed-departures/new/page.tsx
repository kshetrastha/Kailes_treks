import { PageHeader } from "@/components/admin/page-header";
import { FormInput, FormTextarea } from "@/components/shared/form-controls";
import { Button } from "@/components/ui/button";
export default function NewResourcePage() { return <div className="space-y-6"><PageHeader title="Create fixed departures" /><form className="grid gap-5 rounded-2xl border bg-white p-6"><FormInput label="Title / name" name="title" /><FormTextarea label="Description / content" name="description" /><FormInput label="Ordering" name="ordering" type="number" defaultValue={0} /><Button type="button">Save TODO</Button></form></div>; }
