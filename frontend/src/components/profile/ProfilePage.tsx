import { useState } from 'react'
import { FlagIcon } from '@heroicons/react/24/outline'

// hypothetical public profile data model from the backend
type PublicProfile = {
  id: number
  displayName: string
  bio: string
  location: string
  memberSince: string
  listingsCount: number
  averageRating: number
  reviewCount: number
}

// user review model of what a review looks like on a public profile.
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
  },
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

// star rating display
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
  // review local storage until backend endpoint is ready
  const [reviews, setReviews] = useState<UserReview[]>(mockReviews)

  // stores the current review form values.
  const [selectedRating, setSelectedRating] = useState(5)
  const [comment, setComment] = useState('')

  const [showReportForm, setShowReportForm] = useState(false)
  const [reportReason, setReportReason] = useState('')
  const [reportDetails, setReportDetails] = useState('')
  const [reportSubmitted, setReportSubmitted] = useState(false)

  // calculate avg rating from the current review list
  const averageRating =
    reviews.length > 0
      ? reviews.reduce((total, review) => total + review.rating, 0) /
        reviews.length
      : 0

  // mock review submission
  function handleSubmitReview(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()

    if (comment.trim().length === 0) {
      return
    }

    const newReview: UserReview = {
      id: Date.now(),
      reviewerDisplayName: 'Current User',
      rating: selectedRating,
      comment: comment.trim(),
      createdAt: new Date().toISOString(),
    }

    setReviews([newReview, ...reviews])
    setSelectedRating(5)
    setComment('')
  }

  function handleSubmitReport(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()

    if (!reportReason) {
      return
    }

    // TODO: call the backend report endpoint when implemented
    console.log({
      reportedUserId: mockProfile.id,
      reason: reportReason,
      details: reportDetails.trim(),
    })

    setReportSubmitted(true)
    setShowReportForm(false)
    setReportReason('')
    setReportDetails('')
  }

  return (
    <>
      <main className="mx-auto max-w-3xl px-12 py-8">
        <section className="mb-6 flex flex-col gap-6 rounded-2xl border border-gray-200 bg-white p-6 shadow-sm md:flex-row md:items-center">
          {/* public profile header */}
          <div>
            <h1 className="mb-2 text-3xl font-bold text-gray-900">
              {mockProfile.displayName}

              <button
                type="button"
                onClick={() => {
                  setShowReportForm(true)
                  setReportSubmitted(false)
                }}
                className="mt-4 ml-80 inline-flex items-center gap-2 rounded-lg border border-red-200 px-3 py-2 text-xs font-medium text-red-600 transition hover:border-red-300 hover:bg-red-50"
              >
                <FlagIcon className="h-5 w-5" aria-hidden="true" />
                <span>Report Account</span>
              </button>
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

        {/* review form section */}
        <section className="mb-6 rounded-2xl border border-gray-200 bg-white p-6 shadow-sm">
          
          <h2 className="mb-4 text-xl font-semibold text-gray-900">
            Seller Rating
          </h2>

          <div className="mb-4 flex items-center gap-4">
            <div className="text-5xl font-bold text-gray-900">
              {averageRating.toFixed(1)}
            </div>

            <div>
              <StarRating rating={averageRating} size="lg" />
              <p className="text-sm text-gray-500">{reviews.length} reviews</p>
            </div>
          </div>

          <h2 className="mb-6 text-xl font-semibold text-gray-900">
            Leave a Review
          </h2>

          <form onSubmit={handleSubmitReview} className="space-y-4">
            <div>
              <label
                htmlFor="rating"
                className="mb-2 block text-sm font-medium text-gray-700"
              >
                Rating
              </label>

              <select
                id="rating"
                value={selectedRating}
                onChange={(event) => setSelectedRating(Number(event.target.value))}
                className="w-full rounded-lg border border-gray-300 bg-white px-3 py-2 text-gray-900 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value={5}>5 - Excellent</option>
                <option value={4}>4 - Good</option>
                <option value={3}>3 - Okay</option>
                <option value={2}>2 - Poor</option>
                <option value={1}>1 - Bad</option>
              </select>
            </div>

            <div>
              <label
                htmlFor="comment"
                className="mb-2 block text-sm font-medium text-gray-700"
              >
                Comment
              </label>

              <textarea
                id="comment"
                value={comment}
                onChange={(event) => setComment(event.target.value)}
                maxLength={500}
                rows={4}
                placeholder="Share your experience with this seller..."
                className="w-full resize-none rounded-lg border border-gray-300 px-3 py-2 text-gray-900 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />

              <p className="mt-1 text-right text-xs text-gray-500">
                {comment.length}/500
              </p>
            </div>

            <button
              type="submit"
              className="rounded-lg bg-blue-600 px-4 py-2 font-medium text-white shadow-sm hover:bg-blue-700 disabled:cursor-not-allowed disabled:bg-gray-300"
              disabled={comment.trim().length === 0}
            >
              Submit Review
            </button>
          </form>
        </section>

        {/* public reviews section */}
        <section className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm">
          <h2 className="mb-4 text-xl font-semibold text-gray-900">Reviews</h2>

          <div className="space-y-4">
            {reviews.map((review) => (
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

      {showReportForm && (
        <div
          className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
          onClick={() => {
            setShowReportForm(false)
            setReportReason('')
            setReportDetails('')
          }}
        >
          <div
            className="w-full max-w-lg rounded-2xl bg-white p-6 shadow-xl"
            onClick={(event) => event.stopPropagation()}
          >
            <div className="mb-4 flex items-start justify-between">
              <div>
                <h2 className="text-xl font-semibold text-gray-900">
                  Report Account
                </h2>
                <p className="mt-1 text-sm text-gray-600">
                  Tell us why you are reporting this profile.
                </p>
              </div>

              <button
                type="button"
                onClick={() => {
                  setShowReportForm(false)
                  setReportReason('')
                  setReportDetails('')
                }}
                className="rounded-md p-2 text-gray-500 hover:bg-gray-100 hover:text-gray-700"
                aria-label="Close report form"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSubmitReport} className="space-y-4">
              <div>
                <label
                  htmlFor="report-reason"
                  className="mb-2 block text-sm font-medium text-gray-700"
                >
                  Reason
                </label>

                <select
                  id="report-reason"
                  value={reportReason}
                  onChange={(event) => setReportReason(event.target.value)}
                  className="w-full rounded-lg border border-gray-300 bg-white px-3 py-2 text-gray-900 shadow-sm focus:border-red-500 focus:outline-none focus:ring-2 focus:ring-red-200"
                >
                  <option value="">Select a reason</option>
                  <option value="spam">Spam or fake account</option>
                  <option value="harassment">Harassment or inappropriate behavior</option>
                  <option value="scam">Scam or suspicious activity</option>
                  <option value="misleading">Misleading profile information</option>
                  <option value="other">Other</option>
                </select>
              </div>

              <div>
                <label
                  htmlFor="report-details"
                  className="mb-2 block text-sm font-medium text-gray-700"
                >
                  Details
                </label>

                <textarea
                  id="report-details"
                  value={reportDetails}
                  onChange={(event) => setReportDetails(event.target.value)}
                  maxLength={500}
                  rows={4}
                  placeholder="Add any extra details..."
                  className="w-full resize-none rounded-lg border border-gray-300 px-3 py-2 text-gray-900 shadow-sm focus:border-red-500 focus:outline-none focus:ring-2 focus:ring-red-200"
                />

                <p className="mt-1 text-right text-xs text-gray-500">
                  {reportDetails.length}/500
                </p>
              </div>

              <div className="flex justify-end gap-3 pt-2">
                <button
                  type="button"
                  onClick={() => {
                    setShowReportForm(false)
                    setReportReason('')
                    setReportDetails('')
                  }}
                  className="rounded-lg border border-gray-300 px-4 py-2 font-medium text-gray-700 hover:bg-gray-50"
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  disabled={!reportReason}
                  className="rounded-lg bg-red-600 px-4 py-2 font-medium text-white shadow-sm hover:bg-red-700 disabled:cursor-not-allowed disabled:bg-gray-300"
                >
                  Submit Report
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {reportSubmitted && (
        <div className="fixed bottom-6 right-6 z-50 rounded-lg border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700 shadow-lg">
          Report submitted. Thank you for helping keep the marketplace safe.
        </div>
      )}
    </>
  )
}