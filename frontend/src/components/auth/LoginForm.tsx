import { useState } from "react";
import FormError from "../ui/FormError";
import PasswordInput from "../ui/PasswordInput";

type LoginFormProps = {
    onLogin?: (username: string, password: string) => void | Promise<void>;
    onCreateAccount?: () => void;
    onForgotPassword?: () => void;
    isSubmitting?: boolean;
    serverError?: string | null;
};

export default function LoginForm({
    onLogin,
    onCreateAccount,
    onForgotPassword,
    isSubmitting = false,
    serverError = null,
}: LoginFormProps) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    const usernameOk = username.trim().length >= 3;
    const passwordOk = password.length >= 6;
    const canSubmit = usernameOk && passwordOk;

    function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        if (!usernameOk) {
            setError("Username must be at least 3 characters.");
            return;
        }

        if (!passwordOk) {
            setError("Password must be at least 6 characters.");
            return;
        }

        setError(null);
        onLogin?.(username, password);
    }

    return (
        <form onSubmit={handleSubmit} className = "space-y-4">
            {/* Username */}
            <div className = "space-y-1">
                <label className = "text-sm font-medium text-gray-700" htmlFor = "username">
                    Username
                </label>
                <input
                    id="username"
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
            <div className = "space-y-1">
                <PasswordInput
                value={password}
                onChange={(val) => {
                    setPassword(val);
                    if (error) setError(null);
                }}
                />
            </div>

            <FormError message={serverError ?? error} />

            {/* Buttons */}
            <div className="space-y-2 pt-2">
                <button
                    type="submit"
                    disabled={!canSubmit || isSubmitting} //Disabled until input is valid
                    className="w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800"
                    >
                    Log in
                </button>

                <button
                    type="button"
                    onClick={onCreateAccount}
                    className="w-full rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
                    >
                    Create an account
                </button>

                <button
                    type="button"
                    onClick={onForgotPassword}
                    className="w-full text-sm text-gray-600 hover:text-gray-900"
                    >
                    Forgot password?
                </button>
            </div>
        </form>
    );
}