import { useSearch } from "@tanstack/react-router";
import { useEffect, useMemo, useState } from "react";
import ExploreFiltersSidebar from "./ExploreFiltersSidebar";
import { DEFAULT_FILTERS, type ExploreFilters, type ListingCondition } from "./ExploreFiltersModel";
import ListingCard from "../listings/ListingCard";
import { getListings, getListingPhotos } from "../../api/listings";
import type { BookListingGetDto } from "../../api/listings";
import { getBookByIsbn } from "../../api/books";
import type { BookGetDto } from "../../api/books";

function normalizeCondition(condition: string): ListingCondition | null {
  switch (condition.toLowerCase()) {
    case "new":
      return "new";
    case "like new":
      return "likeNew";
    case "good":
      return "good";
    case "fair":
      return "fair";
    case "poor":
      return "poor";
    default:
      return null;
  }
}

type CardModel = {
  id: string;
  title: string;
  authors: string;
  publishers: string;
  isbn: string;
  price: number;
  condition: string;
  purchaseType: string;
  imageUrl: string | null;
};

export default function ExploreLayout() {
  const { q } = useSearch({ from: "/explore" });

  const [listingDtos, setListingDtos] = useState<BookListingGetDto[]>([]);
  const [booksByIsbn, setBooksByIsbn] = useState<Record<string, BookGetDto | null>>({});
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<ExploreFilters>(DEFAULT_FILTERS);
  const [photoUrlsByListingId, setPhotoUrlsByListingId] = useState<
    Record<string, string | null>
  >({});

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
  }, [listingDtos, booksByIsbn]);

useEffect(() => {
  let cancelled = false;

  async function loadListingPhotos() {
    const photoEntries = await Promise.all(
      listingDtos.map(async (listing) => {
        try {
          const photos = await getListingPhotos(listing.bookListingId);
          const firstPhoto = photos[0];

          return [
            String(listing.bookListingId),
            firstPhoto?.photoUrl ?? null,
          ] as const;
        } catch (error) {
          console.warn(
            `Failed to load photos for listing ${listing.bookListingId}:`,
            error
          );

          return [String(listing.bookListingId), null] as const;
        }
      })
    );

    if (!cancelled) {
      setPhotoUrlsByListingId(Object.fromEntries(photoEntries));
    }
  }

  if (listingDtos.length > 0) {
    loadListingPhotos();
  } else {
    setPhotoUrlsByListingId({});
  }

  return () => {
    cancelled = true;
  };
}, [listingDtos]);

  // Build card models
  const cards: CardModel[] = useMemo(() => {
    return listingDtos.map((l) => {
      const book = booksByIsbn[l.isbn];

      return {
        id: String(l.bookListingId),
        title: book?.title ?? `ISBN: ${l.isbn}`,
        authors: book?.authors ?? "",
        publishers: book?.publishers ?? "",
        isbn: l.isbn,
        price: l.price ? Number(l.price) : 0,
        condition: l.condition,
        purchaseType: l.purchaseType,
        imageUrl: photoUrlsByListingId[String(l.bookListingId)] ?? null,
      };
    });
  }, [listingDtos, booksByIsbn, photoUrlsByListingId]);

  const duplicateIsbns = useMemo(() => {
  const isbnCounts = new Map<string, number>();

  cards.forEach((card) => {
    isbnCounts.set(card.isbn, (isbnCounts.get(card.isbn) ?? 0) + 1);
  });

  return new Set(
    Array.from(isbnCounts.entries())
      .filter(([, count]) => count > 1)
      .map(([isbn]) => isbn)
  );
}, [cards]);

  const filteredCards = useMemo(() => {
  return cards.filter((card) => {
    const minPrice =
      filters.priceMin.trim() === "" ? null : Number(filters.priceMin);

    const maxPrice =
      filters.priceMax.trim() === "" ? null : Number(filters.priceMax);

    const matchesMinPrice = minPrice === null || card.price >= minPrice;
    const matchesMaxPrice = maxPrice === null || card.price <= maxPrice;

    const selectedConditions = Object.entries(filters.condition)
      .filter(([, isSelected]) => isSelected)
      .map(([condition]) => condition as ListingCondition);

    const cardCondition = normalizeCondition(card.condition);

    const matchesCondition =
      selectedConditions.length === 0 ||
      (cardCondition !== null && selectedConditions.includes(cardCondition));

    const matchesIsbn =
      filters.isbn.trim() === "" ||
      card.isbn.toLowerCase().includes(filters.isbn.trim().toLowerCase());

    const matchesAuthor =
      filters.author.trim() === "" ||
      card.authors.toLowerCase().includes(filters.author.trim().toLowerCase());

    const matchesPublisher =
      filters.publisher.trim() === "" ||
      card.publishers
        .toLowerCase()
        .includes(filters.publisher.trim().toLowerCase());

    const matchesHasImage =
      !filters.hasImage || card.imageUrl !== null;

    const matchesDuplicates =
      !filters.duplicates || duplicateIsbns.has(card.isbn);

    const matchesPurchaseType =
      filters.purchaseType === "All" ||
      card.purchaseType === filters.purchaseType;

    const searchQuery = typeof q === "string" ? q.trim().toLowerCase() : "";

    const matchesSearch =
      searchQuery === "" ||
      card.title.toLowerCase().includes(searchQuery) ||
      card.authors.toLowerCase().includes(searchQuery) ||
      card.publishers.toLowerCase().includes(searchQuery) ||
      card.isbn.toLowerCase().includes(searchQuery);

    return matchesSearch && matchesMinPrice && matchesMaxPrice && matchesCondition && matchesIsbn && matchesAuthor && matchesPublisher && matchesHasImage && matchesDuplicates && matchesPurchaseType;
  });
}, [cards, filters, duplicateIsbns, q]);

  function handleHide(id: string) {
    setListingDtos((prev) => prev.filter((l) => String(l.bookListingId) !== id));
  }

  function handleSave(id: string) {
    console.log("Save listing:", id);
    // TODO: call API to save listing for user
  }

  return (
    <div className="flex h-screen bg-indigo-50">
      <ExploreFiltersSidebar onChange={setFilters} />

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
              {filteredCards.map((c) => (
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