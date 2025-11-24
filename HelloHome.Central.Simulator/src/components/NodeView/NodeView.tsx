import {type NodeType, type PortType, restartNode} from "../../api.ts";
import EnvironmentPortView from "./PortView/EnvironmentPort.tsx";
import PulsePort from "./PortView/PulsePort.tsx";
import SwitchPort from "./PortView/SwitchPort.tsx";
import PushButtonPort from "./PortView/PushButtonPort.tsx";

const NodeView = ({node, clearSlot}: { node: NodeType, clearSlot:() => void}) => {

    async function handleRestart() {
        await restartNode(node.id);
    }
    function showPort(port:PortType) {
        switch (port.$type) {
            case "Environment": return <EnvironmentPortView port={port}/>;
            case "Pulse" : return <PulsePort port={port}/>
            case "Switch" : return <SwitchPort port={port}/>
            case "PushButton" : return <PushButtonPort port={port}/>
        }
    }

    return (
        <div className="node-card">
            <button className="close-btn" title="Remove node" onClick={clearSlot}>
                <i className="bi bi-x-lg"></i>
            </button>
            <div className="node-card-header position-relative">
                <h6>
                    <i className="bi bi-hdd-network me-2"></i>{node.metadata.name}
                    <button className="restart-btn" title="Restart" onClick={handleRestart}>
                        <i className="bi bi-arrow-repeat"></i>
                    </button>
                </h6>
                <small>ID: {node.id} | RF Address: {node.rfAddress}</small>
                <span className="badge status-badge ${statusClass}">Online</span>
            </div>
            {node.ports.map(p => <div key={p.id}>{showPort(p)}</div>)}
        </div>)
}
export default NodeView