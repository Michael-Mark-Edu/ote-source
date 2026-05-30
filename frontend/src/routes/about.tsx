import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/about')({
  component: AboutPage,
})

function AboutPage() {
  return (
    <>
      {/* About Banner */}
      <section
        className="relative h-[300px] w-full bg-cover bg-center"
        style={{ backgroundImage: "url('/about_us_banner.png')" }}
      >
        <div className="absolute inset-0 bg-black/40" />

        <div className="relative mx-auto flex h-full max-w-6xl flex-col justify-center px-6 text-white">
          <h1 className="text-5xl font-bold">About Open Textbook Exchange</h1>
          <p className="mt-4 max-w-2xl text-xl">
            A student-focused platform built to make textbooks easier to find,
            share, and afford.
          </p>
        </div>
      </section>

      {/* About Us */}
      <section className="bg-gray-100 py-12">
        <div className="mx-auto max-w-5xl px-6">
          <h2 className="text-4xl font-semibold text-gray-900">Who We Are</h2>

          <p className="mt-6 text-xl leading-8 text-gray-700">
            Open Textbook Exchange is a web platform designed for Oregon Tech
            students who want a better way to buy, sell, rent, or trade
            textbooks. The project was created to support students by giving
            them a direct way to exchange course materials within their own
            campus community.
          </p>
        </div>
      </section>

      {/* Mission */}
      <section className="bg-gray-100 py-12">
        <div className="mx-auto max-w-5xl px-6">
          <h2 className="text-4xl font-semibold text-gray-900">Our Mission</h2>

          <p className="mt-6 text-xl leading-8 text-gray-700">
            <strong>Make textbooks more affordable.</strong> Textbooks can be a
            major cost for students, so OTE helps students find used textbooks
            directly from other Oregon Tech students.
          </p>

          <p className="mt-6 text-xl leading-8 text-gray-700">
            <strong>Keep exchanges student-centered.</strong> Instead of relying
            on an outside marketplace, OTE focuses on connecting students who are
            taking similar courses and may need the same materials.
          </p>

          <p className="mt-6 text-xl leading-8 text-gray-700">
            <strong>Make listings simple to create and browse.</strong> Users can
            create textbook listings with details such as title, ISBN, condition,
            price, exchange type, and photos.
          </p>

          <p className="mt-6 text-xl leading-8 text-gray-700">
            <strong>Support trust between users.</strong> Public profiles,
            reviews, and account reporting help students make more informed
            decisions when viewing listings and contacting sellers.
          </p>
        </div>
      </section>

      <section className="h-[60px] bg-gray-700" />

      {/* How It Works */}
      <section className="bg-gray-100 py-16">
        <div className="mx-auto max-w-6xl px-6">
          <h2 className="text-center text-4xl font-semibold text-gray-900">
            How It Works
          </h2>

          <div className="mt-10 grid gap-6 md:grid-cols-2 lg:grid-cols-4">
            <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
              <h3 className="text-xl font-semibold text-gray-900">
                Find Your Book
              </h3>
              <p className="mt-3 text-gray-600">
                Search and browse textbook listings by title, ISBN, course, or
                other listing details.
              </p>
            </div>

            <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
              <h3 className="text-xl font-semibold text-gray-900">
                View the Listing
              </h3>
              <p className="mt-3 text-gray-600">
                Check the book condition, price, exchange type, photos, and
                seller profile before deciding.
              </p>
            </div>

            <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
              <h3 className="text-xl font-semibold text-gray-900">
                Contact the Seller
              </h3>
              <p className="mt-3 text-gray-600">
                Use the seller information to reach out and arrange the textbook
                exchange.
              </p>
            </div>

            <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
              <h3 className="text-xl font-semibold text-gray-900">
                Exchange the Book
              </h3>
              <p className="mt-3 text-gray-600">
                Meet safely, verify the textbook, and complete the buy, sell,
                rent, or trade agreement.
              </p>
            </div>
          </div>
        </div>
      </section>
    </>
  )
}