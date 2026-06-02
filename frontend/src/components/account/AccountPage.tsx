import { useContext, useState } from "react";
import { useNavigate, useSearch } from "@tanstack/react-router";
import AccountTabs from "./AccountTabs";
import MyListingsTab from "./tabs/MyListingsTab";
import SavedListingsTab from "./tabs/SavedListingsTab";
import AccountTab from "./tabs/AccountTab";
import type { SessionTokenGetDto } from "../../api/users";
import { AuthContext } from "../auth/AuthContext";

export type AccountTabKey = "myListings" | "savedListings" | "account";

type AccountSearch = { tab?: AccountTabKey };

export default function AccountPage() {
  const navigate = useNavigate();
  const search = useSearch({ strict: false }) as AccountSearch;
  const auth = useContext(AuthContext);
  const currentUserId = auth?.user ? Number(auth.user.id) : null;

  const activeTab: AccountTabKey =
    search.tab === "myListings" ||
    search.tab === "savedListings" ||
    search.tab === "account"
      ? search.tab
      : "account";

  const [session, setSession] = useState<SessionTokenGetDto | null>(null);

  function changeTab(tab: AccountTabKey) {
    navigate({
      to: "/account",
      search: { tab },
      replace: true,
    });
  }

  console.log("Account session:", session);

  return (
    <div className="mx-auto max-w-3xl p-6 bg-amber-50 min-h-screen">
      <h1 className="text-2xl font-semibold mb-6">Account</h1>

      <AccountTabs
        activeTab={activeTab}
        onChange={changeTab}
        userId={currentUserId}
      />

      <div className="mt-6">
        {activeTab === "myListings" && <MyListingsTab />}
        {activeTab === "savedListings" && <SavedListingsTab />}
        {activeTab === "account" && (
          <AccountTab userId={session?.userId ?? null} onLoggedIn={setSession} />
        )}
      </div>
    </div>
  );
}