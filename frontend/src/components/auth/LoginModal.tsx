import Modal from "../ui/Modal"
import LoginForm from "../auth/LoginForm";

type LoginModalProps = {
    isOpen: boolean;
    onClose: () => void;
};

export default function LoginModal({ isOpen, onClose }: LoginModalProps) {
    return (
        <Modal isOpen={isOpen} onClose={onClose} title="Login">
            <p className = "test-sm text-gray-600">
                <LoginForm
                    onLogin={(email, password) => {
                        console.log("login", { email, password });
                    }}
                    onCreateAccount={() => console.log("create account")}
                    onForgotPassword={() => console.log("forgot password")}
                />
            </p>
        </Modal>
    );
}