import type { AccountTabKey } from "./AccountPage";
import TabButton from "./TabButton";

export default function AccountTabs({
  activeTab,
  onChange,
}: {
  activeTab: AccountTabKey;
  onChange: (tab: AccountTabKey) => void;
}) {
  return (
    <div className="flex border-b border-gray-300 gap-2">
      <TabButton active={activeTab === "myListings"} onClick={() => onChange("myListings")}>
        My Listings
      </TabButton>

      <TabButton active={activeTab === "savedListings"} onClick={() => onChange("savedListings")}>
        Saved Listings
      </TabButton>

      <TabButton active={activeTab === "account"} onClick={() => onChange("account")}>
        Account
      </TabButton>
    </div>
  );
}