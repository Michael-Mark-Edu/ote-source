import { useState } from "react"
import Modal from "../ui/Modal";
import LoginForm from "./LoginForm";
import CreateAccountForm from "./CreateAccountForm";
import ForgotPasswordForm from "./ForgotPasswordForm";
import * as authApi from "../../services/AuthApi";
import { useAuth } from "../auth/useAuth";
import { createUser } from "../../api/users";

type AuthView = 'login' | 'createAccount' | 'forgotPassword';

type AuthModalProps = {
    isOpen: boolean;
    onClose: () => void;
};

export default function AuthModal({ isOpen, onClose}: AuthModalProps) {
    const [view, setView] = useState<AuthView>('login');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [serverError, setServerError] = useState<string | null>(null);
    const [infoMsg, setInfoMsg] = useState<string | null>(null);
    
    const auth = useAuth();

    // Reset to login whenever it opens
    function handleClose() {
        setView('login');
        setServerError(null);
        setInfoMsg(null);
        onClose();
    }

    const title =
        view == "login" ? "Login" 
        : view == "createAccount" ? "Create Account" 
        : "Reset Password";

    return (
    <Modal isOpen={isOpen} onClose={handleClose} title={title}>
      {infoMsg && (
        <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-3 py-2 text-sm text-green-800">
          {infoMsg}
        </div>
      )}

      {view === "login" && (
        <LoginForm
          serverError={serverError}
          isSubmitting={isSubmitting}
          onLogin={async (username: string, password: string) => {
            console.log("Attempting login", { username }); // remove after done testing
            setIsSubmitting(true);
            setServerError(null);
            setInfoMsg(null);

            try {
              const res = await authApi.login(username, password);
              auth.login(res.token, res.user);
              handleClose(); // closes + resets view
            } catch (e) {
              const msg = e instanceof Error ? e.message : "Login failed";
              setServerError(msg);
            } finally {
              setIsSubmitting(false);
            }
          }}
          onCreateAccount={() => {setServerError(null); setInfoMsg(null); setView("createAccount");}}
          onForgotPassword={() => {setServerError(null); setInfoMsg(null); setView("forgotPassword");}}
        />
      )}

      {view === "createAccount" && (
        <CreateAccountForm onBackToLogin={() => {setServerError(null); setInfoMsg(null); setView("login");}}
            serverError={serverError}
            isSubmitting={isSubmitting}
            onSubmit={async (dto) => {
            setIsSubmitting(true);
            setServerError(null);
            setInfoMsg(null);

            try {
              await createUser(dto);
              setView("login");
              setInfoMsg("Account created. Please log in.");
            } catch (e) {
              const msg = e instanceof Error ? e.message : "Account creation failed";
              setServerError(msg);
            } finally {
              setIsSubmitting(false);
            }
          }}
        />
      )}

      {view === "forgotPassword" && (
        <ForgotPasswordForm onBackToLogin={() => {setServerError(null); setInfoMsg(null); setView("login");}}
            serverError={serverError}
            isSubmitting={isSubmitting}
            onSubmit={async (email: string) => {
            setIsSubmitting(true);
            setServerError(null);
            setInfoMsg(null);

            try {
              await authApi.forgotPassword(email);
              setView("login");
              setInfoMsg("Password reset instructions sent (if the email exists).");
            } catch (e) {
              const msg = e instanceof Error ? e.message : "Reset failed";
              setServerError(msg);
            } finally {
              setIsSubmitting(false);
            }
          }}
        />
      )}
    </Modal>
  );
}