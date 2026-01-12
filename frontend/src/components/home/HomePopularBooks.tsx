import BookGridSection from "../explore/BookGridSection";

export default function HomePopularBooks() {
  return (
    <BookGridSection
      backgroundClass="bg-gray-800"
      heightClass="h-[600px]"
      title="Book"
      count={4}
      columns={4}
    />
  );
}