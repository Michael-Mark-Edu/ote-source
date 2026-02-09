export default function BookCard({ title, bookHeight="w-40"}: { title: string, bookHeight?: string }) {
  return (
    <div className="rounded-2xl border p-2 shadow-sm bg-white">
      <div className={`aspect-9/16 rounded-xl grid place-items-center-safe bg-blue-300 ${bookHeight}`}>
        <span className="text-sm text-black">{title}</span>
      </div>
    </div>
  );
}
