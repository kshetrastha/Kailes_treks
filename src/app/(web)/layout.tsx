import { SiteFooter } from "@/components/web/site-footer";
import { SiteHeader } from "@/components/web/site-header";

export default function WebLayout({ children }: { children: React.ReactNode }) {
  return <><SiteHeader /><main>{children}</main><SiteFooter /></>;
}
