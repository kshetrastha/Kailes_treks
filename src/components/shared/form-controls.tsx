import * as React from "react";
import { cn } from "@/lib/utils";

export function Field({ label, error, children }: { label: string; error?: string; children: React.ReactNode }) {
  return <label className="grid gap-2 text-sm font-medium"><span>{label}</span>{children}{error ? <span className="text-xs text-red-600">{error}</span> : null}</label>;
}

export const inputClass = "rounded-md border border-input bg-background px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-ring";

export function FormInput(props: React.InputHTMLAttributes<HTMLInputElement> & { label: string; error?: string }) {
  const { label, error, className, ...rest } = props;
  return <Field label={label} error={error}><input className={cn(inputClass, className)} {...rest} /></Field>;
}

export function FormTextarea(props: React.TextareaHTMLAttributes<HTMLTextAreaElement> & { label: string; error?: string }) {
  const { label, error, className, ...rest } = props;
  return <Field label={label} error={error}><textarea className={cn(inputClass, "min-h-28", className)} {...rest} /></Field>;
}

export function FormSelect(props: React.SelectHTMLAttributes<HTMLSelectElement> & { label: string; options: { label: string; value: string }[]; error?: string }) {
  const { label, error, className, options, ...rest } = props;
  return <Field label={label} error={error}><select className={cn(inputClass, className)} {...rest}>{options.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}</select></Field>;
}
