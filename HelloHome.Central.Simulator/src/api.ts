export const API_BASE = import.meta.env.VITE_API_BASE ?? "/api";

export async function getNodes() : Promise<Node[]> {
    const res = await fetch(`${API_BASE}/node`);
    if (!res.ok) throw new Error("API error");
    return res.json();
}
export async function getNodeState(id: number) : Promise<Node> {
    const res = await fetch(`${API_BASE}/node/${id}/last-values`);
    if (!res.ok) throw new Error("API error");
    return res.json();
}

export async function restartNode(id: number) : Promise<void> {
    const res = await fetch(`${API_BASE}/node/${id}/restart`, {method: "POST"});
    if (!res.ok) throw new Error("API error");
}

export async function addPulses(portId:number, newPulses:number) : Promise<void> {
    const form = new FormData();
    form.append("pulses", newPulses.toString());
    const res = await fetch(`${API_BASE}/pulse/${portId}`, { method: "POST", body: form });
    if (!res.ok) throw new Error("API error");
}


interface History {
    id:number,
    timestamp:string,
    rssi:number,
    total:number|undefined,
    temperature:number|undefined,
    humidity:number|undefined,
    pressure:number|undefined,
    newRelayState?: number | string,
    relayState?: number | string
}
interface Port { id:number, portNumber:number, nodeId:number, $type:string, history:History[] }
interface Node { id:number, identifier:string, rfAddress:number, signature:string, lastSeen: string, metadata: { name:string }, ports:Port[] }

export type { Node as NodeType, Port as PortType, History as HistoryType}
