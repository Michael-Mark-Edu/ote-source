export default function BookCard({ title }: { title: string }) {
  return (
    <div className="rounded-2xl border p-2 shadow-sm bg-white">
      <div className="aspect-9/16 rounded-xl grid place-items-center bg-blue-300">
        <span className="text-sm text-black">{title}</span>
      </div>
    </div>
  );
}
