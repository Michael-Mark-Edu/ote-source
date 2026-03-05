import { Link, useParams } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { getListingById } from "../../api/listings"; // adjust path
import type { BookListingGetDto } from "../../api/listings"; // adjust path

type ListingDetails = {
  id: string;
  isbn: string;
  condition: string;
  purchaseType: string;
  price: string;
  userId: string;
};

function toListingDetails(dto: BookListingGetDto): ListingDetails {
  return {
    id: String(dto.bookListingId),
    isbn: dto.isbn,
    condition: dto.condition,
    purchaseType: dto.purchaseType,
    price: dto.price ?? "—",
    userId: String(dto.userId),
  };
}

export default function ListingPage() {
  const { listingId } = useParams({ from: "/listings/$listingId" });

  const [listing, setListing] = useState<ListingDetails | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setListing(null);
    setError(null);

    (async () => {
      try {
        const dto = await getListingById(listingId);
        setListing(toListingDetails(dto));
      } catch (e) {
        console.error(e);
        setError(e instanceof Error ? e.message : "Failed to load listing");
      }
    })();
  }, [listingId]);

  if (error) {
    return (
      <div className="min-h-screen bg-amber-50">
        <div className="mx-auto max-w-3xl px-4 py-8">
          <Link to="/explore" className="text-sm text-gray-600 hover:text-gray-900">
            ← Back to Explore
          </Link>
          <div className="mt-6 rounded-xl border bg-white p-6">
            <h1 className="text-xl font-semibold">Could not load listing</h1>
            <p className="mt-2 text-sm text-gray-700">{error}</p>
          </div>
        </div>
      </div>
    );
  }

  if (!listing) {
    return (
      <div className="min-h-screen bg-amber-50">
        <div className="mx-auto max-w-3xl px-4 py-8">
          <Link to="/explore" className="text-sm text-gray-600 hover:text-gray-900">
            ← Back to Explore
          </Link>
          <div className="mt-6 rounded-xl border bg-white p-6">
            <h1 className="text-xl font-semibold">Loading…</h1>
            <p className="mt-2 text-sm text-gray-700">Fetching listing details.</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-amber-50">
      <div className="mx-auto max-w-6xl px-4 py-6">
        <div className="mb-4">
          <Link to="/explore" className="text-sm text-gray-600 hover:text-gray-900">
            ← Back to Explore
          </Link>
        </div>

        <div className="grid gap-6 lg:grid-cols-3">
          {/* Image/Card */}
          <div className="lg:col-span-1">
            <div className="rounded-2xl border bg-white p-4 shadow-sm">
              <div className="aspect-9/16 w-full rounded-xl bg-gray-200 grid place-items-center overflow-hidden">
                <span className="text-gray-600 text-sm">No image yet</span>
              </div>

              <div className="mt-4 space-y-2">
                <div className="text-2xl font-semibold">
                  {listing.price === "—" ? "—" : `$${listing.price}`}
                </div>
                <div className="text-sm text-gray-600">Condition: {listing.condition}</div>
                <div className="text-sm text-gray-600">Purchase Type: {listing.purchaseType}</div>

                <button
                  className="mt-2 w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800"
                  type="button"
                  onClick={() => alert("Placeholder: Contact seller")}
                >
                  Contact Seller
                </button>

                <button
                  className="w-full rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
                  type="button"
                  onClick={() => alert("Placeholder: Save listing")}
                >
                  Save Listing
                </button>
              </div>
            </div>
          </div>

          {/* Listing Details */}
          <div className="lg:col-span-2">
            <div className="rounded-2xl border bg-white p-6 shadow-sm">
              <h1 className="text-2xl font-semibold">Listing #{listing.id}</h1>
              <div className="mt-1 text-gray-600">ISBN: {listing.isbn}</div>

              <div className="mt-6 grid gap-4 sm:grid-cols-2">
                <InfoRow label="Listing ID" value={listing.id} />
                <InfoRow label="ISBN" value={listing.isbn} />
                <InfoRow label="Condition" value={listing.condition} />
                <InfoRow label="Purchase Type" value={listing.purchaseType} />
                <InfoRow label="Price" value={listing.price === "—" ? "—" : `$${listing.price}`} />
                <InfoRow label="Seller User ID" value={listing.userId} />
              </div>

              <div className="mt-6">
                <div className="text-sm font-medium text-gray-700">Description</div>
                <p className="mt-2 text-sm text-gray-700 leading-relaxed">
                  Backend DTO doesn’t include description yet.
                </p>
              </div>

              <div className="mt-8 flex flex-wrap gap-3">
                <button
                  className="rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
                  type="button"
                  onClick={() => alert("Placeholder: Edit listing")}
                >
                  Edit
                </button>
                <button
                  className="rounded-lg border border-red-200 bg-red-50 px-4 py-2 text-sm text-red-700 hover:bg-red-100"
                  type="button"
                  onClick={() => alert("Placeholder: Delete listing")}
                >
                  Delete
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function InfoRow({ label, value }: { label: string; value: string }) {
  return (
    <div className="rounded-lg border bg-gray-50 px-4 py-3">
      <div className="text-xs font-medium text-gray-500">{label}</div>
      <div className="mt-1 text-sm text-gray-800">{value}</div>
    </div>
  );
}