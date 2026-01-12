type FilterItem = { label: string };

const defaultFilters: FilterItem[] = [
  { label: "Image" },
  { label: "Posted Today" },
  { label: "Duplicates" },
  { label: "Condition" },
  { label: "Price" },
  { label: "Class" },
  { label: "Subject" },
  { label: "Edition" },
];

export default function ExploreFiltersSidebar({ filters = defaultFilters }: { filters?: FilterItem[] }) {
  return (
    <aside className="w-64 bg-gray-800 text-white">
      <div className="min-h-0 flex-1 flex overflow-hidden">
        <nav aria-label="Sidebar" className="hidden lg:block lg:bg-gray-800 lg:overflow-y-auto">
          <div className="relative w-64 flex space-y-4 flex-col p-2">
            {filters.map((f) => (
              <label key={f.label} className="text-gray-400 hover:text-red-700 flex items-center gap-2 cursor-pointer">
                <input type="checkbox" className="h-5 w-5" />
                <span className="inline-flex text-lg font-normal">{f.label}</span>
              </label>
            ))}
          </div>
        </nav>
      </div>
    </aside>
  );
}
