import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
  component: HomePage,
})

function HomePage() {
  return (
    <>
      {/* Main Banner */}
      <section className="h-[450px] bg-gray-200">
        <div className="mx-auto max-w-6xl h-full px-4 grid items-stretch gap-24 lg:grid-cols-2">

          {/* Banner Image */}
          <div className="h-full w-full">
            <div className="h-full w-full rounded-xl flex items-center justify-center bg-blue-300">
              <span className="text-black text-sm">Banner image placeholder</span>
            </div>
          </div>

          {/* Banner Text */}
          <div className="text-center">
            <h1 className="mt-3 font-bold text-2xl">Find, Sell, or Trade Your Textbooks</h1>
            <p className="mt-3 text-gray-600">Browse listings from local students.</p>
            <div className="mt-6 flex gap-3">
              <a className="rounded-xl px-5 py-3 bg-gray-900 text-white hover:bg-gray-800">Browse</a>
            </div>
          </div>
        </div>
      </section>
        
      {/* Popular Books Banner */}
      <section className="h-[600px] flex items-center justify-center bg-gray-800">
        <div className="mx-auto max-w-6xl w-full px-4 grid items-center gap-8 lg:grid-cols-4">

          {/* Banner Images */}
          {[1, 2, 3, 4].map((num) => (
            <div key={num} className="rounded-2xl border p-2 shadow-sm bg-white">
              <div className="aspect-9/16 rounded-xl grid place-items-center bg-blue-300 ">
                <span className=" text-sm text-black">Book {num}</span>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Latest Books Banner */}
      <section className="h-[600px] flex items-center justify-center bg-gray-400">
        <div className="mx-auto max-w-6xl w-full px-4 grid items-center gap-8 lg:grid-cols-4">

          {/* Banner Images */}
          {[1, 2, 3, 4].map((num) => (
            <div key={num} className="rounded-2xl border bg-white p-2 shadow-sm">
              <div className="aspect-9/16 rounded-xl bg-blue-300 grid place-items-center">
                <span className="text-black text-sm">Book {num}</span>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Mission Statement Banner */}
      <section className="h-[300px] flex items-center justify-center text-center bg-gray-200">
        <div className="mx-auto max-w-6xl w-full px-4 grid items-center">

          {/* Banner Text */}
          <div>
            <h1 className="font-bold text-2xl mb-4">Mission Statement</h1>
            <p className=" text-gray-600">
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras volutpat non velit eget aliquet. Phasellus tristique viverra enim vitae ultrices. 
              Ut fermentum nulla et nisi commodo aliquet. Aenean dictum odio convallis neque tempor feugiat. Curabitur varius magna odio. Curabitur fringilla iaculis leo et consectetur. 
              Etiam ultrices pulvinar nunc, non tristique neque tempus sed. Etiam aliquam, mi sit amet sagittis semper, dui eros sollicitudin augue, non blandit arcu tellus vitae justo. 
              In at tortor laoreet, auctor mi eu, tincidunt turpis. Nullam orci nisl, consequat eget volutpat feugiat, bibendum quis nunc.
            </p>
          </div>
        </div>
      </section>
    </>
  )
}
 