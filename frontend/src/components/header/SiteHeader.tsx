import { Link, useNavigate, useSearch } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import AuthModal from "../auth/AuthModal";
import { useAuth } from "../auth/useAuth";
import {UserIcon } from "@heroicons/react/24/outline";
import { UserIcon as UserIconSolid } from "@heroicons/react/24/solid";
import type { AccountTabKey } from "../account/AccountPage";

type ExploreSearch = { q?: string };

import HeaderNav from "./HeaderNav";
import HeaderSearch from "./HeaderSearch";
import AccountPanel from "./AccountPanel";

export default function SiteHeader() {
  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isPanelOpen, setIsPanelOpen] = useState(false);

  const auth = useAuth();
  const navigate = useNavigate();
  const search = useSearch({ strict: false }) as ExploreSearch;

  const [query, setQuery] = useState(search.q ?? "");

  useEffect(() => {
    setQuery(search.q ?? "");
  }, [search.q]);

  function submitSearch() {
    const q = query.trim();
    if (!q) return;

    navigate({
      to: "/explore",
      search: { q },
    });
  }

  function clearSearch() {
  setQuery("");
  }

  function goToAccountTab(tab: AccountTabKey) {
    navigate({ to: "/account", search: { tab } });
    setIsPanelOpen(false);
  }

  function signOut() {
    auth.logout();
    setIsPanelOpen(false);
  }

  return (
    <>
      <header className="sticky top-0 z-50 bg-white/90 border-b backdrop-blur">
        <div className="mx-auto max-w-8xl h-16 px-4 flex items-center gap-10">
          {/* Title/Logo */}
          <Link to="/" className="font-semibold whitespace-nowrap">OpenTextbookExchange</Link>

          <HeaderNav />

          <HeaderSearch
            query={query}
            setQuery={setQuery}
            onSubmit={submitSearch}
            onClear={clearSearch}
          />

          {/* Login Auth */}
          {auth.isAuthed ? (
            <button
              type="button"
              onClick={() => setIsPanelOpen(true)}
              className="p-2 rounded-full hover:bg-gray-100 transition"
              aria-label="Account"
            >
              <UserIconSolid className="h-6 w-6 text-gray-700" />
            </button>
          ) : (
            // User Icon
            <button
              type="button"
              onClick={() => setIsLoginOpen(true)}
              className="p-2 rounded-full hover:bg-gray-100 transition"
              aria-label="Login"
            >
              <UserIcon className="h-6 w-6 text-gray-700" />
            </button>
          )}
        </div>
      </header>

      <AccountPanel
        isOpen={isPanelOpen}
        onClose={() => setIsPanelOpen(false)}
        username={auth.user?.username ?? "User"}
        onGoToAccountTab={goToAccountTab}
        onSignOut={signOut}
      />

      <AuthModal isOpen={isLoginOpen} onClose={() => setIsLoginOpen(false)} />
    </>
  );
}