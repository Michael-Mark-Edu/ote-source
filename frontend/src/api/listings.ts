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

export type ListingPhotoDto = {
  listingPhotoId: number;
  photoIndex: number;
  photoUrl: string;
  createdAt: string;
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

export async function getListingPhotos(
  listingId: number | string
): Promise<ListingPhotoDto[]> {
  const res = await fetch(`/api/listings/${listingId}/photos`, {
    credentials: "include",
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to load listing photos");
  }

  return (await res.json()) as ListingPhotoDto[];
}

export async function uploadListingPhoto(
  listingId: number | string,
  file: File
): Promise<ListingPhotoDto> {
  const res = await fetch(`/api/listings/${listingId}/photos`, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-Type": file.type || "application/octet-stream",
    },
    body: file,
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to upload listing photo");
  }

  return (await res.json()) as ListingPhotoDto;
}

export async function uploadListingPhotos(
  listingId: number | string,
  files: File[]
): Promise<ListingPhotoDto[]> {
  const uploadedPhotos: ListingPhotoDto[] = [];

  for (const file of files) {
    const uploadedPhoto = await uploadListingPhoto(listingId, file);
    uploadedPhotos.push(uploadedPhoto);
  }

  return uploadedPhotos;
}

export async function deleteListingPhoto(
  listingId: number | string,
  photoIndex: number
): Promise<void> {
  const res = await fetch(`/api/listings/${listingId}/photos/${photoIndex}`, {
    method: "DELETE",
    credentials: "include",
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to delete listing photo");
  }
}