import { MagnifyingGlassIcon, XMarkIcon } from "@heroicons/react/24/outline";

export default function HeaderSearch({
  query,
  setQuery,
  onSubmit,
  onClear,
}: {
  query: string;
  setQuery: (val: string) => void;
  onSubmit: () => void;
  onClear: () => void;
}) {
  return (
    <div className="flex-1 flex justify-center pr-16">
      <form
        className="w-full max-w-xl"
        onSubmit={(e) => {
          e.preventDefault();
          onSubmit();
        }}
      >
        <div className="flex items-center rounded-xl border bg-white px-3 py-2 shadow-sm">
          <input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search books"
            className="w-full outline-none text-sm"
          />

          {/* X icon */}
          {query && (
            <button
              type="button"
              onClick={onClear}
              className="ml-2 p-1 rounded-md hover:bg-gray-100"
              aria-label="Clear search"
            >
              <XMarkIcon className="h-5 w-5 text-gray-600" />
            </button>
          )}

          {/* Search icon */}
          <button
            type="submit"
            className="ml-2 p-1 rounded-md hover:bg-gray-100 disabled:opacity-40"
            aria-label="Search"
            disabled={!query.trim()}
          >
            <MagnifyingGlassIcon className="h-5 w-5 text-gray-700" />
          </button>
        </div>
      </form>
    </div>
  );
}