import type { Metadata } from "next";
import "@/styles/globals.css";

export const metadata: Metadata = { title: { default: "Kailes Treks", template: "%s | Kailes Treks" }, description: "Expeditions and trekking experiences in the Himalayas." };

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return <html lang="en"><body>{children}</body></html>;
}
