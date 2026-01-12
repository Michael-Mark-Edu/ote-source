import BookCard from "./BookCard";

type Cols = 1 | 2 | 3 | 4 | 5 | 6;

const colsToClass: Record<Cols, string> = {
  1: "lg:grid-cols-1",
  2: "lg:grid-cols-2",
  3: "lg:grid-cols-3",
  4: "lg:grid-cols-4",
  5: "lg:grid-cols-5",
  6: "lg:grid-cols-6",
};

export default function BookGridSection({
  backgroundClass = "bg-gray-200",
  count = 5,
  title = "Books",
  columns = 5,
  heightClass = "h-[400px]",
}: {
  backgroundClass?: string;
  count?: number;
  title?: string;
  columns?: Cols;
  heightClass?: string;
}) {
  const colClass = colsToClass[columns];

  return (
    <section className={`${heightClass} flex items-center justify-center ${backgroundClass}`}>
      <div className={`mx-auto max-w-6xl w-full px-4 grid items-center gap-8 ${colClass}`}>
        {Array.from({ length: count }, (_, i) => (
          <BookCard key={i} title={`${title} ${i + 1}`} />
        ))}
      </div>
    </section>
  );
}