import BookGridSection from "../explore/BookGridSection";

export default function HomePopularBooks() {
  return (
    <>
    <section className="h-auto justify-items-center-safe justify-center bg-gray-800">
          <div className="mx-auto p-6 w-auto px-4 flex flex-wrap justify-items-center-safe justify-center-safe gap-8">
            <h1 className="text-white font-bold mt-3 text-5xl">
              Popular Reads
            </h1>
            <BookGridSection
              backgroundClass="bg-gray-800"
              heightClass="h-auto"
              title="Book"
              count={4}
              columns={4}
              bookHeight="w-70"
            />
          </div> 
      </section>
    </>
  );
}