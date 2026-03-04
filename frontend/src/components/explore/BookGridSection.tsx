import BookCard from "./BookCard";

export default function BookGridSection({
  backgroundClass = "bg-gray-200",
  count = 4,
  title = "Books",
  heightClass = "h-auto",
  bookHeight = "w-48",
  startIndex = 0,
}: {
  backgroundClass?: string;
  count?: number;
  title?: string;
  heightClass?: string;
  bookHeight?: string;
  startIndex?: number;
}){
  return (
    <section className={`${backgroundClass} ${heightClass}`}>
      <div className="mx-auto w-full max-w-6xl px-4 py-6">
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-6">
          {Array.from({ length: count }, (_, i) => {
            const idx = startIndex + i;

            return (
              <BookCard
                key={idx}
                title={`${title} ${idx + 1}`}
                bookHeight={bookHeight}
                listingId={String(idx + 1)}
              />
            );
          })}
        </div>
      </div>
    </section>
  );
}