import { Button } from "@/components/ui/button";

export function ConfirmDialog({ label = "Delete" }: { label?: string }) {
  // TODO: Wire Radix AlertDialog for client-side confirmation; server action remains protected.
  return <Button variant="destructive" type="submit">{label}</Button>;
}
