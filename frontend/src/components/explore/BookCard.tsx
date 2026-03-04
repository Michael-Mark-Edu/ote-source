import { Link } from "@tanstack/react-router";

export default function BookCard({
  title,
  bookHeight = "w-40",
  listingId,
}: {
  title: string;
  bookHeight?: string;
  listingId: string;
}) {
  return (
    <Link
      to="/listings/$listingId"
      params={{ listingId }}
      className="block"
    >
      <div className="rounded-2xl border p-2 shadow-sm bg-white hover:shadow-md transition">
        <div className={`aspect-9/16 rounded-xl grid place-items-center-safe bg-blue-300 w-full ${bookHeight}`}>
          <span className="text-sm text-black">{title}</span>
        </div>
      </div>
    </Link>
  );
}