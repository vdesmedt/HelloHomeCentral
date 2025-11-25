import type {PortType} from "../../../api.ts";
import {useMqtt} from "../../MqttProvider.tsx";

const RelayPort = ({port}: { port: PortType }) => {
    const { publish } = useMqtt();
    const lastState = port.history[0]?.newRelayState ?? port.history[0]?.relayState;
    const normalizedState = typeof lastState === "string" ? lastState.toLowerCase() : lastState;
    const isOn = normalizedState === 1 || normalizedState === "1" || normalizedState === "on";
    const relayStateLabel = normalizedState === undefined ? "Unknown" : (isOn ? "On" : "Off");

    const handleClick = (state: "on" | "off") => () => {
        publish(`Node/${port.nodeId}/relay/${port.portNumber}/status`, state);
    };

    return (
        <div className="port-section">
            <div className="port-header">
                <div className="sensor-icon switch-icon">
                    <i className="bi bi-lightning-charge"></i>
                </div>
                <strong>Relay</strong>
            </div>
            <div className="port-item">
                <label className="form-label mb-1">Current State</label>
                <div className="port-value">{relayStateLabel}</div>
                <div className="btn-group simulate-btn" role="group">
                    <button type="button" className="btn btn-success btn-sm" onClick={handleClick("on")}>
                        <i className="bi bi-power me-1"></i> Turn On
                    </button>
                    <button type="button" className="btn btn-secondary btn-sm" onClick={handleClick("off")}>
                        <i className="bi bi-power me-1"></i> Turn Off
                    </button>
                </div>
            </div>
        </div>
    );
};

export default RelayPort;
