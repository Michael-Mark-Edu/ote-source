import { Link, useParams } from "@tanstack/react-router";
import type { Listing } from "../../types/listing";

type ListingDetails = {
  id: string;
  title: string;
  author: string;
  isbn?: string;
  edition?: string;
  condition: "New" | "Like New" | "Good" | "Fair" | "Poor";
  price: number;
  school?: string;
  course?: string;
  description?: string;
  imageUrl?: string;
  sellerName?: string;
  createdAt?: string;
};

// Mock getListing API
function getMockListing(listingId: string): ListingDetails | null {
  const stored = JSON.parse(localStorage.getItem("listings") || "[]");

  const listing = stored.find((l: Listing) => String(l.id) === listingId);
  if (!listing) return null;

  return {
    id: String(listing.id),
    title: listing.title,
    author: listing.author,
    isbn: listing.isbn,
    edition: listing.edition,
    condition: listing.condition,
    price: listing.price,
    school: listing.campus,
    course: `${listing.subject} ${listing.courseNumber}`,
    description: listing.description,
    imageUrl: listing.image,
    sellerName: listing.sellerName,
    createdAt: listing.createdAt
  };
}

export default function ListingPage() {
  const { listingId } = useParams({ from: "/listings/$listingId" });

  // TODO: replace with real API call + loading states
  const listing = getMockListing(String(listingId));

  if (!listing) {
  return (
    <div className="min-h-screen bg-amber-50">
      <div className="mx-auto max-w-3xl px-4 py-8">
        <Link to="/explore" className="text-sm text-gray-600 hover:text-gray-900">
          ← Back to Explore
        </Link>
        <div className="mt-6 rounded-xl border bg-white p-6">
          <h1 className="text-xl font-semibold">Listing not found</h1>
          <p className="mt-2 text-sm text-gray-700">
            This listing may have been removed, or your mock data hasn’t been seeded yet.
          </p>
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
                {listing.imageUrl ? (
                  <img src={listing.imageUrl} alt={listing.title} className="h-full w-full object-cover" />
                ) : (
                  <span className="text-gray-600 text-sm">Cover Image</span>
                )}
              </div>

              <div className="mt-4 space-y-2">
                <div className="text-2xl font-semibold">${listing.price}</div>
                <div className="text-sm text-gray-600">Condition: {listing.condition}</div>

                <button
                  className="mt-2 w-full rounded-lg bg-gray-900 px-4 py-2 text-sm text-white hover:bg-gray-800"
                  type="button"
                  onClick={() => {
                    // TODO: message seller / request / etc
                    alert("Placeholder: Contact seller");
                  }}
                >
                  Contact Seller
                </button>

                <button
                  className="w-full rounded-lg border border-gray-300 px-4 py-2 text-sm hover:bg-gray-50"
                  type="button"
                  onClick={() => {
                    // TODO: save listing endpoint
                    alert("Placeholder: Save listing");
                  }}
                >
                  Save Listing
                </button>
              </div>
            </div>
          </div>

          {/* Listing Details */}
          <div className="lg:col-span-2">
            <div className="rounded-2xl border bg-white p-6 shadow-sm">
              <h1 className="text-2xl font-semibold">{listing.title}</h1>
              <div className="mt-1 text-gray-600">by {listing.author}</div>

              <div className="mt-6 grid gap-4 sm:grid-cols-2">
                <InfoRow label="Listing ID" value={listing.id} />
                <InfoRow label="ISBN" value={listing.isbn ?? "—"} />
                <InfoRow label="Edition" value={listing.edition ?? "—"} />
                <InfoRow label="School" value={listing.school ?? "—"} />
                <InfoRow label="Course" value={listing.course ?? "—"} />
                <InfoRow label="Posted" value={listing.createdAt ?? "—"} />
                <InfoRow label="Seller" value={listing.sellerName ?? "—"} />
              </div>

              <div className="mt-6">
                <div className="text-sm font-medium text-gray-700">Description</div>
                <p className="mt-2 text-sm text-gray-700 leading-relaxed">
                  {listing.description ?? "No description provided."}
                </p>
              </div>

              {/* Listing Owner Actions Placeholder */}
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