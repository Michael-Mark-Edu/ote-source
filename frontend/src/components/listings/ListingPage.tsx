import { Link, useParams } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { getListingById, getListingPhotos, type ListingPhotoDto, type BookListingGetDto } from "../../api/listings";
import { getBookByIsbn } from "../../api/books";
import { getUserById } from "../../api/users";
import { isListingSaved, toggleSavedListing } from "./savedListings";


type ListingDetails = {
  id: string;
  isbn: string;
  title: string;
  description: string;
  authors: string;
  publishers: string;
  condition: string;
  purchaseType: string;
  price: string;
  userId: string;
  sellerUsername: string;
  sellerEmail: string;
};

function toListingDetails(
  dto: BookListingGetDto,
  bookTitle = `Textbook Listing #${dto.bookListingId}`,
  bookDescription = "No description available.",
  authors = "Unknown author",
  publishers = "Unknown publisher",
  sellerUsername = `User #${dto.userId}`,
  sellerEmail = ""
): ListingDetails {
  return {
    id: String(dto.bookListingId),
    isbn: dto.isbn,
    title: bookTitle,
    description: bookDescription,
    authors,
    publishers,
    condition: dto.condition,
    purchaseType: dto.purchaseType,
    price: dto.price ?? "—",
    userId: String(dto.userId),
    sellerUsername,
    sellerEmail,
  };
}

