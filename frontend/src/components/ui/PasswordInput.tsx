import { useState } from "react";

type PasswordInputProps = {
  id?: string;
  label?: string;
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  autoComplete?: string;
  required?: boolean;
};

export default function PasswordInput({
  id = "password",
  label = "Password",
  value,
  onChange,
  placeholder = "password",
  autoComplete = "current-password",
  required = true,
}: PasswordInputProps) {
  const [show, setShow] = useState(false);

  return (
    <div className="space-y-1">
      <label className="text-sm font-medium text-gray-700" htmlFor={id}>
        {label}
      </label>

      <div className="relative">
        <input
          id={id}
          type={show ? "text" : "password"}
          autoComplete={autoComplete}
          value={value}
          onChange={(e) => onChange(e.target.value)}
          className="w-full rounded-lg border border-gray-300 px-3 py-2 pr-20 text-sm"
          placeholder={placeholder}
          required={required}
        />

        <button
          type="button"
          onClick={() => setShow((s) => !s)}
          className="absolute right-2 top-1/2 -translate-y-1/2 rounded-md px-2 py-1 text-sm text-gray-600 hover:bg-gray-100 hover:text-gray-900"
          aria-label={show ? "Hide password" : "Show password"}
        >
          {show ? "Hide" : "Show"}
        </button>
      </div>
    </div>
  );
}