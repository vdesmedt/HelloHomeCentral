import type {PortType} from "../../../api.ts";
import {useMqtt} from "../../MqttProvider.tsx";
import React from "react";

const SwitchPort = ({port}: {port:PortType}) => {
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
                <strong>Switch Sensor</strong>
            </div>
            <div className="port-item">
                <label className="form-label mb-1">Switch State</label>
                <div className="port-value">STATE</div>
                <div className="btn-group simulate-btn" role="group">
                    <button type="button" className="btn btn-success btn-sm switch-on" onClick={handleClick}>
                        <i className="bi bi-toggle-on me-1"></i> Simulate ON
                    </button>
                    <button type="button" className="btn btn-secondary btn-sm switch-off" onClick={handleClick}>
                        <i className="bi bi-toggle-off me-1"></i> Simulate OFF
                    </button>
                </div>
            </div>
        </div>
    );
}

export default SwitchPort;

