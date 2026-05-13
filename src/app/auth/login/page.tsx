import { getServerSession } from "next-auth";
import { redirect } from "next/navigation";
import { authOptions } from "@/lib/auth";
import { LoginForm } from "./login-form";

export default async function LoginPage() {
  const session = await getServerSession(authOptions);
  if (session?.user) redirect("/admin");
  return <section className="grid min-h-screen place-items-center bg-slate-950 px-4"><LoginForm /></section>;
}
