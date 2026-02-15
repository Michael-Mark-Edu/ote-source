import { useMemo, useState } from "react";
import BookGridSection from "../explore/BookGridSection";

export default function BookCarousel({
  sectionTitle,
  totalBooks = 16,
  perPage = 4,
  backgroundClass,
}: {
  sectionTitle: string;
  totalBooks?: number;
  perPage?: number;
  backgroundClass: string;
}) {
  const [page, setPage] = useState(0);

  const maxPage = useMemo(
    () => Math.max(0, Math.ceil(totalBooks / perPage) - 1),
    [totalBooks, perPage]
  );

  const canPrev = page > 0;
  const canNext = page < maxPage;

  const startIndex = page * perPage;

  return (
    <section className="h-auto">
      <div className="mx-auto w-full max-w-6xl p-6 px-4 flex flex-col items-center gap-6">
        <h1 className="text-white font-bold mt-3 text-5xl">{sectionTitle}</h1>

        <div className="relative w-full">
          {/* Left Button */}
          <button
            onClick={() => canPrev && setPage((p) => p - 1)}
            disabled={!canPrev}
            className="absolute left-0 top-1/2 -translate-y-1/2 z-10 rounded-full bg-white/90 px-3 py-2 shadow hover:bg-white disabled:opacity-40"
            aria-label="Previous"
          >
            ‹
          </button>

          {/* Center Grid Section */}
          <div className="px-10">
            <BookGridSection
              backgroundClass={backgroundClass}
              heightClass="h-auto"
              title="Book"
              count={perPage}
              columns={4}
              bookHeight="w-70"
              startIndex={startIndex}
              layout="carousel"
            />
          </div>

          {/* Right Button*/}
          <button
            onClick={() => canNext && setPage((p) => p + 1)}
            disabled={!canNext}
            className="absolute right-0 top-1/2 -translate-y-1/2 z-10 rounded-full bg-white/90 px-3 py-2 shadow hover:bg-white disabled:opacity-40"
            aria-label="Next"
          >
            ›
          </button>
        </div>

        <div className="text-white/70 text-sm">
          Showing {startIndex + 1}–{Math.min(startIndex + perPage, totalBooks)} of {totalBooks}
        </div>
      </div>
    </section>
  );
}