import { createFileRoute } from "@tanstack/react-router";
import ListingPage from "../../components/listings/ListingPage";

export const Route = createFileRoute("/listings/$listingId")({
  component: ListingPage,
});