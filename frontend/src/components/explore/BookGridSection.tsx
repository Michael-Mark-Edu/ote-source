import BookCard from "./BookCard";

export default function BookGridSection({
  backgroundClass = "bg-gray-200",
  count = 5,
  title = "Books",
}: 
{
  backgroundClass?: string;
  count?: number;
  title?: string;
}) 
{
  return (
    <section className={`h-[400px] flex items-center justify-center ${backgroundClass}`}>
      <div className="mx-auto max-w-6xl w-screen px-4 grid items-center gap-8 lg:grid-cols-5">
        {Array.from({ length: count }, (_, i) => (
          <BookCard key={i} title={`${title} ${i + 1}`} />
        ))}
      </div>
    </section>
  );
}
