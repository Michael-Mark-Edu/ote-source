import { Link } from "@tanstack/react-router";
import type { AccountTabKey } from "./AccountPage";

const baseTabClass =
  "border-b-2 px-4 py-2 text-sm font-medium transition";

const inactiveTabClass =
  "border-transparent text-gray-600 hover:border-gray-300 hover:text-gray-900";

const activeTabClass =
  "border-blue-600 text-blue-700";

export default function AccountTabs({
  activeTab,
  onChange,
  userId,
}: {
  activeTab: AccountTabKey;
  onChange: (tab: AccountTabKey) => void;
  userId: number | null;
}) {
  return (
    <div className="flex gap-2 border-b border-gray-300">
      <button
        type="button"
        onClick={() => onChange("myListings")}
        className={`${baseTabClass} ${
          activeTab === "myListings" ? activeTabClass : inactiveTabClass
        }`}
      >
        My Listings
      </button>

      <button
        type="button"
        onClick={() => onChange("savedListings")}
        className={`${baseTabClass} ${
          activeTab === "savedListings" ? activeTabClass : inactiveTabClass
        }`}
      >
        Saved Listings
      </button>

      {userId ? (
        <Link
          to="/users/$userId"
          params={{ userId: String(userId) }}
          className={`${baseTabClass} ${inactiveTabClass}`}
        >
          Profile
        </Link>
      ) : (
        <button
          type="button"
          disabled
          className={`${baseTabClass} cursor-not-allowed border-transparent text-gray-400`}
        >
          Profile
        </button>
      )}

      <button
        type="button"
        onClick={() => onChange("account")}
        className={`${baseTabClass} ${
          activeTab === "account" ? activeTabClass : inactiveTabClass
        }`}
      >
        Account
      </button>
    </div>
  );
}