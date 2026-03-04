import { useState } from "react"
import Modal from "../ui/Modal";
import LoginForm from "./LoginForm";
import CreateAccountForm from "./CreateAccountForm";
import ForgotPasswordForm from "./ForgotPasswordForm";
import * as authApi from "../../services/AuthApi";
import { useAuth } from "../auth/useAuth";

type AuthView = 'login' | 'createAccount' | 'forgotPassword';

type AuthModalProps = {
    isOpen: boolean;
    onClose: () => void;
};

export default function AuthModal({ isOpen, onClose}: AuthModalProps) {
    const [view, setView] = useState<AuthView>('login');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [serverError, setServerError] = useState<string | null>(null);
    const auth = useAuth();

    // Reset to login whenever it opens
    function handleClose() {
        setView('login');
        setServerError(null);
        onClose();
    }

    const title =
        view == "login" ? "Login" 
        : view == "createAccount" ? "Create Account" 
        : "Reset Password";

    return (
    <Modal isOpen={isOpen} onClose={handleClose} title={title}>
      {view === "login" && (
        <LoginForm
          serverError={serverError}
          isSubmitting={isSubmitting}
          onLogin={async (username: string, password: string) => {
            console.log("Attempting login", { username }); // remove after done testing
            setIsSubmitting(true);
            setServerError(null);

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
          onCreateAccount={() => {setServerError(null); setView("createAccount");}}
          onForgotPassword={() => {setServerError(null); setView("forgotPassword");}}
        />
      )}

      {view === "createAccount" && (
        <CreateAccountForm onBackToLogin={() => {setServerError(null); setView("login");}}
            serverError={serverError}
            isSubmitting={isSubmitting}
            onSubmit={async (email: string, username: string, password: string) => {
            setIsSubmitting(true);
            setServerError(null);

            try {
              const res = await authApi.register(email, username, password);
              auth.login(res.token, res.user);
              handleClose(); // closes + resets view
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
        <ForgotPasswordForm onBackToLogin={() => {setServerError(null); setView("login");}}
            serverError={serverError}
            isSubmitting={isSubmitting}
            onSubmit={async (email: string) => {
            setIsSubmitting(true);
            setServerError(null);

            try {
              await authApi.forgotPassword(email);
              setView("login");
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