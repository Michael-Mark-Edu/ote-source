import { Link } from "@tanstack/react-router";
import { useState } from "react";
import AuthModal from "../auth/AuthModal";
import { useAuth } from "../auth/useAuth";


export default function SiteHeader() {
  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const auth = useAuth();

  return (
    <>
      <header className="sticky bg-white/90 border-b">
        <div className="mx-auto max-w-8xl h-16 px-4 flex items-center gap-10">
          {/* Title/Logo */}
          <a className="font-semibold">OpenTextbookExchange</a>

          {/* Page Choices */}
          <nav className="hidden sm:flex gap-10 text-sm text-gray-600">
            <Link to="/" className="hover:text-gray-900 text-lg pl-40">Home</Link>
            <Link to="/explore" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Explore</Link>
            <Link to="/about" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">About</Link>
            <Link to="/account" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Account</Link>
          </nav>

        {/* Page Choices */}
        <nav className="hidden sm:flex gap-10 text-sm text-gray-600">
          <Link to="/" className="hover:text-gray-900 text-lg pl-40">Home</Link>
          <Link to="/explore" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Explore</Link>
          <Link to="/about" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">About</Link>
          <Link to="/user_upload" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Upload</Link>
        </nav>

          {/* Login */}
          {auth.isAuthed ? (
          <button
            type="button"
            onClick={() => auth.logout()}
            className="rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
          >
            Logout
          </button>
          ) : (
          <button
            type="button"
            onClick={() => setIsLoginOpen(true)}
            className="rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800"
          >
            Login
          </button>
        )}
        </div>
      </header>

      <AuthModal isOpen={isLoginOpen} onClose={() => setIsLoginOpen(false)} />
    </>
  );
}