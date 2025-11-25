import {type PortType, addPulses} from "../../../api.ts";
import {useState} from "react";

const PulsePort = ({port}: {port:PortType}) => {
    const [pulses, setNewPulse] = useState<number>(1);
    const sendPulse =  (portId: number, newPulses:number) =>  async () => {
        await addPulses(portId, newPulses);
    }
    return (
        <div className="port-section">
            <div className="port-header">
                <div className="sensor-icon pulse-icon">
                    <i className="bi bi-activity"></i>
                </div>
                <div className="port-title">
                    <strong>Pulse Sensor</strong>
                    <small>Port #{port.portNumber}</small>
                </div>
            </div>
            <div className="port-body">
                <div className="port-stat">
                    <p className="label">Pulse Count</p>
                    <div className="port-value">{port?.history[0]?.total}</div>
                    <div className="simulate-group">
                        <input type="number" className="form-control form-control-sm" placeholder="Pulse count" value={pulses} onChange={e => setNewPulse(parseInt(e.target.value))} />
                        <button className="btn btn-warning btn-sm" type="button" onClick={sendPulse(port.id, pulses)}>
                            <i className="bi bi-play-fill"></i> Send
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default PulsePort