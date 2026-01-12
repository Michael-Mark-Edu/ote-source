export default function HomeBanner() {
  return (
    <section className="h-[450px] bg-gray-200">
      <div className="mx-auto max-w-6xl h-full px-4 grid items-stretch gap-24 lg:grid-cols-2">
        <div className="h-full w-full">
          <div className="h-full w-full rounded-xl flex items-center justify-center bg-blue-300">
            <span className="text-black text-sm">Banner image placeholder</span>
          </div>
        </div>

        <div className="text-center">
          <h1 className="mt-3 font-bold text-2xl">Find, Sell, or Trade Your Textbooks</h1>
          <p className="mt-3 text-gray-600">Browse listings from local students.</p>
          <div className="mt-6 flex gap-3 justify-center">
            <a className="rounded-xl px-5 py-3 bg-gray-900 text-white hover:bg-gray-800">
              Browse
            </a>
          </div>
        </div>
      </div>
    </section>
  );
}
