import type { AuthUser } from "../services/AuthApi";

const KEY = "ote_auth";

export type StoredAuth = {
  token: string;
  user: AuthUser;
};

export function loadAuth(): StoredAuth | null {
  const raw = localStorage.getItem(KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as StoredAuth;
  } catch {
    return null;
  }
}

export function saveAuth(token: string, user: AuthUser) {
  localStorage.setItem(KEY, JSON.stringify({ token, user } satisfies StoredAuth));
}

export function clearAuth() {
  localStorage.removeItem(KEY);
}