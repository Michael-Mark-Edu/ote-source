export type AuthUser = {
  id: string;
  username: string,
  email: string;
};

export type LoginResponse = {
  token: string;
  user: AuthUser;
};

export class ApiError extends Error {
  status: number;
  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

const USE_MOCK = (import.meta.env.VITE_USE_MOCK_AUTH ?? "false") === "true";
const API_BASE = import.meta.env.VITE_API_BASE_URL ?? "";

type StoredUser = {
  id: string;
  email: string;
  username: string;
  password: string; // mock password
};

const USERS_KEY = "ote_mock_users";

function sleep(ms: number) {
  return new Promise((r) => setTimeout(r, ms));
}

function loadUsers(): StoredUser[] {
  const raw = localStorage.getItem(USERS_KEY);
  if (!raw) return [];
  try {
    return JSON.parse(raw) as StoredUser[];
  } catch {
    return [];
  }
}

function saveUsers(users: StoredUser[]) {
  localStorage.setItem(USERS_KEY, JSON.stringify(users));
}

function newId() {
  return crypto.randomUUID();
}

function makeToken(userId: string) {
  return `mock.${userId}.${Date.now()}`;
}

type ApiErrorBody = { message?: string; error?: string };

async function handleJson<T>(res: Response): Promise<T> {
  if (res.ok) return (await res.json()) as T;

  let msg = `Request failed (${res.status})`;
  try {
    const body = (await res.json()) as ApiErrorBody;
    if (body?.message) msg = body.message;
    else if (body?.error) msg = body.error;
  } catch {
    // ignore
  }
  throw new ApiError(res.status, msg);
}

type SessionTokenGetDto = {
  userId: number;
  createdAt: string;
  expiresAt: string;
  token: string;
};

type UserGetDto = {
  userId: number;
  username: string;
  emailAddress: string;
};

// "usernameOrEmail" in mock mode. "username" in production mode.
export async function login(usernameOrEmail: string, password: string): Promise<LoginResponse> {
  if (USE_MOCK) {
    await sleep(300);

    const users = loadUsers();
    const lowered = usernameOrEmail.toLowerCase();

    const user =
      users.find((u) => u.email.toLowerCase() === lowered) ??
      users.find((u) => u.username.toLowerCase() === lowered);
      
    if (!user) throw new ApiError(401, "No account found with that email/username.");
    if (user.password !== password) throw new ApiError(401, "Incorrect password.");

    return {
      token: makeToken(user.id),
      user: { id: user.id, email: user.email, username: user.username },
    };
  }

  // Login and open session token
  const tokenRes = await fetch(`${API_BASE}/api/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username: usernameOrEmail, password }),
  });

  const session = await handleJson<SessionTokenGetDto>(tokenRes);
  console.log("session from /api/login:", session); // for testing

  // Fetch the user profile
  const userRes = await fetch(`${API_BASE}/api/users/${session.userId}`, {
    method: "GET",
  });
  
  const userDto = await handleJson<UserGetDto>(userRes);

  return {
    token: session.token,
    user: {
      id: String(userDto.userId),
      username: userDto.username,
      email: userDto.emailAddress,
    },
  };
}

export async function register(email: string, username: string, password: string): Promise<LoginResponse> {
  if (USE_MOCK) {
    await sleep(400);

    const users = loadUsers();
    const existsEmail = users.some((u) => u.email.toLowerCase() === email.toLowerCase());
    if (existsEmail) throw new ApiError(409, "An account with that email already exists.");

    const existsUsername = users.some((u) => u.username.toLowerCase() === username.toLowerCase());
    if (existsUsername) throw new ApiError(409, "That username is already taken.");

    const created: StoredUser = { id: newId(), email, username, password };
    users.push(created);
    saveUsers(users);

    return {
      token: makeToken(created.id),
      user: { id: created.id, email: created.email, username: created.username },
    };
  }

  // Create user on backend
  const createRes = await fetch(`${API_BASE}/api/users`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username,
      emailAddress: email,
      firstName: null,
      lastName: null,
      middleName: null,
      schoolId: 36, // TODO: replace with school/campus selection in the future
      password,
    }),
  });

  await handleJson<unknown>(createRes);

  // After account is registered, attempt login
  return login(username, password);
}

export async function forgotPassword(email: string): Promise<void> {
  if (USE_MOCK) {
    await sleep(300);

    const users = loadUsers();
    const exists = users.some((u) => u.email.toLowerCase() === email.toLowerCase());
    if (!exists) throw new ApiError(404, "No account found with that email.");

    // TODO: Add email verification
    return;
  }

  const res = await fetch(`${API_BASE}/api/forgot-password`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });

  await handleJson<unknown>(res);
}