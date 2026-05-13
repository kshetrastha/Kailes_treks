"use client";
import { useState } from "react";
import { signIn } from "next-auth/react";

export function LoginForm() {
  const [error, setError] = useState<string | null>(null);
  return <form action={async (formData) => { const result = await signIn("credentials", { email: formData.get("email"), password: formData.get("password"), callbackUrl: "/admin", redirect: false }); if (result?.error) setError("Invalid email or password"); else window.location.href = result?.url ?? "/admin"; }} className="w-full max-w-sm rounded-2xl bg-white p-8 shadow-xl"><h1 className="text-2xl font-black">Admin login</h1>{error ? <p className="mt-4 rounded-md bg-red-50 p-3 text-sm text-red-600">{error}</p> : null}<label className="mt-6 grid gap-2 text-sm font-medium">Email<input className="rounded-md border px-3 py-2" name="email" type="email" /></label><label className="mt-4 grid gap-2 text-sm font-medium">Password<input className="rounded-md border px-3 py-2" name="password" type="password" /></label><button className="mt-6 w-full rounded-md bg-primary px-4 py-2 font-semibold text-white">Sign in</button></form>;
}
