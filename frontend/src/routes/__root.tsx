import { Link, Outlet, createRootRoute } from '@tanstack/react-router'

export const Route = createRootRoute({
  component: RootComponent,
})

function RootComponent() {
  return (
    // Webpage Background
    <div className="min-h-dvh bg-gray-400">

      {/* Navigation Bar */}
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
          <Link to="/sign_in" className="rounded-lg px-3 py-2 bg-gray-900 text-white text-sm ml-auto">Sign in</Link>
        </div>
      </header>

    <Outlet />

      {/* Site Map Banner */}
      <section className="h-[300px] bg-gray-900 text-white flex items-center">
        <div className="mx-auto max-w-6xl w-full grid grid-cols-1 md:grid-cols-3 gap-24">
          
          {/* Contact Info */}
          <div className="text-left space-y-2">
            <h3 className="font-semibold text-lg mb-3">Contact Us!</h3>
            <p className="text-med text-gray-300">
              Open Textbook Exchange<br />
              Portland, OR<br />
              main@opentextbookexchange.shop<br />
              (012) 345-6789
            </p>
          </div>

          {/* Navigation Links */}
          <div className="text-left mx-auto">
            <h3 className="font-semibold text-lg mb-3">Navigation</h3>
            <ul className="text-med text-gray-300 space-y-2">
              <li><a href="#home" className="hover:text-white">Home</a></li>
              <li><a href="#explore" className="hover:text-white">Explore</a></li>
              <li><a href="#about" className="hover:text-white">About</a></li>
              <li><a href="#sign-in" className="hover:text-white">Sign In</a></li>
            </ul>
          </div>

          {/* Social Media */}
          <div className="text-left mx-auto">
            <h3 className="font-semibold text-lg mb-3">Social</h3>
            <ul className="text-med text-gray-300 space-y-2">
              <li><a href="#" className="hover:text-white">Instagram</a></li>
              <li><a href="#" className="hover:text-white">Twitter</a></li>
              <li><a href="#" className="hover:text-white">Facebook</a></li>
            </ul>
          </div>
        </div>
      </section>

      {/* Copyright Footer */}
      <footer className="bg-gray-950 text-gray-400 text-sm">
        <div className="mx-auto max-w-6xl w-full px-6 py-4 flex items-center justify-between">
          <p>© {new Date().getFullYear()} OpenTextbookExchange</p>
          <p>Designed for Oregon Tech Students</p>
        </div>
      </footer>
    </div> 
  )
}
