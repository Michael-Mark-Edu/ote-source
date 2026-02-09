import { useState } from "react";
import FormError from "../ui/FormError";

type ForgotPasswordFormProps = {
  onSubmit?: (email: string) => void;
  onBackToLogin?: () => void;
  serverError?: string | null;
  isSubmitting?: boolean;
};

export default function ForgotPasswordForm({
  onSubmit,
  onBackToLogin,
  serverError = null,
  isSubmitting = false,
}: ForgotPasswordFormProps) {
  const [email, setEmail] = useState("");
  const [error, setError] = useState<string | null>(null);

  const emailOk = email.includes("@") && email.includes(".");

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!emailOk) {
      setError("Please enter a valid email address.");
      return;
    }
    setError(null);
    onSubmit?.(email);
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-1">
        <label className="text-sm font-medium text-gray-700" htmlFor="resetEmail">
          Email
        </label>
        <input
          id="resetEmail"
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

      <FormError message={serverError ?? error} />

      <div className="space-y-2 pt-2">
        <button
          type="submit"
          disabled={!emailOk || isSubmitting} //Disabled until input is valid and not submitting
          className="w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Send reset link
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