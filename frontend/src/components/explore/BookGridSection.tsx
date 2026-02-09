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
}: {
  backgroundClass?: string;
  count?: number;
  title?: string;
  columns?: Cols;
  heightClass?: string;
  bookHeight?: string;
}) {
  const colClass = colsToClass[columns];

  return (
    <section className={`${heightClass} flex items-center-safe justify-center-safe ${backgroundClass}`}>
      <div className={`mx-auto p-6 w-auto px-4 flex flex-wrap items-center-safe gap-8 ${colClass}`}>
          {Array.from({ length: count }, (_, i) => (
            <BookCard key={i} title={`${title} ${i + 1}`} bookHeight={`${bookHeight}`} />
          ))}
      </div>
    </section>
  );
}

{/**export default function Bleepboop () {
  return (
    <>
    <section className="bg-gray-200">
      { /** Book listing Rows and Columns 
      { /** NEW FLEX GRID 
      <main className="flex-1 overflow-y-auto">
        <h1 className="text-2x1 text-white">
          { /** trying to fit beau's code into my code here 
          <section className="h-auto items-center justify-center bg-gray-200">
            <div className="mx-auto p-6 w-auto px-4 flex flex-wrap items-center gap-8 ">
              { /** TODO: Implement BookCard 
              {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20].map((num) => (
                <div key={num} className="rounded-2xl border p-2 shadow-sm bg-white">
                  <div className="aspect-9/16 place-items-center w-40 grid rounded-xl bg-blue-300 ">
                    <span className=" text-sm text-black">Book {num}</span>
                  </div>
                </div>
              ))}
            </div>
          </section>
        </h1>
      </main>
      </section>
    </>
  )
}*/}

{ /** Beau's code, keeping here for backup purposes */}
{ /** type Cols = 1 | 2 | 3 | 4 | 5 | 6;

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
} */}
