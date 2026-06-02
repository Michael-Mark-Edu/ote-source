import { Link } from "@tanstack/react-router";
import { useContext, useEffect, useState } from "react";
import { AuthContext } from "../../auth/AuthContext";
import { getListings, type BookListingGetDto } from "../../../api/listings";
import { getBookByIsbn } from "../../../api/books";

type MyListingCard = {
  id: string;
  title: string;
  isbn: string;
  price: string;
  condition: string;
  purchaseType: string;
};

export default function MyListingsTab() {
  const auth = useContext(AuthContext);

  const currentUserId = auth?.user ? Number(auth.user.id) : null;

  const [myListings, setMyListings] = useState<MyListingCard[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadMyListings() {
      if (!currentUserId) {
        setMyListings([]);
        setLoading(false);
        return;
      }

      setLoading(true);

      try {
        const listings = await getListings();

        const currentUserListings = listings.filter(
          (listing: BookListingGetDto) => listing.userId === currentUserId
        );

        const listingCards = await Promise.all(
          currentUserListings.map(async (listing) => {
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

        setMyListings(listingCards);
      } catch (error) {
        console.error("Failed to load my listings:", error);
        setMyListings([]);
      } finally {
        setLoading(false);
      }
    }

    loadMyListings();
  }, [currentUserId]);

  if (!currentUserId) {
    return (
      <div className="rounded-xl border bg-white p-6 shadow-sm">
        <h2 className="mb-2 text-lg font-semibold">My Listings</h2>
        <p className="text-sm text-gray-600">
          Log in to view the listings you have created.
        </p>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="rounded-xl border bg-white p-6 shadow-sm">
        <h2 className="mb-2 text-lg font-semibold">My Listings</h2>
        <p className="text-sm text-gray-600">Loading your listings...</p>
      </div>
    );
  }

  return (
    <div className="rounded-xl border bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-lg font-semibold">My Listings</h2>

      {myListings.length === 0 ? (
        <p className="text-sm text-gray-600">
          You have not created any listings yet.
        </p>
      ) : (
        <div className="space-y-3">
          {myListings.map((listing) => (
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

              <Link
                to="/listings/$listingId"
                params={{ listingId: listing.id }}
                className="rounded border border-gray-300 bg-white px-3 py-2 text-center text-sm font-medium text-gray-700 hover:bg-gray-100"
              >
                View
              </Link>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}