import { useSearch } from "@tanstack/react-router";
import ExploreFiltersSidebar from "./ExploreFiltersSidebar";
import BookGridSection from "./BookGridSection";

export default function ExploreLayout() {
  const { q } = useSearch({ from: "/explore" });

  return (
    <div className="flex h-screen">
      <ExploreFiltersSidebar />

      <main className="flex-1 overflow-y-auto">
        <div className="sticky top-0 z-10 bg-white/90 border-b px-4 py-3">
          <div className="text-l text-gray-600">
            {q ? (
              <>
                Showing results for: <span className="font-medium text-gray-900">{q}</span>
              </>
            ) : (
              <>Showing all books</>
            )}
          </div>
        </div>

        <BookGridSection
          layout="dense"
          backgroundClass="bg-gray-200"
          heightClass="h-auto"
          title="Book"
          bookHeight="w-40"
          columns={5}
          count={50} // fix later, this will come from API
        />
      </main>
    </div>
  );
}