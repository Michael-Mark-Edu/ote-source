import BookGridSection from "../explore/BookGridSection";

export default function HomeLatestBooks() {
  return (
    <BookGridSection
      backgroundClass="bg-gray-400"
      heightClass="h-[600px]"
      title="Book"
      count={4}
      columns={4}
    />
  );
}