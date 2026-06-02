import { createContext } from "react";

export type AuthUser = {
  id: string;
  email: string;
  username: string;
};

export type AuthState = {
  token: string | null;
  user: AuthUser | null;
  isAuthed: boolean;
  login: (token: string, user: AuthUser) => void;
  logout: () => void;
};

export const AuthContext = createContext<AuthState | null>(null);