export default function ListingPage() {
  const { listingId } = useParams({ from: "/listings/$listingId" });
  const [listing, setListing] = useState<ListingDetails | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [showSellerEmail, setShowSellerEmail] = useState(false);
  const [saved, setSaved] = useState(false);
  const [photos, setPhotos] = useState<ListingPhotoDto[]>([]);

  useEffect(() => {
    setListing(null);
    setError(null);
    setShowSellerEmail(false);
    setPhotos([]);

    (async () => {
      try {
        const dto = await getListingById(listingId);
        setSaved(isListingSaved(String(dto.bookListingId)));

        let bookTitle = `Textbook Listing #${dto.bookListingId}`;
        let bookDescription = "No description available.";
        let authors = "Unknown author";
        let publishers = "Unknown publisher";
        let sellerUsername = `User #${dto.userId}`;
        let sellerEmail = "";

        try {
          const book = await getBookByIsbn(dto.isbn);

          if (book) {
            bookTitle = book.title;
            bookDescription = book.description ?? "No description available.";
            authors = book.authors;
            publishers = book.publishers;
          }
        } catch (bookError) {
          console.warn("Failed to load book information:", bookError);
        }

        try {
          const seller = await getUserById(dto.userId);
          sellerUsername = seller.username;
          sellerEmail = seller.emailAddress;
        } catch (sellerError) {
          console.warn("Failed to load seller information:", sellerError);
        }

        try {
          const listingPhotos = await getListingPhotos(dto.bookListingId);
          setPhotos(listingPhotos);
        } catch (photoError) {
          console.warn("Failed to load listing photos:", photoError);
          setPhotos([]);
        }

        setListing(toListingDetails(dto, bookTitle, bookDescription, authors, publishers, sellerUsername, sellerEmail));
      } catch (e) {
        console.error(e);
        setError(e instanceof Error ? e.message : "Failed to load listing");
      }
    })();
  }, [listingId]);

  if (error) {
    return (
      <div className="min-h-screen bg-neutral-100">
        <div className="mx-auto max-w-5xl px-4 py-6">
          <Link to="/explore" className="text-sm text-blue-700 hover:underline">
            ← back to listings
          </Link>

          <div className="mt-6 border border-gray-300 bg-white p-6">
            <h1 className="text-xl font-semibold">Could not load listing</h1>
            <p className="mt-2 text-sm text-gray-700">{error}</p>
          </div>
        </div>
      </div>
    );
  }

  if (!listing) {
    return (
      <div className="min-h-screen bg-neutral-100">
        <div className="mx-auto max-w-5xl px-4 py-6">
          <Link to="/explore" className="text-sm text-blue-700 hover:underline">
            ← back to listings
          </Link>

          <div className="mt-6 border border-gray-300 bg-white p-6">
            <h1 className="text-xl font-semibold">Loading…</h1>
            <p className="mt-2 text-sm text-gray-700">Fetching listing details.</p>
          </div>
        </div>
      </div>
    );
  }

  const displayPrice = listing.price === "—" ? "Price not listed" : `$${listing.price}`;



  return (
    <div className="min-h-screen bg-amber-50">
      <main className="mx-auto max-w-5xl px-4 py-6">
        <div className="mb-4">
          <Link to="/explore" className="text-sm text-blue-700 hover:underline">
            ← back to listings
          </Link>
        </div>

        <div className="border border-gray-300 bg-white">
          {/* Header */}
          <div className="border-b border-gray-300 px-5 py-4">
            <h1 className="text-2xl font-semibold text-gray-900">
              {listing.title}
            </h1>

            <div className="mt-2 flex flex-wrap gap-x-4 gap-y-1 text-sm text-gray-600">
              <Link
                to="/users/$userId"
                params={{ userId: listing.userId }}
                className="text-blue-700 hover:underline"
              >
                {listing.sellerUsername}
              </Link>
              <span>Oregon Tech</span>
            </div>
          </div>

          {/* Body */}
          <div className="grid gap-6 p-5 lg:grid-cols-[minmax(0,2fr)_320px]">
            {/* Left side */}
            <div>
              <div className="flex h-[420px] items-center justify-center border border-gray-300 bg-gray-100">
                {photos.length > 0 ? (
                  <img
                    src={photos[0].photoUrl}
                    alt={`Listing photo ${photos[0].photoIndex}`}
                    className="h-full w-full object-contain"
                  />
                ) : (
                  <span className="text-sm text-gray-500">No image uploaded yet</span>
                )}
              </div>

              <section className="mt-6">
                <h2 className="border-b border-gray-300 pb-2 text-lg font-semibold text-gray-900">
                  Description
                </h2>

                <p className="mt-3 text-sm leading-6 text-gray-700">
                  {listing.description}
                </p>
              </section>

              <section className="mt-6">
                <h2 className="border-b border-gray-300 pb-2 text-lg font-semibold text-gray-900">
                  Listing Details
                </h2>

                <dl className="mt-3 grid gap-y-2 text-sm sm:grid-cols-[140px_1fr]">
                  <dt className="font-medium text-gray-600">ISBN</dt>
                  <dd className="text-gray-900">{listing.isbn}</dd>

                  <dt className="font-medium text-gray-600">Author(s)</dt>
                  <dd className="text-gray-900">{listing.authors}</dd>

                  <dt className="font-medium text-gray-600">Publisher(s)</dt>
                  <dd className="text-gray-900">{listing.publishers}</dd>

                  <dt className="font-medium text-gray-600">Condition</dt>
                  <dd className="text-gray-900">{listing.condition}</dd>

                  <dt className="font-medium text-gray-600">Exchange type</dt>
                  <dd className="text-gray-900">{listing.purchaseType}</dd>
                </dl>
              </section>
            </div>

            {/* Right side */}
            <aside className="border border-gray-300 bg-neutral-50 p-4">
              <div className="text-3xl font-semibold text-gray-900">
                {displayPrice}
              </div>

              <div className="mt-4 space-y-2 text-sm text-gray-700">
                <div>
                  <span className="font-medium">Condition:</span>{" "}
                  {listing.condition}
                </div>
                <div>
                  <span className="font-medium">Exchange:</span>{" "}
                  {listing.purchaseType}
                </div>
                <div>
                  <span className="font-medium">Seller:</span>{" "}
                  <Link
                    to="/users/$userId"
                    params={{ userId: listing.userId }}
                    className="text-blue-700 hover:underline"
                  >
                    {listing.sellerUsername}
                  </Link>
                </div>
              </div>

              {listing.sellerEmail ? (
                <div className="mt-5 space-y-2">
                  <button
                    type="button"
                    onClick={() => setShowSellerEmail((current) => !current)}
                    className="w-full bg-blue-700 px-4 py-2 text-sm font-medium text-white hover:bg-blue-800"
                  >
                    {showSellerEmail ? "Hide Seller Email" : "Contact Seller"}
                  </button>

                  {showSellerEmail && (
                    <div className="rounded border border-gray-300 bg-white p-3 text-center">
                      <p className="text-sm font-medium text-gray-900">
                        {listing.sellerEmail}
                      </p>

                      <button
                        type="button"
                        onClick={() => navigator.clipboard.writeText(listing.sellerEmail)}
                        className="mt-2 rounded border border-gray-300 bg-gray-50 px-3 py-2 text-sm font-medium text-gray-800 hover:bg-gray-100"
                      >
                        Copy email
                      </button>
                    </div>
                  )}
                </div>
              ) : (
                <button
                  className="mt-5 w-full cursor-not-allowed bg-gray-400 px-4 py-2 text-sm font-medium text-white"
                  type="button"
                  disabled
                >
                  Contact Seller Unavailable
                </button>
              )}
              <Link
                to="/users/$userId"
                params={{ userId: listing.userId }}
                className="mt-3 block w-full border border-gray-400 bg-white px-4 py-2 text-center text-sm font-medium text-gray-800 hover:bg-gray-100"
              >
                View Seller Profile
              </Link>

              <button
                className="mt-3 w-full border border-gray-400 bg-white px-4 py-2 text-sm font-medium text-gray-800 hover:bg-gray-100"
                type="button"
                onClick={() => {
                  toggleSavedListing(listing.id);
                  setSaved(isListingSaved(listing.id));
                }}
              >
                {saved ? "Unsave Listing" : "Save Listing"}
              </button>

              <div className="mt-5 border-t border-gray-300 pt-4 text-xs leading-5 text-gray-600">
                Meet in a public place and verify the textbook before completing
                the exchange.
              </div>
            </aside>
          </div>
        </div>
      </main>
    </div>
  );
}