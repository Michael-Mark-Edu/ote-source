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
        <BookGridSection backgroundClass="bg-gray-200" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-300" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-200" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-300" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-200" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-300" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
        <BookGridSection backgroundClass="bg-gray-200" heightClass="h-[1000]" title="Book" bookHeight="w-40" count={6} />
      </main>
    </div>
  );
}
