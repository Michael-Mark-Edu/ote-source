import { Link } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { getListingById } from "../../../api/listings";
import type { BookListingGetDto } from "../../../api/listings";
import { getBookByIsbn } from "../../../api/books";
import { getSavedListingIds, unsaveListing } from "../../listings/savedListings";

type SavedListingCard = {
  id: string;
  title: string;
  isbn: string;
  price: string;
  condition: string;
  purchaseType: string;
};

export default function SavedListingsTab() {
  const [savedListings, setSavedListings] = useState<SavedListingCard[]>([]);
  const [loading, setLoading] = useState(true);

  async function loadSavedListings() {
    setLoading(true);

    const savedIds = getSavedListingIds();

    try {
      const listingCards = await Promise.all(
        savedIds.map(async (listingId) => {
          const listing: BookListingGetDto = await getListingById(listingId);

          let title = `Listing #${listing.bookListingId}`;

          try {
            const book = await getBookByIsbn(listing.isbn);

            if (book) {
              title = book.title;
            }
          } catch (bookError) {
            console.warn("Failed to load book title:", bookError);
          }

          return {
            id: String(listing.bookListingId),
            title,
            isbn: listing.isbn,
            price: listing.price ?? "—",
            condition: listing.condition,
            purchaseType: listing.purchaseType,
          };
        })
      );

      setSavedListings(listingCards);
    } catch (error) {
      console.error("Failed to load saved listings:", error);
      setSavedListings([]);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadSavedListings();
  }, []);

  function handleUnsave(listingId: string) {
    unsaveListing(listingId);
    setSavedListings((current) =>
      current.filter((listing) => listing.id !== listingId)
    );
  }

  if (loading) {
    return (
      <div className="rounded-xl border bg-white p-6 shadow-sm">
        <h2 className="mb-2 text-lg font-semibold">Saved Listings</h2>
        <p className="text-sm text-gray-600">Loading saved listings...</p>
      </div>
    );
  }

  return (
    <div className="rounded-xl border bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-lg font-semibold">Saved Listings</h2>

      {savedListings.length === 0 ? (
        <p className="text-sm text-gray-600">
          You have not saved any listings yet.
        </p>
      ) : (
        <div className="space-y-3">
          {savedListings.map((listing) => (
            <div
              key={listing.id}
              className="flex flex-col gap-3 rounded-lg border border-gray-200 bg-gray-50 p-4 sm:flex-row sm:items-center sm:justify-between"
            >
              <div>
                <Link
                  to="/listings/$listingId"
                  params={{ listingId: listing.id }}
                  className="font-medium text-blue-700 hover:underline"
                >
                  {listing.title}
                </Link>

                <p className="mt-1 text-sm text-gray-600">
                  ISBN: {listing.isbn}
                </p>

                <p className="mt-1 text-sm text-gray-600">
                  {listing.condition} · {listing.purchaseType} · ${listing.price}
                </p>
              </div>

              <button
                type="button"
                onClick={() => handleUnsave(listing.id)}
                className="rounded border border-gray-300 bg-white px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100"
              >
                Remove
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}