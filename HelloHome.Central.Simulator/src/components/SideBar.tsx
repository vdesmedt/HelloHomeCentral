import NodeList from "./NodeList.tsx";
import MqttStatus from "./MqttStatus.tsx";

const SideBar = () => {
    return (
        <div className="sidebar">
            <h5><i className="bi bi-list-ul me-2"></i>Available Nodes</h5>
            <div className="drag-instruction">
                <i className="bi bi-info-circle me-1"></i>
                <strong>Drag & Drop</strong> nodes to detail slots
            </div>
            <NodeList/>
            <MqttStatus/>
        </div>
    )
}

export default SideBar;