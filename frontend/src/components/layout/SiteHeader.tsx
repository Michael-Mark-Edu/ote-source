import { Link } from "@tanstack/react-router";

export default function SiteHeader() {
  return (
    <header className="sticky bg-white/90 border-b">
      <div className="mx-auto max-w-8xl h-16 px-4 flex items-center gap-10">
        {/* Title/Logo */}
        <a className="font-semibold">OpenTextbookExchange</a>

        {/* Page Choices */}
        <nav className="hidden sm:flex gap-10 text-sm text-gray-600">
          <Link to="/" className="hover:text-gray-900 text-lg pl-40">Home</Link>
          <Link to="/explore" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Explore</Link>
          <Link to="/about" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">About</Link>
          <Link to="/test" className="hover:text-gray-900 text-lg pl-10 border-l border-gray-300">Testing</Link>
        </nav>

        {/* Search Bar */}
        <div className="w-40 sm:w-64 m-auto">
          <input
            type="search"
            placeholder="Search books..."
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm"
          />
        </div>

        {/* Sign in */}
        <Link to="/sign_in" className="rounded-lg px-3 py-2 bg-gray-900 text-white text-sm ml-auto">
          Sign in
        </Link>
      </div>
    </header>
  );
}