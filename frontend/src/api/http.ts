export class ApiError extends Error {
  status: number;
  bodyText?: string;

  constructor(message: string, status: number, bodyText?: string) {
    super(message);
    this.name = "ApiError";
    this.status = status;
    this.bodyText = bodyText;
  }
}

type HttpOptions = {
  method?: "GET" | "POST" | "PATCH" | "DELETE";
  body?: unknown;
};

const API_BASE =
import.meta.env.VITE_API_BASE_URL?.replace(/\/+$/, "") ?? "";

export async function http<T>(path: string, options: HttpOptions = {}): Promise<T> {
  const method = options.method ?? "GET";

  const headers: Record<string, string> = {
    Accept: "application/json",
  };

  if (options.body !== undefined) {
    headers["Content-Type"] = "application/json";
  }

  const res = await fetch(`${API_BASE}${path}`, {
    method,
    headers,
    credentials: "include",
    body: options.body !== undefined ? JSON.stringify(options.body) : undefined,
  });

  const contentType = res.headers.get("content-type") ?? "";
  const isJson = contentType.includes("application/json");

  if (!res.ok) {
    const bodyText = isJson ? JSON.stringify(await res.json()) : await res.text();
    throw new ApiError(`Request failed: ${method} ${path}`, res.status, bodyText);
  }

  if (res.status === 204) {
    return undefined as T;
  }

  return (isJson ? await res.json() : await res.text()) as T;
}