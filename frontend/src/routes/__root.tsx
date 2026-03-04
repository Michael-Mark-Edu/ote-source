import { Outlet, createRootRoute } from "@tanstack/react-router";
import SiteHeader from "../components/header/SiteHeader";
import SiteFooter from "../components/layout/SiteFooter";
import { AuthProvider } from "../components/auth/AuthProvider";

export const Route = createRootRoute({
  component: RootComponent,
});

function RootComponent() {
  return (
    <AuthProvider>
      <div className="min-h-dvh bg-gray-400">
        <SiteHeader />
        <Outlet />
        <SiteFooter />
      </div>
    </AuthProvider>
  );
}
