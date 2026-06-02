import { useState } from "react";
import FormError from "../ui/FormError";
import PasswordInput from "../ui/PasswordInput";

type CreateAccountDto = {
  username: string;
  emailAddress: string;
  firstName: string;
  lastName: string;
  middleName: string;
  password: string;
  schoolId: number;
};

type CreateAccountFormProps = {
  onSubmit?: (dto: CreateAccountDto) => void;
  onBackToLogin?: () => void;
  isSubmitting?: boolean;
  serverError?: string | null;
};

function isOitEmail(email: string) {
  return email.trim().toLowerCase().endsWith("@oit.edu");
}

export default function CreateAccountForm({
  onSubmit,
  onBackToLogin,
  isSubmitting = false,
  serverError = null,
}: CreateAccountFormProps) {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [firstName, setFirstName] = useState("");
  const [middleName, setMiddleName] = useState("");
  const [lastName, setLastName] = useState("");
  const [schoolId, setSchoolId] = useState(0);
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string | null>(null);

  const emailOk = isOitEmail(email);
  const usernameOk = username.trim().length >= 3;

  const firstOk = firstName.trim().length >= 1;
  const lastOk = lastName.trim().length >= 1;

  const passwordOk = password.length >= 6;
  const passwordsMatch = password === confirmPassword;

  const schoolOk = Number.isFinite(schoolId) && schoolId > 0;

  const emailInvalid = email.trim().length > 0 && !emailOk;
  const usernameInvalid = username.trim().length > 0 && !usernameOk;
  const passwordTooShort = password.length > 0 && !passwordOk;
  const confirmPasswordMismatch = confirmPassword.length > 0 && !passwordsMatch;

  const canSubmit = emailOk && usernameOk && firstOk && lastOk && passwordOk && passwordsMatch && schoolOk;

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!emailOk) {
      setError("Please use your Oregon Tech email address ending in @oit.edu.");
      return;
    }

    if (!passwordsMatch) {
      setError("Passwords do not match.");
      return;
    }

    if (!canSubmit) {
      setError("Please fill all required fields and password (6+ chars).");
      return;
    }

    setError(null);

    onSubmit?.({
      emailAddress: email.trim(),
      username: username.trim(),
      firstName: firstName.trim(),
      middleName: middleName.trim(),
      lastName: lastName.trim(),
      password,
      schoolId,
    });
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      {/* Email */}
      <div className="space-y-1">
        <label
          className="text-sm font-medium text-gray-700"
          htmlFor="createEmail"
        >
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
          placeholder="you@oit.edu"
          required
        />

        <p className="text-xs text-gray-500">
          Use your Oregon Tech email address.
        </p>

        {emailInvalid && (
          <p className="text-xs text-red-600">
            Email must end with @oit.edu.
          </p>
        )}
      </div>

      {/* Username */}
      <div className="space-y-1">
        <label
          className="text-sm font-medium text-gray-700"
          htmlFor="createUsername"
        >
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

        {usernameInvalid && (
          <p className="text-xs text-red-600">
            Username must be at least 3 characters.
          </p>
        )}
      </div>

      {/* Names */}
      <div className="grid grid-cols-1 gap-3 sm:grid-cols-3">
        <div className="space-y-1">
          <label
            className="text-sm font-medium text-gray-700"
            htmlFor="firstName"
          >
            First
          </label>

          <input
            id="firstName"
            type="text"
            autoComplete="given-name"
            value={firstName}
            onChange={(e) => {
              setFirstName(e.target.value);
              if (error) setError(null);
            }}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
            placeholder="First"
            required
          />
        </div>

        <div className="space-y-1">
          <label
            className="text-sm font-medium text-gray-700"
            htmlFor="middleName"
          >
            Middle
          </label>

          <input
            id="middleName"
            type="text"
            autoComplete="additional-name"
            value={middleName}
            onChange={(e) => {
              setMiddleName(e.target.value);
              if (error) setError(null);
            }}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
            placeholder="Optional"
          />
        </div>

        <div className="space-y-1">
          <label
            className="text-sm font-medium text-gray-700"
            htmlFor="lastName"
          >
            Last
          </label>

          <input
            id="lastName"
            type="text"
            autoComplete="family-name"
            value={lastName}
            onChange={(e) => {
              setLastName(e.target.value);
              if (error) setError(null);
            }}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
            placeholder="Last"
            required
          />
        </div>
      </div>

      {/* School */}
      <div className="space-y-1">
        <label
          className="text-sm font-medium text-gray-700"
          htmlFor="createSchool"
        >
          Campus
        </label>

        <select
          id="createSchool"
          value={schoolId}
          onChange={(e) => {
            setSchoolId(Number(e.target.value));
            if (error) setError(null);
          }}
          className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
          required
        >
          <option value={0}>Select a campus</option>
          <option value={36}>Oregon Tech Portland-Metro</option>
          <option value={36}>Oregon Tech Klamath Falls</option>
        </select>

        {!schoolOk && schoolId === 0 && (
          <p className="text-xs text-gray-500">
            Select your Oregon Tech campus.
          </p>
        )}
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

      {passwordTooShort && (
        <p className="text-xs text-red-600">
          Password must be at least 6 characters.
        </p>
      )}

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

      {confirmPasswordMismatch && (
        <p className="text-xs text-red-600">Passwords do not match.</p>
      )}

      {/* Error Message */}
      <FormError message={serverError ?? error} />

      <div className="space-y-2 pt-2">
        <button
          type="submit"
          disabled={!canSubmit || isSubmitting}
          className="w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800 disabled:cursor-not-allowed disabled:opacity-50"
        >
          {isSubmitting
            ? "Creating account..."
            : canSubmit
              ? "Create account"
              : "Complete required fields"}
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