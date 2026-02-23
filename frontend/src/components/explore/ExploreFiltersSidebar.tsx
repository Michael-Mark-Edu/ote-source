import { useState } from "react";
import { CONDITION_LABELS, DEFAULT_FILTERS, type ExploreFilters, type ListingCondition } from "./ExploreFiltersModel";

type Props = {
  onChange?: (filters: ExploreFilters) => void;
};

export default function ExploreFiltersSidebar({ onChange }: Props) {
  const [filters, setFilters] = useState<ExploreFilters>(DEFAULT_FILTERS);

  function update(next: ExploreFilters) {
    setFilters(next);
    onChange?.(next);
  }

  function setFlag<K extends "hasImage" | "postedToday" | "duplicates">(key: K, value: boolean) {
    update({ ...filters, [key]: value });
  }

  function toggleCondition(key: ListingCondition) {
    update({
      ...filters,
      condition: { ...filters.condition, [key]: !filters.condition[key] },
    });
  }

  function clearFilters() {
    update(DEFAULT_FILTERS);
  }

  return (
   <aside className="w-full md:w-72 lg:w-80 shrink-0">
      <div className="sticky top-4 border border-blues-200 bg-blue-50 p-4 shadow-sm">
        <div className="flex items-start justify-between gap-3">
          <div>
            <h2 className="text-lg font-semibold text-slate-900">Filters</h2>
            <p className="text-sm text-slate-600">Narrow down listings</p>
          </div>

          <button
            type="button"
            onClick={clearFilters}
            className="text-sm font-medium text-slate-700 hover:text-slate-900 underline underline-offset-4"
          >
            Clear filters
          </button>
        </div>

        <div className="mt-4 space-y-6">
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Quick</h3>
            <div className="mt-3 space-y-3">
              <label className="flex items-center justify-between gap-3">
                <span className="text-sm text-slate-800">Has Image</span>
                <input
                  type="checkbox"
                  checked={filters.hasImage}
                  onChange={(e) => setFlag("hasImage", e.target.checked)}
                  className="h-4 w-4 accent-slate-900"
                />
              </label>

              <label className="flex items-center justify-between gap-3">
                <span className="text-sm text-slate-800">Posted Today</span>
                <input
                  type="checkbox"
                  checked={filters.postedToday}
                  onChange={(e) => setFlag("postedToday", e.target.checked)}
                  className="h-4 w-4 accent-slate-900"
                />
              </label>

              <label className="flex items-center justify-between gap-3">
                <span className="text-sm text-slate-800">Duplicates</span>
                <input
                  type="checkbox"
                  checked={filters.duplicates}
                  onChange={(e) => setFlag("duplicates", e.target.checked)}
                  className="h-4 w-4 accent-slate-900"
                />
              </label>
            </div>
          </section>

          {/* Checkboxes */}
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Condition</h3>
            <div className="mt-3 space-y-2">
              {(Object.keys(CONDITION_LABELS) as ListingCondition[]).map((key) => (
                <label key={key} className="flex items-center gap-3">
                  <input
                    type="checkbox"
                    checked={filters.condition[key]}
                    onChange={() => toggleCondition(key)}
                    className="h-4 w-4 accent-slate-900"
                  />
                  <span className="text-sm text-slate-800">{CONDITION_LABELS[key]}</span>
                </label>
              ))}
            </div>
          </section>

          {/* Price */}
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Price</h3>
            <div className="mt-3 grid grid-cols-2 gap-3">
              <div>
                <label className="block text-xs font-medium text-slate-600">Min</label>
                <input
                  inputMode="numeric"
                  value={filters.priceMin}
                  onChange={(e) => update({ ...filters, priceMin: e.target.value })}
                  placeholder="0"
                  className="mt-1 w-full rounded-lg border border-blue-200 bg-white px-3 py-2 text-sm outline-none focus:ring-2 focus:rinr-blue-200"
                />
              </div>
              <div>
                <label className="block text-xs font-medium text-slate-600">Max</label>
                <input
                  inputMode="numeric"
                  value={filters.priceMax}
                  onChange={(e) => update({ ...filters, priceMax: e.target.value })}
                  placeholder="200"
                  className="mt-1 w-full rounded-lg border border-blue-200 bg-white px-3 py-2 text-sm outline-none focus:ring-2 focus:rinr-blue-200"
                />
              </div>
            </div>

            {/* TODO: functional filtering slider */}
            <input type="range" min={0} max={500} className="mt-3 w-full" />

          </section>

          {/* Class */}
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Class</h3>
            <input
              value={filters.class}
              onChange={(e) => update({ ...filters, class: e.target.value })}
              placeholder="e.g., CST 211"
              className="mt-2 w-full rounded-lg border border-blue-200 bg-white px-3 py-2 text-sm outline-none focus:ring-2 focus:rinr-blue-200"
            />
            {/* TODO: Implement dropdown/autocomplete */}
            <p className="mt-1 text-xs text-slate-500">Placeholder for dropdown/autocomplete later.</p> 
          </section>

          {/* Subject */}
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Subject</h3>
            <input
              value={filters.subject}
              onChange={(e) => update({ ...filters, subject: e.target.value })}
              placeholder="e.g., Data Structures"
              className="mt-2 w-full rounded-lg border border-blue-200 bg-white px-3 py-2 text-sm outline-none focus:ring-2 focus:rinr-blue-200"
            />
            {/* TODO: Implement dropdown/autocomplete */}
            <p className="mt-1 text-xs text-slate-500">Placeholder for dropdown/autocomplete later.</p>
          </section>

          {/* Edition */}
          <section>
            <h3 className="text-sm font-semibold text-slate-900">Edition</h3>
            <input
              value={filters.edition}
              onChange={(e) => update({ ...filters, edition: e.target.value })}
              placeholder="e.g., 3rd"
              className="mt-2 w-full rounded-lg border border-blue-200 bg-white px-3 py-2 text-sm outline-none focus:ring-2 focus:rinr-blue-200"
            />
          </section>
        </div>
      </div>
    </aside>
  );
}