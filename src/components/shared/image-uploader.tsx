import { FormInput } from "@/components/shared/form-controls";

export function ImageUploader({ name = "heroImageUrl", label = "Image URL", defaultValue }: { name?: string; label?: string; defaultValue?: string | null }) {
  // TODO: Replace URL entry with object storage upload adapter when production bucket credentials are available.
  return <FormInput label={label} name={name} defaultValue={defaultValue ?? ""} placeholder="/uploads/hero.jpg or https://..." />;
}
