import BookCard from "./BookCard";

type Cols = 1 | 2 | 3 | 4 | 5 | 6 | 7;

const colsToClass: Record<Cols, string> = {
  1: "lg:grid-cols-1",
  2: "lg:grid-cols-2",
  3: "lg:grid-cols-3",
  4: "lg:grid-cols-4",
  5: "lg:grid-cols-5",
  6: "lg:grid-cols-6",
  7: "lg:grid-cols-7",
};

export default function BookGridSection({
  backgroundClass = "bg-gray-200",
  count = 5,
  title = "Books",
  columns = 5,
  heightClass = "h-[400px]",
  bookHeight = "w-40",
  startIndex = 0,
  layout = "dense",
}: {
  backgroundClass?: string;
  count?: number;
  title?: string;
  columns?: Cols;
  heightClass?: string;
  bookHeight?: string;
  startIndex?: number;
  layout?: "dense" | "carousel";
}){
  // Dense grid
  const denseColsClass = colsToClass[columns];
  const denseGridClass = `grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 ${denseColsClass} gap-6`;

  // Carousel grid
  const carouselContainerClass = "mx-auto w-full max-w-6xl px-4 py-6 flex flex-wrap gap-6";

  return (
    <section className={`${backgroundClass} ${heightClass}`}>
      <div className="mx-auto w-full max-w-6xl px-4 py-6">
        {layout === "carousel" ? (
          <div className={carouselContainerClass}>
            {Array.from({ length: count }, (_, i) => {
              const idx = startIndex + i;

              const itemClass =
                columns === 4
                  ? "basis-full sm:basis-[48%] lg:basis-[23%]"
                  : columns === 3
                  ? "basis-full sm:basis-[48%] lg:basis-[31%]"
                  : "basis-auto";

              return (
                <div key={idx} className={itemClass}>
                  <BookCard 
                    title={`${title} ${idx + 1}`} 
                    bookHeight={bookHeight} 
                    listingId={String(idx + 1)} 
                  />
                </div>
              );
            })}
          </div>
        ) : (
          <div className={denseGridClass}>
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
        )}
      </div>
    </section>
  );
}