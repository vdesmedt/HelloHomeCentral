import {useEffect, useState} from "react";
import {getNodes, type NodeType} from "../api.ts";

const NodeList = () => {
    const [nodes, setNodes] = useState<NodeType[]>([]);

    useEffect(() => {
        getNodes().then(x => setNodes(x)).catch(console.error);
    }, []);

    const handleDragStart = (e: React.DragEvent<HTMLElement>) => {
        // Add a style/class
        e.currentTarget.classList.add("dragging");

        // DataTransfer (use text/plain for best compatibility; add text/html if you need)
        e.dataTransfer.effectAllowed = "copy";
        e.dataTransfer.setData("text/plain", e.currentTarget.innerText);
        e.dataTransfer.setData("text/html", e.currentTarget.innerHTML);
        e.dataTransfer.setData("application/json", JSON.stringify({
            nodeId: e.currentTarget.dataset.nodeId,
            nodeName: e.currentTarget.dataset.nodeName,
            signature: e.currentTarget.dataset.nodeSig
        }));
    };

    const handleDragEnd = (e: React.DragEvent<HTMLElement>) => {
        e.currentTarget.classList.remove("dragging");
    };

    return (
        <div id="nodeList">
            {nodes.map(t => (
                <div key={t.id} className="node-item"
                     data-node-id={t.id}
                     data-node-name={t.metadata.name}
                     data-node-sig={t.signature}
                     draggable="true"
                     onDragStart={handleDragStart}
                     onDragEnter={handleDragEnd}>
                    <div className="node-id">ID: {t.id}</div>
                    <div className="node-name"><i className="bi bi-hdd-network me-1"></i>{t.metadata.name}</div>
                    <div className="node-address">Signature {t.signature}</div>
                    <i className="bi bi-grip-vertical drag-handle"></i>
                </div>
                )
            )}
        </div>);
}

export default NodeList;