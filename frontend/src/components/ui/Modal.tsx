type ModalProps = {
    isOpen: boolean;
    title?: string;
    onClose: () => void;
    children: React.ReactNode;
};

export default function Modal({ isOpen, title, onClose, children }: ModalProps) {

    if (!isOpen) return null;

    return (
        <div className = "fixed inset-0 z-50">
            {/* Backdrop */}
            <button
                className = "absolute inset-0 bg-black/50"
                onClick = {onClose}
                aria-label = "Close modal"
            />

            {/* Panel */}
            <div className = "relative mx-auto mt-24 w-[92%] max-w-md rounded-2xl bg-white shadow-xl">
                <div className= "flex items-center justify-between border-b px-5 py-4">
                    <h2 className = "text-lg font-semibold">{title ?? "Modal"}</h2>
                    <button
                        onClick = {onClose}
                        className = "rounded-lg px-2 py-1 text-gray-500 hover:bg-gray-100 hover:text-gray-800"
                        aria-label = "Close"
                    >
                        X
                    </button>
                </div>

                <div className = "px-5 py-4">{children}</div>
            </div>
        </div>
    );
}