{/** A navbar/sidebar component to be used on any page that a navbar will be used on */}
{/** currently used exclusively for the explore page, and the text hard coded for now for the presentation */}

export default function Navbar() {
   return(
      <div className="flex h-screen">
         { /** This is the navbar fitted to the right side of the screen */}
         <aside className="w-64 bg-gray-800 text-white">

            { /** This is the customization of the navbar, colors and how wide it is, etc. */}
            <div className="min-h-0 flex-1 flex overflow-hidden">
               <nav aria-label="Sidebar" className="narrow-sidebar hidden lg:block lg:bg-gray-800 lg:overflow-y-auto">
                  
                  { /** These are the contents of each item that within the navbar */}
                     <div className="relative w-64 flex space-y-4 flex-col p-2">
                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <span className="p-1 inline-flex text-center text-lg font-normal">Image</span>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text text-lg font-normal ">Posted Today</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Duplicates</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Condition</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Price</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Class</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Subject</div>
                        </a>

                        <a href="#" className="text-gray-400 hover:text-red-700">
                           <input type="checkbox" className="h-5 w-5"/>
                           <div className="p-1 inline-flex text-center text-lg font-normal ">Edition</div>
                        </a>
                     </div>
               </nav>
            </div>

         </aside>

         { /** The main part of the page next to the sidebar */}
         <main className="flex-1 overflow-y-auto">

            { /* Search Bar Banner */}
            <div className="bg-gray-300 h-10">
               <div className="mx-auto max-w-6x1 w-full px-4 grid items-center gap-8 lg:grid-cols-4">
                     <span className="text-sm text-black">Search Bar Placeholder</span>
               </div>
            </div>

            <h1 className="text-2xl text-white">
               <section className="h-[400px] flex items-center justify-center bg-gray-200">
                  <div className="mx-auto max-w-6xl w-screen px-4 grid items-center gap-8 lg:grid-cols-5">

                     {/* Book Banner Images */}
                     {[1, 2, 3, 4, 5].map((num) => (
                        <div key={num} className="rounded-2xl border p-2 shadow-sm bg-white">
                           <div className="aspect-9/16 rounded-xl grid place-items-center bg-blue-300 ">
                              <span className=" text-sm text-black">Book {num}</span>
                           </div>
                        </div>
                     ))}
                  </div>
               </section>

                  {/*  Books */}
               <section className="h-[400px] flex items-center justify-center bg-gray-300">
                  <div className="mx-auto max-w-6xl w-screen px-4 grid items-center gap-8 lg:grid-cols-5">

                     {/*  Book Banner Images */}
                     {[1, 2, 3, 4, 5].map((num) => (
                        <div key={num} className="rounded-2xl border bg-white p-2 shadow-sm">
                           <div className="aspect-9/16 rounded-xl bg-blue-300 grid place-items-center">
                              <span className="text-black text-sm">Book {num}</span>
                           </div>
                        </div>
                     ))}
                  </div>
               </section>

                   {/*  Books */}  
               <section className="h-[400px] flex items-center justify-center bg-gray-200">
                  <div className="mx-auto max-w-6xl w-screen px-4 grid items-center gap-8 lg:grid-cols-5">

                     {/*  Book Banner Images */}
                     {[1, 2, 3, 4, 5].map((num) => (
                        <div key={num} className="rounded-2xl border p-2 shadow-sm bg-white">
                           <div className="aspect-9/16 rounded-xl grid place-items-center bg-blue-300 ">
                              <span className=" text-sm text-black">Book {num}</span>
                           </div>
                        </div>
                     ))}
                  </div>
               </section>

               {/*  Books */}
               <section className="h-[400px] flex items-center justify-center bg-gray-300">
                  <div className="mx-auto max-w-6xl w-screen px-4 grid items-center gap-8 lg:grid-cols-5">

                     {/*  Book Banner Images */}
                     {[1, 2, 3, 4, 5].map((num) => (
                        <div key={num} className="rounded-2xl border bg-white p-2 shadow-sm">
                           <div className="aspect-9/16 rounded-xl bg-blue-300 grid place-items-center">
                              <span className="text-black text-sm">Book {num}</span>
                           </div>
                        </div>
                     ))}
                  </div>
               </section>

            </h1>
            </main>
         
      </div>
   )
}
