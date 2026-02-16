import { Link } from "@tanstack/react-router";

export default function HomeBanner() {
  return (
    <section className="relative w-full h-[450px] overflow-hidden">
      <div className="absolute inset-0 ">
        <div className="h-full w-full bg-blue-300"/>
          {/*<img src="" className="h-full w-full object-cover/>*/}
        </div>

        <div className="absolute inset-0 bg-black/45"/>

        <div className="relative h-full">
          <div className="flex h-full items-center justify-center">
            <div className="max-w-xl text-white">
              <h1 className="font-bold text-5xl leading-tight">Find, Sell, or Trade Your Textbooks</h1>
              <p className="mt-3 text-white/90">Browse listings from local students.</p>
              
              <div className="mt-6 flex gap-3">
                <Link to="/explore"className="rounded-xl bg-white px-5 py-3 font-medium text-gray-900 hover:bg-gray-100 inline-block">Browse</Link>
              </div>
            </div>
          </div>
        </div>
    </section>
  );
}