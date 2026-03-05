export type BookGetDto = {
  isbn: string;
  title: string;
  authors: string;
  publishers: string;
  description?: string | null;
  publishDate?: string | null;
};

export type BookPostDto = {
  isbn: string;
  title: string;
  authors: string;
  publishers: string;
  description?: string | null;
  publishDate?: string | null;
};

export async function getBookByIsbn(isbn: string): Promise<BookGetDto | null> {
  const res = await fetch(`/api/books/${encodeURIComponent(isbn)}`, {
    credentials: "include",
  });

  if (res.status === 404) return null;

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || `Failed to load book for ISBN ${isbn}`);
  }

  return (await res.json()) as BookGetDto;
}

export async function createBook(dto: BookPostDto): Promise<void> {
  const res = await fetch("/api/books", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(dto),
  });

  if (!res.ok) {
    const msg = await res.text();
    throw new Error(msg || "Failed to create book");
  }
}