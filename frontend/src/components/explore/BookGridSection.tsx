import BookCard from "./BookCard";

const prototypeBooks = [
  {
    listingId: "1",
    title: "Calculus",
    author: "Used textbook",
    imageUrl: "/mock-books/comparch.jpg",
  },
  {
    listingId: "2",
    title: "Physics",
    author: "Used textbook",
    imageUrl: "/mock-books/comptheory.jpg",
  },
  {
    listingId: "3",
    title: "Software Engineering",
    author: "Used textbook",
    imageUrl: "/mock-books/Cpluslearn.jpg",
  },
  {
    listingId: "4",
    title: "Computer Networking",
    author: "Used textbook",
    imageUrl: "/mock-books/discretemath.jpg",
  },
]

export default function BookGridSection({
  backgroundClass = "bg-gray-200",
  count = 4,
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
}) {
  const visibleBooks = prototypeBooks.slice(startIndex, startIndex + count);

  return (
    <section className={`${backgroundClass} ${heightClass}`}>
      <div className="mx-auto w-full max-w-6xl px-4 py-6">
        <div className="grid grid-cols-2 gap-6 lg:grid-cols-4">
          {visibleBooks.map((book) => (
            <BookCard
              key={book.listingId}
              title={book.title}
              author={book.author}
              bookHeight={bookHeight}
              listingId={book.listingId}
              imageUrl={book.imageUrl}
            />
          ))}
        </div>
      </div>
    </section>
  );
}