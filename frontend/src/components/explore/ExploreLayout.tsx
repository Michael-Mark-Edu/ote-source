import ExploreFiltersSidebar from "./ExploreFiltersSidebar";
import ExploreSearchBar from "./ExploreSearchBar";
import BookGridSection from "./BookGridSection";

export default function ExploreLayout() {
  return (
    <div className="flex h-screen">
      <ExploreFiltersSidebar />

      <main className="flex-1 overflow-y-auto">
        <ExploreSearchBar />

        {/* BookGridSection */}
        <BookGridSection backgroundClass="bg-gray-200" title="Book" />
        <BookGridSection backgroundClass="bg-gray-300" title="Book" />
        <BookGridSection backgroundClass="bg-gray-200" title="Book" />
        <BookGridSection backgroundClass="bg-gray-300" title="Book" />
      </main>
    </div>
  );
}
