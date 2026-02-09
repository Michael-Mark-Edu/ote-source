import { Link } from "@tanstack/react-router";

export default function SiteFooter() {
  return (
    <>
      {/* Site Map Banner */}
      <section className="h-[300px] bg-gray-900 text-white flex items-center">
        <div className="mx-auto max-w-6xl w-full grid grid-cols-1 md:grid-cols-3 gap-24">
          {/* Contact Info */}
          <div className="text-left space-y-2">
            <h3 className="font-semibold text-lg mb-3">Contact Us!</h3>
            <p className="text-med text-gray-300">
              Open Textbook Exchange
              <br />
              Portland, OR
              <br />
              main@opentextbookexchange.shop
              <br />
              (012) 345-6789
            </p>
          </div>

          {/* Navigation Links */}
          <div className="text-left mx-auto">
            <h3 className="font-semibold text-lg mb-3">Navigation</h3>
            <ul className="text-med text-gray-300 space-y-2">
              <li>
                <Link to="/" className="hover:text-white">Home</Link>
              </li>

              <li>
                <Link to="/explore" className="hover:text-white">Explore</Link>
              </li>

              <li>
                <Link to="/about" className="hover:text-white">About</Link>
              </li>

              <li>
                <Link to="/account" className="hover:text-white">Account</Link>
              </li>

            </ul>
          </div>

          {/* Social Media */}
          <div className="text-left mx-auto">
            <h3 className="font-semibold text-lg mb-3">Social</h3>
            <ul className="text-med text-gray-300 space-y-2">
              <li>
                <a href="#" className="hover:text-white">Instagram</a>
              </li>
              
              <li>
                <a href="#" className="hover:text-white">Twitter</a>
              </li>

              <li>
                <a href="#" className="hover:text-white">Facebook</a>
              </li>
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
    </>
  );
}