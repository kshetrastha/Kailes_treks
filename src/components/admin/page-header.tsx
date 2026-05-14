import Link from "next/link";

export function PageHeader({ title, description, actionHref, actionLabel }: { title: string; description?: string; actionHref?: string; actionLabel?: string }) {
  return <div className="flex flex-col gap-4 border-b pb-6 sm:flex-row sm:items-center sm:justify-between"><div><h1 className="text-3xl font-bold tracking-tight">{title}</h1>{description ? <p className="mt-2 text-muted-foreground">{description}</p> : null}</div>{actionHref ? <Link className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground" href={actionHref}>{actionLabel ?? "Create"}</Link> : null}</div>;
}
