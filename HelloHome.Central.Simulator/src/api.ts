export const API_BASE = import.meta.env.VITE_API_BASE ?? "/api";

export async function getHello() {
    const res = await fetch(`${API_BASE}/hello`);
    if (!res.ok) throw new Error("API error");
    return res.json() as Promise<{ message: string }>;
}

export async function getTodos() {
    const res = await fetch(`${API_BASE}/todos`);
    if (!res.ok) throw new Error("API error");
    return res.json();
}
