import type {PortType} from "../../../api.ts";
import {useMqtt} from "../../MqttProvider.tsx";
import React from "react";

const PushButtonPort = ({port}: {port:PortType}) => {
    const { publish } = useMqtt();

    const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        const target = e.target as HTMLElement;
        if(target.classList.contains("switch-on"))
            publish("Node/" + port.nodeId + "/push/" + port.portNumber + "/status", "on")
        else if(target.classList.contains("switch-off"))
            publish("Node/" + port.nodeId + "/push/" + port.portNumber + "/status", "off")
    };

    return (
        <div className="port-section">
            <div className="port-header">
                <div className="sensor-icon switch-icon">
                    <i className="bi bi-toggle-on"></i>
                </div>
                <strong>Push Sensor</strong>
            </div>
            <div className="port-item">
                <div className="btn-group simulate-btn" role="group">
                    <button type="button" className="btn btn-success btn-sm switch-on" onClick={handleClick}>
                        <i className="bi bi-toggle-on me-1"></i> Simulate Push
                    </button>
                </div>
            </div>
        </div>
    );
}

export default PushButtonPort;

