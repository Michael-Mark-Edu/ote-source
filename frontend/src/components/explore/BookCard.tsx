import { Link } from "@tanstack/react-router";

export default function BookCard({
  title,
  author,
  bookHeight = "w-40",
  listingId,
  imageUrl,
}: {
  title: string;
  author?: string;
  bookHeight?: string;
  listingId: string;
  imageUrl?: string;
}) {
  return (
    <Link
      to="/listings/$listingId"
      params={{ listingId }}
      className="block"
    >
      <div className="rounded-md border border-gray-300 bg-white p-2 shadow-sm transition hover:shadow-md">
        <div
          className={`aspect-2/3 w-full overflow-hidden rounded bg-gray-100 ${bookHeight}`}
        >
          {imageUrl ? (
            <img
              src={imageUrl}
              alt={title}
              className="h-full w-full object-cover"
            />
          ) : (
            <div className="flex h-full w-full items-center justify-center bg-gray-200 px-2 text-center">
              <span className="text-sm text-gray-700">{title}</span>
            </div>
          )}
        </div>

        <div className="mt-2">
          <p className="line-clamp-2 text-sm font-semibold text-gray-900">
            {title}
          </p>
          {author && (
            <p className="mt-1 line-clamp-1 text-xs text-gray-600">
              {author}
            </p>
          )}
        </div>
      </div>
    </Link>
  );
}