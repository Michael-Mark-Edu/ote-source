import { createFileRoute } from "@tanstack/react-router";
import ExploreLayout from "../components/explore/ExploreLayout";

export const Route = createFileRoute("/explore")({
  validateSearch: (search: Record<string, unknown>) => {
    return {
      q: typeof search.q === "string" ? search.q : "",
    };
  },
  component: ExplorePage,
});

function ExplorePage() {
  return (
    <>
      <section className="bg-gray-200">
        <ExploreLayout />
      </section>
    </>
  );
}
