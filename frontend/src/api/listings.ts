export type BookListingPostDto = {
  condition: string;
  purchaseType: string;
  price?: string | null;
  userId: number;
  isbn: string;
};

export type BookListingGetDto = {
  bookListingId: number;
  condition: string;
  purchaseType: string;
  price?: string | null;
  userId: number;
  isbn: string;
};

export async function createListing(dto: BookListingPostDto): Promise<BookListingGetDto> {
  const res = await fetch("/api/listings", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(dto),
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to create listing");
  }

  return (await res.json()) as BookListingGetDto;
}

export async function getListings(): Promise<BookListingGetDto[]> {
  const res = await fetch("/api/listings", { credentials: "include" });
  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to load listings");
  }
  return (await res.json()) as BookListingGetDto[];
}

export async function getListingById(listingId: number | string): Promise<BookListingGetDto> {
  const res = await fetch(`/api/listings/${listingId}`, { credentials: "include" });
  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to load listing");
  }
  return (await res.json()) as BookListingGetDto;
}