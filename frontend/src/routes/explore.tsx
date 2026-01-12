import { createFileRoute } from "@tanstack/react-router";
import ExploreBanner from "../components/explore/ExploreBanner";
import ExploreLayout from "../components/explore/ExploreLayout";

export const Route = createFileRoute("/explore")({
  component: ExplorePage,
});

function ExplorePage() {
  return (
    <>
      <ExploreBanner />

      <section className="bg-gray-200">
        <ExploreLayout />
      </section>
    </>
  );
}
