import { XMarkIcon, ClipboardDocumentCheckIcon, BookmarkIcon, ArrowRightEndOnRectangleIcon } from "@heroicons/react/24/outline";
import { UserIcon as UserIconSolid } from "@heroicons/react/24/solid";

type AccountTabKey = "myListings" | "savedListings" | "account";

export default function AccountPanel({
  isOpen,
  onClose,
  username,
  onGoToAccountTab,
  onSignOut,
}: {
  isOpen: boolean;
  onClose: () => void;
  username: string;
  onGoToAccountTab: (tab: AccountTabKey) => void;
  onSignOut: () => void;
}) {
  return (
    <>
      {/* Overlay */}
      {isOpen && (
        <div className="fixed inset-0 z-40 bg-black/30" onClick={onClose} />
      )}

      {/* Side Panel */}
      <div
        className={`fixed top-0 right-0 z-50 h-full w-80 bg-white shadow-xl transform transition-transform duration-300 ${
          isOpen ? "translate-x-0" : "translate-x-full"
        }`}
      >
        <div className="p-6 flex flex-col h-full">
          {/* Hello [User] */}
          <div className="flex items-start justify-between mb-12">
            <div>
              <p className="text-lg text-gray-500">Hello,</p>
              <p className="text-xl font-semibold">{username}</p>
            </div>

            <button
              onClick={onClose}
              className="p-2 rounded-full hover:bg-gray-100"
              aria-label="Close panel"
            >
              <XMarkIcon className="h-7 w-7 text-gray-700" />
            </button>
          </div>

          {/* Menu Items */}
          <div className="flex flex-col text-gray-700">
            <button
              onClick={() => onGoToAccountTab("account")}
              className="text-xl flex items-center justify-between py-3 px-2 rounded-lg hover:bg-gray-100 transition"
            >
              <span>Profile</span>
              <UserIconSolid className="h-7 w-7 text-gray-600" />
            </button>

            <button
              onClick={() => onGoToAccountTab("myListings")}
              className="text-xl flex items-center justify-between py-3 px-2 rounded-lg hover:bg-gray-100 transition"
            >
              <span>My Listings</span>
              <ClipboardDocumentCheckIcon className="h-7 w-7 text-gray-600" />
            </button>

            <button
              onClick={() => onGoToAccountTab("savedListings")}
              className="text-xl flex items-center justify-between py-3 px-2 rounded-lg hover:bg-gray-100 transition"
            >
              <span>Saved Listings</span>
              <BookmarkIcon className="h-7 w-7 text-gray-600" />
            </button>

            <div className="border-t my-4" />

            <button
              onClick={onSignOut}
              className="text-xl flex items-center justify-between py-3 px-2 rounded-lg hover:bg-red-50 transition"
            >
              <span>Sign Out</span>
              <ArrowRightEndOnRectangleIcon className="h-7 w-7" />
            </button>
          </div>
        </div>
      </div>
    </>
  );
}