import { useState } from "react";
import { useAccountProfile } from "../hooks/useAccountProfile";
import { login } from "../../../api/users";
import type { SessionTokenGetDto } from "../../../api/users";

export default function AccountTab({
  userId,
  onLoggedIn,
}: {
  userId: number | null;
  onLoggedIn: (session: SessionTokenGetDto) => void;
}) {
  // login form state
  const [loginUsername, setLoginUsername] = useState("");
  const [loginPassword, setLoginPassword] = useState("");
  const [loginError, setLoginError] = useState<string | null>(null);
  const [loginState, setLoginState] = useState<"idle" | "loggingIn">("idle");

  // must now accept userId and load via GET /api/users/{userId}
  const { profile, form, setField, save, saveState, errorMsg, loading } =
    useAccountProfile(userId);

  async function onLogin() {
    setLoginError(null);
    setLoginState("loggingIn");
    try {
      const session = await login({ username: loginUsername, password: loginPassword });
      onLoggedIn(session); // sets session in AccountPage -> userId becomes available
    } catch (e) {
      setLoginError(e instanceof Error ? e.message : "Login failed.");
    } finally {
      setLoginState("idle");
    }
  }

  // If not logged in yet, show login UI
  if (!userId) {
    return (
      <div className="space-y-4 rounded-xl border bg-white p-6 shadow-sm">
        <h2 className="text-lg font-semibold">Log in</h2>
        <p className="text-sm text-gray-600">
          Log in to view and edit your account details.
        </p>

        <div className="grid gap-3 sm:grid-cols-2">
          <input
            className="w-full rounded-md border px-3 py-2"
            placeholder="Username"
            value={loginUsername}
            onChange={(e) => setLoginUsername(e.target.value)}
            disabled={loginState === "loggingIn"}
          />
          <input
            className="w-full rounded-md border px-3 py-2"
            placeholder="Password"
            type="password"
            value={loginPassword}
            onChange={(e) => setLoginPassword(e.target.value)}
            disabled={loginState === "loggingIn"}
          />
        </div>

        {loginError && (
          <div className="rounded-md border border-red-200 bg-red-50 p-3 text-sm text-red-700">
            {loginError}
          </div>
        )}

        <button
          className="rounded-md bg-amber-600 px-6 py-2 font-medium text-white hover:bg-amber-700 disabled:opacity-50"
          onClick={onLogin}
          disabled={loginState === "loggingIn" || !loginUsername || !loginPassword}
        >
          {loginState === "loggingIn" ? "Logging in..." : "Login"}
        </button>
      </div>
    );
  }

  if (loading) return <div className="p-6">Loading account...</div>;
  if (!profile) return <div className="p-6 text-red-700">{errorMsg ?? "Failed to load profile."}</div>;

  return (
    <div className="space-y-4 rounded-xl border bg-white p-6 shadow-sm">
      <div>
        <div className="text-sm text-gray-500">Email Address</div>
        <div className="font-medium">{profile.emailAddress}</div>
      </div>

      <div className="grid gap-4 sm:grid-cols-2">
        <label className="block">
          <div className="text-sm font-medium text-gray-700">Username</div>
          <input
            className="mt-1 w-full rounded-md border px-3 py-2 focus:ring-2 focus:ring-amber-500 outline-none"
            value={form.username}
            onChange={(e) => setField("username", e.target.value)}
            disabled={saveState === "saving"}
          />
        </label>

        <label className="block">
          <div className="text-sm font-medium text-gray-700">First Name</div>
          <input
            className="mt-1 w-full rounded-md border px-3 py-2 focus:ring-2 focus:ring-amber-500 outline-none"
            value={form.firstName}
            onChange={(e) => setField("firstName", e.target.value)}
            disabled={saveState === "saving"}
          />
        </label>

        <label className="block">
          <div className="text-sm font-medium text-gray-700">Middle Name</div>
          <input
            className="mt-1 w-full rounded-md border px-3 py-2 focus:ring-2 focus:ring-amber-500 outline-none"
            value={form.middleName}
            onChange={(e) => setField("middleName", e.target.value)}
            disabled={saveState === "saving"}
          />
        </label>

        <label className="block">
          <div className="text-sm font-medium text-gray-700">Last Name</div>
          <input
            className="mt-1 w-full rounded-md border px-3 py-2 focus:ring-2 focus:ring-amber-500 outline-none"
            value={form.lastName}
            onChange={(e) => setField("lastName", e.target.value)}
            disabled={saveState === "saving"}
          />
        </label>
      </div>

      {errorMsg && (
        <div className="rounded-md border border-red-200 bg-red-50 p-3 text-sm text-red-700">
          {errorMsg}
        </div>
      )}

      <button
        className="mt-4 rounded-md bg-amber-600 px-6 py-2 font-medium text-white hover:bg-amber-700 disabled:opacity-50"
        onClick={save}
        disabled={saveState === "saving"}
      >
        {saveState === "saving" ? "Saving..." : "Save Changes"}
      </button>

      {saveState === "saved" && (
        <span className="ml-3 text-sm text-green-600">Changes saved!</span>
      )}
    </div>
  );
}
