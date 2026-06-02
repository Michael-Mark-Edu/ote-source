import { useState } from "react";
import { Link } from "@tanstack/react-router";
import { BookmarkIcon, EyeSlashIcon } from "@heroicons/react/24/outline";
import { isListingSaved, toggleSavedListing } from "./savedListings";

export default function ListingCard({
  listingId,
  title,
  price,
  imageUrl,
  onSave,
  onHide,
}: {
  listingId: string;
  title: string;
  price: number;
  imageUrl?: string | null;
  onSave?: (id: string) => void;
  onHide?: (id: string) => void;
}) {
  const [saved, setSaved] = useState(() => isListingSaved(listingId));

  return (
    <div className="relative">
      <Link
        to="/listings/$listingId"
        params={{ listingId }}
        className="block"
      >
        <div className="rounded-2xl border bg-white shadow-sm hover:shadow-md transition overflow-hidden aspect-square">
          {/* Image Section */}
          <div className="relative h-[70%] bg-gray-50 flex items-center justify-center">
            {/* Price Tag */}
            <div className="absolute left-2 top-2 rounded-md bg-white px-2 py-1 text-sm font-semibold text-green-700 border">
              ${price}
            </div>

            {imageUrl ? (
              <img
                src={imageUrl}
                alt={title}
                className="max-h-full max-w-full object-contain p-3"
                loading="lazy"
              />
            ) : (
              <div className="h-full w-full grid place-items-center text-gray-400 text-sm">
                No image
              </div>
            )}
          </div>

          {/* Title */}
          <div className="h-[30%] px-4 py-3 flex flex-col justify-between">
            <div className="text-blue-700 text-xl leading-snug line-clamp-2">
              {title}
            </div>

            <div className="flex justify-between items-center mt-2">
              {/* Save Listing Icon */}
              <button
                type="button"
                onClick={(e) => {
                  e.preventDefault();
                  e.stopPropagation();

                  toggleSavedListing(listingId);
                  setSaved(isListingSaved(listingId));

                  onSave?.(listingId);
                }}
                className="p-1 rounded hover:bg-gray-100"
                aria-label={saved ? "Unsave listing" : "Save listing"}
              >
                <BookmarkIcon
                  className={`h-8 w-8 ${
                    saved
                      ? "fill-gray-700 text-gray-700"
                      : "text-gray-600 hover:text-black"
                  }`}
                />
              </button>

              {/* Hide Listing Icon */}
              <button
                type="button"
                onClick={(e) => {
                  e.preventDefault();
                  e.stopPropagation();
                  onHide?.(listingId);
                }}
                className="p-1 rounded hover:bg-gray-100"
                aria-label="Hide listing"
              >
                <EyeSlashIcon className="h-8 w-8 text-gray-600 hover:text-black" />
              </button>
            </div>
          </div>
        </div>
      </Link>
    </div>
  );
}