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

export type ListingImageGetDto = {
  id: number;
  listingId: number;
  imageUrl: string;
  s3Key?: string;
  createdAt?: string;
};

export async function uploadListingImages(
  listingId: number | string,
  files: File[]
): Promise<ListingImageGetDto[]> {
  if (files.length === 0) {
    return [];
  }

  const formData = new FormData();

  files.forEach((file) => {
    formData.append("images", file);
  });

  const res = await fetch(`/api/listings/${listingId}/images`, {
    method: "POST",
    credentials: "include",
    body: formData,
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to upload listing images");
  }

  return (await res.json()) as ListingImageGetDto[];
}