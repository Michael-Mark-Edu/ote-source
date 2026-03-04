import BookCarousel from "./BookCarousel";

export default function HomeLatestBooks() {
 return <BookCarousel sectionTitle="Latest Additions" backgroundClass="bg-gray-400" totalBooks={16} perPage={4} />;
}