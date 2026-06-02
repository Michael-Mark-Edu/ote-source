const SAVED_LISTINGS_KEY = "ote_saved_listing_ids";

export function getSavedListingIds(): string[] {
  const saved = localStorage.getItem(SAVED_LISTINGS_KEY);

  if (!saved) {
    return [];
  }

  try {
    return JSON.parse(saved) as string[];
  } catch {
    return [];
  }
}

export function isListingSaved(listingId: string) {
  return getSavedListingIds().includes(listingId);
}

export function saveListing(listingId: string) {
  const savedIds = getSavedListingIds();

  if (savedIds.includes(listingId)) {
    return savedIds;
  }

  const updatedIds = [...savedIds, listingId];
  localStorage.setItem(SAVED_LISTINGS_KEY, JSON.stringify(updatedIds));

  return updatedIds;
}

export function unsaveListing(listingId: string) {
  const updatedIds = getSavedListingIds().filter((id) => id !== listingId);
  localStorage.setItem(SAVED_LISTINGS_KEY, JSON.stringify(updatedIds));

  return updatedIds;
}

export function toggleSavedListing(listingId: string) {
  if (isListingSaved(listingId)) {
    return unsaveListing(listingId);
  }

  return saveListing(listingId);
}