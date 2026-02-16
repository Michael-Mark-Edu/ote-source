import { Link } from "@tanstack/react-router";

export default function HeaderNav() {
  return (
    <nav className="hidden sm:flex items-center gap-6 text-sm text-gray-600 whitespace-nowrap">
      <Link to="/" className="hover:text-gray-900 text-lg pl-16">Home</Link>
      <Link to="/explore" className="hover:text-gray-900 text-lg border-l border-gray-300 pl-6">Explore</Link>
      <Link to="/about" className="hover:text-gray-900 text-lg border-l border-gray-300 pl-6">About</Link>
    </nav>
  );
}