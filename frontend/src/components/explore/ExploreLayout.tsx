import { useSearch } from "@tanstack/react-router";
import { useState } from "react";
import ExploreFiltersSidebar from "./ExploreFiltersSidebar";
import ListingCard from "../listings/ListingCard";

export default function ExploreLayout() {
  const { q } = useSearch({ from: "/explore" });

  const initialListings = Array.from({ length: 30 }, (_, i) => ({
    id: String(i + 1),
    title: `The art of mad max fury road ${i + 1}`,
    price: 20,
    imageUrl: null
  }));

  const [listings, setListings] = useState(initialListings);

  function handleHide(id: string) {
    setListings((prev) => prev.filter((l) => l.id !== id));
  }

  function handleSave(id: string) {
    console.log("Save listing:", id);
    // TODO: call API to save listing for user
  }

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

        <div className="mx-auto max-w-8xl px-4 py-6">
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-12">
            {listings.map((l) => (
              <ListingCard
                key={l.id}
                listingId={l.id}
                title={l.title}
                price={l.price}
                imageUrl={l.imageUrl}
                onSave={handleSave}
                onHide={handleHide}
              />
            ))}
          </div>
        </div>
      </main>
    </div>
  );
}