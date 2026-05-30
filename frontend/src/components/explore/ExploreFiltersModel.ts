export type ListingCondition = "new" | "likeNew" | "good" | "fair" | "poor";

export type PurchaseTypeFilter = "All" | "Sell" | "Trade" | "Rent" | "Free";

export type ExploreFilters = {
  hasImage: boolean;
  postedToday: boolean;
  duplicates: boolean;
  condition: Record<ListingCondition, boolean>;
  priceMin: string;
  priceMax: string;
  class: string;
  subject: string;
  edition: string;
  isbn: string;
  author: string;
  publisher: string;
  purchaseType: PurchaseTypeFilter;
};

export const DEFAULT_FILTERS: ExploreFilters = {
  hasImage: false,
  postedToday: false,
  duplicates: false,
  condition: { new: false, likeNew: false, good: false, fair: false, poor: false },
  priceMin: "",
  priceMax: "",
  class: "",
  subject: "",
  edition: "",
  isbn: "",
  author: "",
  publisher: "",
  purchaseType: "All",
};

export const CONDITION_LABELS: Record<ListingCondition, string> = {
  new: "New",
  likeNew: "Like New",
  good: "Good",
  fair: "Fair",
  poor: "Poor",
};