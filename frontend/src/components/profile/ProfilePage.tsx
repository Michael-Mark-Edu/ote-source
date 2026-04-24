// hypothetical public profile data model from the backend
type PublicProfile = {
  id: number
  displayName: string
  profileImageUrl?: string
  bio: string
  location: string
  memberSince: string
  listingsCount: number
  averageRating: number
  reviewCount: number
}

// user review model of what a review looks like on a users public profile.
type UserReview = {
  id: number
  reviewerDisplayName: string
  rating: number
  comment: string
  createdAt: string
}

// temp mock profile data
const mockProfile: PublicProfile = {
  id: 6,
  displayName: 'Bean Coffie',
  profileImageUrl: '',
  bio: 'Software engineering student selling textbooks from previous terms.',
  location: 'Oregon Tech',
  memberSince: '2025-09-01',
  listingsCount: 4,
  averageRating: 4.6,
  reviewCount: 2,
}

// temp mock review data
const mockReviews: UserReview[] = [
  {
    id: 1,
    reviewerDisplayName: 'Saul Goodman',
    rating: 5,
    comment: 'Great seller. The book was exactly as described.',
    createdAt: '2026-04-18T10:30:00Z',
  },
  {
    id: 2,
    reviewerDisplayName: 'Walter White',
    rating: 4,
    comment: 'Easy to communicate with and quick meetup.',
    createdAt: '2026-04-12T14:15:00Z',
  }
]

// converts backend dateStrings into readable date format
function formatDate(dateString: string) {
  return new Date(dateString).toLocaleDateString()
}

// star rating properties
type StarRatingProps = {
  rating: number
  maxRating?: number
  size?: 'sm' | 'md' | 'lg'
}

//  star rating display
function StarRating({ rating, maxRating = 5, size = 'md' }: StarRatingProps) {
  const roundedRating = Math.round(rating)

  const sizeClass = {
    sm: 'text-sm',
    md: 'text-xl',
    lg: 'text-2xl',
  }[size]

  return (
    <div
      className={`flex items-center gap-1 text-yellow-500 ${sizeClass}`}
      aria-label={`${rating} out of ${maxRating} stars`}
    >
      {Array.from({ length: maxRating }, (_, index) => {
        const starNumber = index + 1
        const isFilled = starNumber <= roundedRating

        return (
          <span key={starNumber} aria-hidden="true">
            {isFilled ? '★' : '☆'} 
          </span>
        )
      })}
    </div>
  )
}

export default function ProfilePage() {
  return (
    <main className="mx-auto max-w-3xl px-12 py-8">
      <section className="mb-6 flex flex-col gap-6 rounded-2xl border border-gray-200 bg-white p-6 shadow-sm md:flex-row md:items-center">
        {/* Main public profile header. */}
        <div>
          <h1 className="mb-2 text-3xl font-bold text-gray-900">
            {mockProfile.displayName}
          </h1>

          <p className="mb-4 max-w-2xl text-gray-600">{mockProfile.bio}</p>

          <div className="flex flex-wrap gap-3 text-sm text-gray-500">
            <span className="rounded-full bg-gray-100 px-3 py-1">
              {mockProfile.location}
            </span>
            <span className="rounded-full bg-gray-100 px-3 py-1">
              Member since {formatDate(mockProfile.memberSince)}
            </span>
            <span className="rounded-full bg-gray-100 px-3 py-1">
              {mockProfile.listingsCount} active listings
            </span>
          </div>
        </div>  
      </section>

      {/* Rating summary section. */}
      <section className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm">
        <h2 className="mb-4 text-xl font-semibold text-gray-900">
          Seller Rating
        </h2>

        <div className="flex items-center gap-4">
          <div className="text-5xl font-bold text-gray-900">
            {mockProfile.averageRating.toFixed(1)}
          </div>

          <div>
            <StarRating rating={mockProfile.averageRating} size="lg" />
            <p className="text-sm text-gray-500">
              {mockProfile.reviewCount} reviews
            </p>
          </div>
        </div>

        <h2 className="mb-4 text-xl font-semibold text-gray-900">Reviews</h2>

        <div className="space-y-4">
          {mockReviews.map((review) => (
            <article
              key={review.id}
              className="rounded-xl border border-gray-100 bg-gray-50 p-4"
            >
              <div className="mb-3 flex flex-col justify-between gap-2 sm:flex-row sm:items-start">
                <div>
                  <h3 className="font-semibold text-gray-900">
                    {review.reviewerDisplayName}
                  </h3>
                  <p className="text-sm text-gray-500">
                    {formatDate(review.createdAt)}
                  </p>
                </div>

                <StarRating rating={review.rating} size="md" />
              </div>

              <p className="text-gray-700">{review.comment}</p>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}