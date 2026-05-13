import { FormTextarea } from "@/components/shared/form-controls";

export function RichTextEditor({ name, label, defaultValue }: { name: string; label: string; defaultValue?: string | null }) {
  // TODO: Mount TipTap/Plate editor; textarea preserves existing HTML workflows server-side today.
  return <FormTextarea label={label} name={name} defaultValue={defaultValue ?? ""} className="min-h-52 font-mono" />;
}
