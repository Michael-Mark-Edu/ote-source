export default function ExploreSearchBar() {
  return (
    <>
    { /* Search Bar Banner */}
    <div className="flex bg-white h-12 justify-start">
        <div className="sm:w-100 m-1 justify-start">
            <input
            type="search"
            placeholder="Explore books..."
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm justify-start"
            />
        </div>
    </div>
    </>
  );
}
