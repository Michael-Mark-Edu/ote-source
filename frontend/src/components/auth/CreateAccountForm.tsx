import { useState } from "react";
import FormError from "../ui/FormError";
import PasswordInput from "../ui/PasswordInput";

type CreateAccountFormProps = {
  onSubmit?: (email: string, username: string, password: string) => void;
  onBackToLogin?: () => void;
  isSubmitting?: boolean;
  serverError?: string | null;
};

export default function CreateAccountForm({
  onSubmit,
  onBackToLogin,
  isSubmitting = false,
  serverError = null,
}: CreateAccountFormProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string | null>(null);

  const emailOk = email.includes("@") && email.includes(".");
  const usernameOk = username.trim().length >= 3;
  const passwordOk = password.length >= 6;
  const passwordsMatch = password === confirmPassword;
  const canSubmit = emailOk && usernameOk && passwordOk && passwordsMatch;

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!passwordsMatch) {
      setError("Passwords do not match.");
      return;
    }

    if (!canSubmit) {
      setError("Please enter a valid email and a password (6+ chars).");
      return;
    }
    setError(null);
    onSubmit?.(email, username, password);;
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">

      {/* Email */}
      <div className="space-y-1">
        <label className="text-sm font-medium text-gray-700" htmlFor="createEmail">
          Email
        </label>
        <input
          id="createEmail"
          type="email"
          autoComplete="email"
          value={email}
          onChange={(e) => {
            setEmail(e.target.value);
            if (error) setError(null);
          }}
          className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
          placeholder="you@example.com"
          required
        />
      </div>

      {/* Username */}
      <div className="space-y-1">
        <label className="text-sm font-medium text-gray-700" htmlFor="createUsername">
          Username
        </label>
        <input
          id="createUsername"
          type="text"
          autoComplete="username"
          value={username}
          onChange={(e) => {
            setUsername(e.target.value);
            if (error) setError(null);
          }}
          className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
          placeholder="username"
          required
        />
      </div>

      {/* Password */}
      <PasswordInput
        id="createPassword"
        label="Password"
        value={password}
        onChange={(val) => {
          setPassword(val);
          if (error) setError(null);
        }}
        autoComplete="new-password"
      />

            <PasswordInput
        id="confirmPassword"
        label="Confirm Password"
        value={confirmPassword}
        onChange={(val) => {
          setConfirmPassword(val);
          if (error) setError(null);
        }}
        autoComplete="new-password"
      />

      {/* Error Message */}
      <FormError message={serverError ?? error} />

      <div className="space-y-2 pt-2">
        <button
          type="submit"
          disabled={!canSubmit || isSubmitting}
          className="w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Create account
        </button>

        <button
          type="button"
          onClick={onBackToLogin}
          className="w-full rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
        >
          Back to login
        </button>
      </div>
    </form>
  );
}