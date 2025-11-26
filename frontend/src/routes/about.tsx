import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/about')({
  component: AboutPage,
})

function AboutPage() {
  return (
  <>
      
     {/* About Banner */}
      <section className="h-[300px] w-full bg-contain bg-center relative"
      style={{ backgroundImage: "url('/about_us_banner.png')" }}
      >
      </section>

     {/* About Us */}
      <section className="h-[150px] bg-gray-100">
        <p className=" text-black font-semibold text-4xl mx-70 pt-8">Who we are</p>
        <p className=" text-black text-xl mx-70 pt-6">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras volutpat non velit eget aliquet. Phasellus tristique viverra enim vitae ultrices. 
              Ut fermentum nulla et nisi commodo aliquet. Aenean dictum odio convallis neque tempor feugiat. Curabitur varius magna odio.</p>
      </section>

      {/* Mission */}
      <section className="h-[400px] bg-gray-100">
        <p className=" text-black font-semibold text-4xl mx-70 pt-10">Our Mission</p>
        <p className=" text-black text-xl mx-70 pt-6"><strong>Lorem ipsum dolor sit amet.</strong> consectetur adipiscing elit. Cras volutpat non velit eget aliquet. Phasellus tristique viverra enim vitae ultrices. 
              Ut fermentum nulla et nisi commodo aliquet. Aenean dictum odio convallis neque tempor feugiat. Curabitur varius magna odio.</p>
        <p className=" text-black text-xl mx-70 pt-6"><strong>Curabitur imperdiet est velit.</strong> ultricies massa aliquam non. Suspendisse non ligula aliquet, aliquam velit ac, vehicula risus. Curabitur velit nibh, consectetur eget arcu a, semper dictum odio.</p>
        <p className=" text-black text-xl mx-70 pt-6"><strong>Aenean interdum lectus in elementum mollis.</strong> Fusce ullamcorper ultricies dui, sit amet vehicula eros facilisis nec. Maecenas id eleifend velit, vitae sagittis risus.</p>
        <p className=" text-black text-xl mx-70 pt-6"><strong>Praesent sagittis blandit neque in lobortis.</strong> Aenean interdum lectus in elementum mollis.</p>
      </section>

      {/* About Us */}
      <section className="h-[60px] bg-gray-600"></section>

      {/* How It Works */}
      <section className="h-[450px] bg-gray-100">
        <p className=" text-black font-semibold text-4xl text-center mx-70 pt-12 pb-8">How It Works</p>
        <section className="grid grid-cols-4 gap-4 mx-150">
          <div className="text-center p-2"><strong>Find Your Book</strong><br></br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras volutpat non velit eget aliquet. Phasellus tristique viverra enim vitae ultrices.</div>
          <div className="text-center p-2"><strong>Contact Seller</strong><br></br>ultricies massa aliquam non. Suspendisse non ligula aliquet, aliquam velit ac, vehicula risus.</div>
          <div className="text-center p-2"><strong>Meet up</strong><br></br>Fusce ullamcorper ultricies dui, sit amet vehicula eros facilisis nec. Maecenas id eleifend velit, vitae sagittis risus.</div>
          <div className="text-center p-2"><strong>Recieve Book</strong><br></br>Praesent sagittis blandit neque in lobortis. Aenean interdum lectus in elementum mollis.</div>
        </section>
      </section>
    </>
  )
}