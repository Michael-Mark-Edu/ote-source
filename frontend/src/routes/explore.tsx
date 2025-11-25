import { createFileRoute } from '@tanstack/react-router'
import Navbar from "../components/Navbar"

export const Route = createFileRoute('/explore')({
  component: ExplorePage,
})

function ExplorePage() {
    return (
        <>
            {/* Explore Banner */}
            <section className='h-[100px] bg-gray-200'>
                <div className="mx-auto max-w-6xl h-full  grid items-stretch gap-24 lg:grid-cols-1">
                    {/* Explore Banner Image */}
                    <div className="h-full w-full">
                        <div className="h-full w-full rounded-2xl flex items-center justify-center bg-blue-300">
                            <span className="text-black text-sm">Explore Banner Image Placeholder</span>
                        </div>
                    </div>
                </div>
            </section>

            { /** Trying a new sidebar */}
            <section className="bg-gray-200">
                <div>
                    <Navbar />
                </div>
            </section>

            {/** this part will include anything at the bottom after the explore page */}
        </>
    )
}