import { useMemo, useState } from "react";
import { loadAuth, saveAuth, clearAuth } from "../../services/AuthStorage";
import { AuthContext, type AuthState } from "./AuthContext";
import type { AuthUser } from "../../services/AuthApi";

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const saved = loadAuth();

  const [token, setToken] = useState<string | null>(saved?.token ?? null);
  const [user, setUser] = useState<AuthUser | null>(saved?.user ?? null);

  const isAuthed = !!token && !!user;

  const value = useMemo<AuthState>(
    () => ({
      isAuthed: Boolean(token),
      token,
      user,
      login: (newToken: string, newUser: AuthUser) => {
        setToken(newToken);
        setUser(newUser);
        saveAuth(newToken, newUser);
      },
      logout: () => {
        setToken(null);
        setUser(null);
        clearAuth();
      },
    }),
    [isAuthed, token, user]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}