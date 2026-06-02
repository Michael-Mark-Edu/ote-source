import { createFileRoute } from "@tanstack/react-router";
import HomeBanner from "../components/home/HomeBanner";
import HomePopularBooks from "../components/home/HomePopularBooks";
import HomeLatestBooks from "../components/home/HomeLatestBooks";
import HomeMission from "../components/home/HomeMission";

export const Route = createFileRoute("/")({
  component: HomePage,
});

function HomePage() {
  return (
    <>
      <HomeBanner />
      <HomePopularBooks />
      <HomeLatestBooks />
      <HomeMission />
    </>
  );
}