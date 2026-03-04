import { http } from "./http";

export type LoginDto = {
  username: string;
  password: string;
};

export type SessionTokenGetDto = {
  userId: number;
  createdAt: string;
  expiresAt: string;
  token: string;
};

export type UserGetDto = {
  userId: number;
  username: string;
  emailAddress: string;
  firstName: string | null;
  lastName: string | null;
  middleName: string | null;
  schoolId: number;
};

export type UserPatchDto = Partial<{
  username: string;
  firstName: string | null;
  lastName: string | null;
  middleName: string | null;
  schoolId: number;
}>;

export async function login(dto: LoginDto): Promise<SessionTokenGetDto> {
  return http<SessionTokenGetDto>(`/api/login`, {
    method: "POST",
    body: dto,
  });
}

export async function patchUser(userId: number, patch: UserPatchDto): Promise<UserGetDto> {
  return http<UserGetDto>(`/api/users/${userId}`, {
    method: "PATCH",
    body: patch,
  });
}

export async function getCurrentUser(): Promise<UserGetDto> {
  return http<UserGetDto>("/api/users/me");
}

export async function getUserById(userId: number): Promise<UserGetDto> {
  return http<UserGetDto>(`/api/users/${userId}`);
}