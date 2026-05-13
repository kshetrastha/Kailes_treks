import type { ReactNode } from "react";

export type Column<T> = { header: string; cell: (row: T) => ReactNode; className?: string };

export function DataTable<T>({ rows, columns, empty = "No records found" }: { rows: T[]; columns: Column<T>[]; empty?: string }) {
  return <div className="overflow-hidden rounded-xl border bg-card"><table className="w-full text-sm"><thead className="bg-muted/70 text-left"><tr>{columns.map((column) => <th key={column.header} className="px-4 py-3 font-semibold">{column.header}</th>)}</tr></thead><tbody>{rows.length ? rows.map((row, index) => <tr key={index} className="border-t">{columns.map((column) => <td key={column.header} className={column.className ?? "px-4 py-3"}>{column.cell(row)}</td>)}</tr>) : <tr><td className="px-4 py-10 text-center text-muted-foreground" colSpan={columns.length}>{empty}</td></tr>}</tbody></table></div>;
}
