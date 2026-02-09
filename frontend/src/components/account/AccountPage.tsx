import { useState } from "react";
import AccountTabs from "./AccountTabs";
import MyListingsTab from "./tabs/MyListingsTab";
import SavedListingsTab from "./tabs/SavedListingsTab";
import AccountTab from "./tabs/AccountTab";
import type { SessionTokenGetDto } from "../../api/users";

export type AccountTabKey = "myListings" | "savedListings" | "account";

export default function AccountPage() {
  const [activeTab, setActiveTab] = useState<AccountTabKey>("account");
    const [session, setSession] = useState<SessionTokenGetDto | null>(null);

  return (
    <div className="mx-auto max-w-3xl p-6 bg-amber-50 min-h-screen">
      <h1 className="text-2xl font-semibold mb-6">Account</h1>

      <AccountTabs activeTab={activeTab} onChange={setActiveTab} />

      <div className="mt-6">
        {activeTab === "myListings" && <MyListingsTab />}
        {activeTab === "savedListings" && <SavedListingsTab />}
        {activeTab === "account" && (<AccountTab userId={session?.userId ?? null} onLoggedIn={setSession} />)}
      </div>
    </div>
  );
}