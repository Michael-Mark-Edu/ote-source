import { useSearch } from "@tanstack/react-router";
import { useEffect, useMemo, useState } from "react";
import ExploreFiltersSidebar from "./ExploreFiltersSidebar";
import ListingCard from "../listings/ListingCard";
import { getListings } from "../../api/listings";
import type { BookListingGetDto } from "../../api/listings";
import { getBookByIsbn } from "../../api/books";
import type { BookGetDto } from "../../api/books";

type CardModel = {
  id: string;
  title: string;
  price: number;
  imageUrl: string | null;
};

export default function ExploreLayout() {
  const { q } = useSearch({ from: "/explore" });

  const [listingDtos, setListingDtos] = useState<BookListingGetDto[]>([]);
  const [booksByIsbn, setBooksByIsbn] = useState<Record<string, BookGetDto | null>>({});
  const [error, setError] = useState<string | null>(null);

  // Load listings
  useEffect(() => {
    (async () => {
      try {
        setError(null);
        const data = await getListings();
        setListingDtos(data);
      } catch (e) {
        console.error(e);
        setError(e instanceof Error ? e.message : "Failed to load listings");
      }
    })();
  }, []);

  // Load books for unique ISBNs found in listings
  useEffect(() => {
    (async () => {
      const isbns = Array.from(new Set(listingDtos.map((l) => l.isbn))).filter(Boolean);

      // Fetch ISBNs we haven't fetched yet
      const toFetch = isbns.filter((isbn) => booksByIsbn[isbn] === undefined);
      if (toFetch.length === 0) return;

      const results = await Promise.all(
        toFetch.map(async (isbn) => {
          try {
            const book = await getBookByIsbn(isbn); // returns BookGetDto
            return [isbn, book] as const;
          } catch (e) {
            console.warn("Failed to fetch book for ISBN:", isbn, e);
            return [isbn, null] as const;
          }
        })
      );

      setBooksByIsbn((prev) => {
        const next = { ...prev };
        for (const [isbn, book] of results) next[isbn] = book;
        return next;
      });
    })();
    
  });

  // Build card models
  const cards: CardModel[] = useMemo(() => {
    return listingDtos.map((l) => {
      const book = booksByIsbn[l.isbn];

      return {
        id: String(l.bookListingId),
        title: book?.title ?? `ISBN: ${l.isbn}`,
        price: l.price ? Number(l.price) : 0,
        imageUrl: null,
      };
    });
  }, [listingDtos, booksByIsbn]);

  function handleHide(id: string) {
    setListingDtos((prev) => prev.filter((l) => String(l.bookListingId) !== id));
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
          {error ? (
            <div className="rounded-xl border bg-white p-4 text-sm text-red-700">{error}</div>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-12">
              {cards.map((c) => (
                <ListingCard
                  key={c.id}
                  listingId={c.id}
                  title={c.title}
                  price={c.price}
                  imageUrl={c.imageUrl}
                  onSave={handleSave}
                  onHide={handleHide}
                />
              ))}
            </div>
          )}
        </div>
      </main>
    </div>
  );
}