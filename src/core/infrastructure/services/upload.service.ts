import { mkdir, writeFile } from "node:fs/promises";
import path from "node:path";
import { randomUUID } from "node:crypto";

export async function saveUpload(file: File) {
  const bytes = Buffer.from(await file.arrayBuffer());
  const uploadRoot = process.env.UPLOAD_DIR ?? "public/uploads";
  await mkdir(uploadRoot, { recursive: true });
  const extension = path.extname(file.name) || ".bin";
  const fileName = `${randomUUID()}${extension}`;
  const diskPath = path.join(uploadRoot, fileName);
  await writeFile(diskPath, bytes);
  return `/${path.relative("public", diskPath).replaceAll(path.sep, "/")}`;
}
