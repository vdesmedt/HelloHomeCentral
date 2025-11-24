import {useState} from "react";
import {getNodeState, type NodeType} from "../../api.ts";
import NodeView from "./NodeView.tsx";

const NodeSlot = ({slotNumber}: { slotNumber: number }) => {
    const [node, setNode] = useState<NodeType | null>(null);
    const handleDragOver = (e: React.DragEvent<HTMLElement>) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = "copy";
        // Add a style/class
        e.currentTarget.classList.add("drag-over");
    };

    const handleDragLeave = (e: React.DragEvent<HTMLElement>) => {
        e.currentTarget.classList.remove("drag-over");
    };

    const handleDragDrop = (e: React.DragEvent<HTMLElement>) => {
        e.preventDefault();
        e.currentTarget.classList.remove("drag-over");
        const nodeData = JSON.parse(e.dataTransfer.getData("application/json"))
        getNodeState(nodeData.nodeId).then(x => setNode(x));
    }

    const clearSlot = () => setNode(null);

    return (
        <div className="col-lg-4 col-md-6 mb-3 drop-zone"
             onDragOver={handleDragOver} onDragLeave={handleDragLeave} onDrop={handleDragDrop}
             data-slot="{slotNumber}">
            {node == null ? (
                <div className="empty-slot">
                    <i className="bi bi-plus-circle"></i>
                    <h5>Slot {slotNumber}</h5>
                    <p className="mb-0">Drag a node here to display its details</p>
                </div>
            ) : (
                <NodeView node={node} clearSlot={clearSlot}/>
            )
            }
        </div>
    );
}

export default NodeSlot