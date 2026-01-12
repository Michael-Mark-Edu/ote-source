import { Outlet, createRootRoute } from "@tanstack/react-router";
import SiteHeader from "../components/layout/SiteHeader";
import SiteFooter from "../components/layout/SiteFooter";

export const Route = createRootRoute({
  component: RootComponent,
});

function RootComponent() {
  return (
    <div className="min-h-dvh bg-gray-400">
      <SiteHeader />
      <Outlet />
      <SiteFooter />
    </div>
  );
}