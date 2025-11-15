export const API_BASE = import.meta.env.VITE_API_BASE ?? "/api";

export async function getNodes() {
    const res = await fetch(`${API_BASE}/node`);
    if (!res.ok) throw new Error("API error");
    return res.json();
}

interface Node { id:number, signature:string, metadata: { name:string}}
