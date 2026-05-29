import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { PhotoIcon } from '@heroicons/react/24/solid'
import { ChevronDownIcon } from '@heroicons/react/16/solid'
import { useMemo, useState, useContext } from "react";
import { AuthContext } from '../components/auth/AuthContext';
import { getBookByIsbn, createBook, type BookPostDto } from "../api/books";
import { createListing, uploadListingImages, type BookListingPostDto } from "../api/listings";


export const Route = createFileRoute("/user_upload")({
  component: UploadPage,
});

type Condition = "New" | "Like New" | "Good" | "Fair" | "Poor";

type UploadForm = {
  campus: "Klamath Falls" | "Portland/Metro";
  title: string;
  author: string;
  edition: string;
  publishingYear: number | "";
  isbn: string;
  condition: Condition;
  subject: string;
  courseNumber: number | "";
  description: string;
  price: number | "";
  trade: string;
};

function UploadPage() {
  const navigate = useNavigate();

  const [form, setForm] = useState<UploadForm>({
    campus: "Klamath Falls",
    title: "",
    author: "",
    edition: "",
    publishingYear: "",
    isbn: "",
    condition: "New",
    subject: "",
    courseNumber: "",
    description: "",
    price: "",
    trade: "",
  });

  const [files, setFiles] = useState<File[]>([]);

  const previewUrl = useMemo(() => {
    if (files.length === 0) return null;
    return URL.createObjectURL(files[0]);
  }, [files]);

  function setField<K extends keyof UploadForm>(key: K, value: UploadForm[K]) {
    setForm((prev) => ({ ...prev, [key]: value }));
  }

  const auth = useContext(AuthContext);

  async function onSubmit(e: React.FormEvent) {
  e.preventDefault();

  if (!auth?.user) {
    alert("You must be logged in to create a listing.");
    return;
  }

  const userId = Number(auth.user.id);
  if (!Number.isFinite(userId) || userId <= 0) {
    alert("Invalid user id. Please sign out and sign in again.");
    return;
  }

  if (!form.isbn.trim()) {
    alert("ISBN is required.");
    return;
  }

  try {
    // Verify book exists by ISBN. If not, create it.
    const existingBook = await getBookByIsbn(form.isbn.trim());

    if (!existingBook) {
      if (!form.title.trim() || !form.author.trim()) {
        alert("Title and Author are required to create a new book record.");
        return;
      }

      const bookDto: BookPostDto = {
        isbn: form.isbn.trim(),
        title: form.title.trim(),
        authors: form.author.trim(),
        publishers: "Unknown",  // TODO: add a publishers field to form later
        description: form.description.trim() ? form.description.trim() : null,
        publishDate:
          form.publishingYear === ""
            ? null
            : `${form.publishingYear}-01-01T00:00:00Z`,
      };

      await createBook(bookDto);
    }

    // Create the listing
    const dto: BookListingPostDto = {
    condition: form.condition,
    purchaseType: form.trade.trim().length ? "Trade" : "Sell",
    price: form.price === "" ? null : String(form.price),
    userId,
    isbn: form.isbn.trim(),
    };

    const createdListing = await createListing(dto);

    // Upload images after the listing has been created
    // The backend handles sending these images to S3
    await uploadListingImages(createdListing.bookListingId, files);

    // Navigate to the new listing page
    navigate({
    to: "/listings/$listingId",
    params: { listingId: String(createdListing.bookListingId) },
    });
    } catch (err) {
        console.error(err);
        alert(err instanceof Error ? err.message : "Failed to create listing");
    }
}

  return (
        <main className="flex flex-wrap justify-center-safe items-center-safe bg-white">
            <div className="mx-auto max-w-2xl px-4 py-8">
                <h1 className="text-2xl font-semibold text-gray-900">Upload Listing</h1>
                <p className="mt-1 text-sm text-gray-500">
                Create a listing for your textbook.
                </p>

                <form onSubmit={onSubmit} className="w-full max-w-lg flex flex-wrap justify-center-safe items-center-safe">
                    {/** Location / Campus */}
                    <div className="flex flex-wrap w-full justify-center-safe items-center-safe">
                        <div className="w-full mb-3 mx-18 my-3">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-city"> 
                                Campus
                            </label>
                            {/** drop down selection for campus */}
                            <div className="relative">
                                <select
                                    id="campus"
                                    value={form.campus}
                                    onChange={(e) => setField("campus", e.target.value as UploadForm["campus"])}
                                    className="block appearance-none w-full bg-gray-200 border border-gray-200 text-gray-700 py-3 px-4 pr-30 mb-3 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500" 
                                >
                                    <option>Klamath Falls</option>
                                    <option>Portland/Metro</option>
                                </select>
                                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2  text-gray-700">
                                    <ChevronDownIcon
                                    aria-hidden="true"
                                    className="pointer-events-none col-start-1 row-start-1 mr-2 size-5 self-center justify-self-end text-gray-400 sm:size-4"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    {/** Book Block */}
                    <div className="flex flex-wrap w-full justify-center-safe items-center-safe">
                        {/* Title */ }
                        <div className="w-full mb-3 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-title">
                                Title of Book
                            </label>
                            <input 
                            id="title"
                            value={form.title}
                            onChange={(e) => setField("title", e.target.value)}
                            type="text"
                            placeholder="Title"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            />
                        </div>
                        
                        {/* Author */}
                        <div className="w-full mb-3 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-author">
                                Author
                            </label>
                            <input 
                            id="author"
                            value={form.author}
                            onChange={(e) => setField("author", e.target.value)}
                            type="text"
                            placeholder="Author"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500"
                            />
                        </div>

                        {/* Edition */}
                        <div className="w-full mb-3 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-edition">
                                Edition
                            </label>
                            <input 
                            id="edition"
                            value={form.edition}
                            onChange={(e) => setField("edition", e.target.value)}
                            type="text"
                            placeholder="3rd Edition"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            />
                        </div>

                        {/* Publishing Year */}
                        <div className="w-full mb-3 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-year">
                                Publishing Year
                            </label>
                            <input 
                            id="publishingYear"
                            value={form.publishingYear}
                            onChange={(e) => {
                                const v = e.target.value;
                                setField("publishingYear", v === "" ? "" : Number(v));
                            }}
                            type="number"
                            inputMode="numeric"
                            placeholder="Year"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            />
                        </div>

                        {/* ISBN */}
                        <div className="w-full mb-3 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-isbn">
                                ISBN
                            </label>
                            <input
                            id="isbn"
                            value={form.isbn}
                            onChange={(e) => setField("isbn", e.target.value)}
                            type="text"
                            inputMode="numeric"
                            placeholder="ISBN"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500"
                            /> 
                        </div>

                        {/* Condition */}
                        <div className="w-full mb-3 mx-18">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-condition">
                                Condition
                            </label>
                            <div className="relative">
                                <select 
                                id="condition"
                                value={form.condition}
                                onChange={(e) => setField("condition", e.target.value as Condition)}
                                className="block appearance-none w-full bg-gray-200 border border-gray-200 text-gray-700 py-3 px-4 pr-30 mb-3 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500" 
                                >
                                    <option>New</option>
                                    <option>Like New</option>
                                    <option>Good</option>
                                    <option>Fair</option>
                                    <option>Poor</option>
                                </select>
                                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2  text-gray-700">
                                    <ChevronDownIcon
                                    aria-hidden="true"
                                    className="pointer-events-none col-start-1 row-start-1 mr-2 size-5 self-center justify-self-end text-gray-400 sm:size-4"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Course Info Block*/}
                    <div className="flex flex-wrap -mx-3 mg-6">
                        {/* Subject */}
                        <div className="w-full md:w-1/2 px-3 md:mb-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-subject">
                                Subject
                            </label>
                            <input 
                            id="subject"
                            value={form.subject}
                            onChange={(e) => setField("subject", e.target.value)}
                            type="text"
                            placeholder="Subject"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            /> 
                        </div>

                        {/* Course Number */}
                        <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-course_number">
                                Course Number
                            </label>
                            <input 
                            id="courseNumber"
                            value={form.courseNumber}
                            onChange={(e) => {
                                const v = e.target.value;
                                setField("courseNumber", v === "" ? "" : Number(v));
                            }}
                            type="number"
                            inputMode="numeric"
                            placeholder="211"
                            className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500"
                            /> 
                        </div>
                    </div>

                    {/** Photos Block */}
                    <div className="flex flex-wrap -mx-3 mg-6 ">
                        <div className="col-span-full">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="book-photos">
                                Book photos
                            </label>

                            <div className="mt-2 flex justify-center rounded-lg border border-dashed border-blue-300 px-34 py-10">
                                <div className="text-center">
                                    {previewUrl ? (
                                        <img
                                            src={previewUrl}
                                            alt="Preview"
                                            className="mx-auto h-40 w-40 object-contain rounded-md"
                                        />
                                    ) : (
                                        <PhotoIcon aria-hidden="true" className="mx-auto size-12 text-gray-600" />
                                    )}

                                    <div className="mt-4 flex text-sm/6 text-gray-400">
                                        <label
                                        htmlFor="file-upload"
                                        className="relative cursor-pointer rounded-md bg-transparent font-semibold text-indigo-400 focus-within:outline-2 focus-within:outline-offset-2 focus-within:outline-indigo-500 hover:text-indigo-300"
                                        >
                                            <span>Upload a file</span>
                                            <input
                                            id="file-upload"
                                            name="file-upload"
                                            type="file"
                                            accept="image/*"
                                            multiple
                                            className="sr-only"
                                            onChange={(e) => {
                                                const selectedFiles = Array.from(e.target.files ?? []);

                                                const validFiles = selectedFiles.filter((file) => {
                                                    const isImage = file.type.startsWith("image/");
                                                    const isSmallEnough = file.size <= 10 * 1024 * 1024;

                                                    return isImage && isSmallEnough;
                                                });

                                                if (validFiles.length !== selectedFiles.length) {
                                                    alert("Only image files under 10MB are allowed.");
                                                }

                                                setFiles(validFiles);
                                                }}
                                            />
                                        </label>
                                        <span>or drag and drop</span>
                                    </div>
                                    <p className="text-xs/5 text-gray-400">PNG, JPG, GIF up to 10MB</p>
                                    {files.length > 0 && (
                                        <ul className="mt-2 text-xs text-gray-500">
                                            {files.map((file) => (
                                            <li key={file.name}>{file.name}</li>
                                            ))}
                                        </ul>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Description Block */}
                    <div className="flex flex-wrap w-full justify-center-safe items-center-safe">
                        <div className="w-full mb-3 mx-6 my-3">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="description">
                                Description of Book
                            </label>
                            <div className="mt-2">
                                <textarea
                                id="description"
                                rows={3}
                                placeholder="Write a description about the book..."
                                className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500"
                                value={form.description}
                                onChange={(e) => setField("description", e.target.value)}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Negotiation Block? */}
                    <div className="flex flex-wrap w-full justify-center-safe items-center-safe">
                        <div className="w-full mb-6 mx-6">
                            <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="trade">
                                Trade Negotionations
                            </label>
                            <div className="mt-2">
                                <textarea
                                id="trade"
                                rows={3}
                                placeholder="Here you can write about what you would want to trade it for or if you are just looking to get it off your hands..."
                                className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500"
                                value={form.trade}
                                onChange={(e) => setField("trade", e.target.value)}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Price Block */}
                    <div>
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="price">
                            Price ($)
                        </label>
                        <input
                            id="price"
                            type="number"
                            inputMode="decimal"
                            min={0}
                            step="0.01"
                            value={form.price}
                            onChange={(e) => {
                            const v = e.target.value;
                            setField("price", v === "" ? "" : Number(v));
                            }}
                            placeholder="25.00"
                            className="block w-full rounded bg-gray-200 text-gray-700 border border-gray-200 py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                        />
                    </div>

                    {/* Actions */}
                    <div className="mt-7 flex items-center justify-end gap-x-20 my-10">
                        <button 
                            type="button" 
                            className="text-m/6 font-semibold text-gray-500"
                            onClick={() => navigate({ to: "/explore" })}
                        >
                            Cancel
                        </button>

                        <button
                            type="submit"
                            className="rounded-md bg-blue-500 px-15 py-2 text-m font-semibold text-white focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500"
                        >
                            Publish
                        </button>
                    </div>
                </form>
            </div>   
        </main>
    )
}