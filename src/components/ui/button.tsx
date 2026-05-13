import * as React from "react";
import { cn } from "@/lib/utils";

export function Button({ className, variant = "primary", ...props }: React.ButtonHTMLAttributes<HTMLButtonElement> & { variant?: "primary" | "outline" | "ghost" | "destructive" }) {
  return <button className={cn("inline-flex items-center justify-center rounded-md px-4 py-2 text-sm font-medium shadow-sm disabled:opacity-50", variant === "primary" && "bg-primary text-primary-foreground hover:bg-orange-600", variant === "outline" && "border bg-background hover:bg-muted", variant === "ghost" && "shadow-none hover:bg-muted", variant === "destructive" && "bg-destructive text-destructive-foreground", className)} {...props} />;
}
