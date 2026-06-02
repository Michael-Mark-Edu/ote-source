import BookCarousel from "./BookCarousel";

export default function HomePopularBooks() {
  return (
    <section className="bg-gray-800">
      <BookCarousel sectionTitle="Popular Reads" backgroundClass="bg-gray-800" totalBooks={16} perPage={4} />
    </section>
  );
}