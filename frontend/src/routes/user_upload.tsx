import { createFileRoute } from '@tanstack/react-router'
import { PhotoIcon } from '@heroicons/react/24/solid'
import { ChevronDownIcon } from '@heroicons/react/16/solid'

export const Route = createFileRoute('/user_upload')({
  component: RouteComponent,
})

function RouteComponent() {
  return (
    <>
    {/** Webpage section */}
        {/* Banner section */}
        <section className='h-[100px] bg-gray-200'>
            <div className="mx-auto max-w-6xl h-full  grid items-stretch gap-24 lg:grid-cols-1">
                {/* Banner Image */}
                <div className="h-auto w-auto">
                    <div className="h-full w-full rounded-2xl flex items-center justify-center bg-blue-300">
                        <span className="text-black text-sm">Banner Image Placeholder (needed or not?)</span>
                    </div>
                </div>
            </div>
        </section>
        
        {/** test section for input data (looks ugly) */}
        <main className="flex flex-wrap justify-center-safe items-center-safe bg-white">
            <form className="w-full max-w-lg flex flex-wrap justify-center-safe items-center-safe">
                {/** Location / Campus Block */}
                <div className="flex flex-wrap w-full justify-center-safe items-center-safe">
                    <div className="w-full mb-3 mx-18 my-3">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-city"> 
                            Campus
                        </label>
                        {/** drop down selection for campus */}
                        <div className="relative">
                            <select className="block appearance-none w-full bg-gray-200 border border-gray-200 text-gray-700 py-3 px-4 pr-30 mb-3 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500" 
                            id="grid-city">
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
                    {/** Book of Name */ }
                    <div className="w-full mb-3 mx-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-title">
                            Title of Book
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-title"
                            type="text"
                            placeholder="Title"
                        ></input> 
                    </div>
                    {/** Author */}
                    <div className="w-full mb-6 mx-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-author">
                            Author
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-author"
                            type="text"
                            placeholder="Author"
                        ></input> 
                    </div>
                    {/** Edition */}
                    <div className="w-full mb-3 mx-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-edition">
                            Edition
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-edition"
                            type="text"
                            placeholder="1st Edition"
                        ></input> 
                    </div>
                    {/** Publishing Year */}
                    <div className="w-full mb-3 mx-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-year">
                            Publishing Year
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-year"
                            type="integer"
                            placeholder="Year"
                        ></input> 
                    </div>
                    {/** ISBN */}
                    <div className="w-full mb-3 mx-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-isbn">
                            ISBN
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-isbn"
                            type="integer"
                            placeholder="ISBN"
                        ></input> 
                    </div>
                    {/** Condition */}
                    <div className="w-full mb-3 mx-18">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-condition">
                            Condition
                        </label>
                        <div className="relative">
                            <select className="block appearance-none w-full bg-gray-200 border border-gray-200 text-gray-700 py-3 px-4 pr-30 mb-3 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500" 
                            id="grid-city">
                                <option>Excellent</option>
                                <option>Some Damage</option>
                                <option>Major Damage</option>
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
                {/** Course Info Block*/}
                <div className="flex flex-wrap -mx-3 mg-6">
                    {/** Subject */}
                    <div className="w-full md:w-1/2 px-3 md:mb-6">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-subject">
                            Subject
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-subject"
                            type="text"
                            placeholder="Subject"
                        ></input> 
                    </div>
                    {/** Course Number */}
                    <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                        <label className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2" htmlFor="grid-course_number">
                            Course Number
                        </label>
                        <input className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-non focus:bg-white focus:border-gray-500" 
                            id="grid-course_number"
                            type="integer"
                            placeholder="Course Number"
                        ></input> 
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
                            <PhotoIcon aria-hidden="true" className="mx-auto size-12 text-gray-600" />
                            <div className="mt-4 flex text-sm/6 text-gray-400">
                                <label
                                htmlFor="file-upload"
                                className="relative cursor-pointer rounded-md bg-transparent font-semibold text-indigo-400 focus-within:outline-2 focus-within:outline-offset-2 focus-within:outline-indigo-500 hover:text-indigo-300"
                                >
                                <span>Upload a file</span>
                                <input id="file-upload" name="file-upload" type="file" className="sr-only" />
                                </label>
                                <p className="pl-1">or drag and drop</p>
                            </div>
                            <p className="text-xs/5 text-gray-400">PNG, JPG, GIF up to 10MB</p>
                            </div>
                        </div>
                    </div>
                </div>
                {/** Description Block */}
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
                            defaultValue={''}
                            />
                        </div>
                    </div>
                </div>
                {/** Negotiation Block? */}
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
                            defaultValue={''}
                            />
                        </div>
                    </div>
                </div>

                <div className="mt-6 flex items-center justify-end gap-x-20 my-10">
                    <button type="button" className="text-m/6 font-semibold text-gray-500">
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
        </main>
    </>
    )
}
