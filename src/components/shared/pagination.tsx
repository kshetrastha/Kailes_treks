import Link from "next/link";

export function Pagination({ page, pageCount, basePath }: { page: number; pageCount: number; basePath: string }) {
  if (pageCount <= 1) return null;
  return <nav className="flex items-center justify-end gap-2"><Link className="rounded-md border px-3 py-1 text-sm aria-disabled:opacity-50" aria-disabled={page <= 1} href={`${basePath}?page=${Math.max(1, page - 1)}`}>Previous</Link><span className="text-sm text-muted-foreground">Page {page} of {pageCount}</span><Link className="rounded-md border px-3 py-1 text-sm aria-disabled:opacity-50" aria-disabled={page >= pageCount} href={`${basePath}?page=${Math.min(pageCount, page + 1)}`}>Next</Link></nav>;
}
