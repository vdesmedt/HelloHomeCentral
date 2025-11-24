import {type PortType, addPulses} from "../../../api.ts";
import {useState} from "react";

const PulsePort = ({port}: {port:PortType}) => {
    const [pulses, setNewPulse] = useState<number>(1);
    const sendPulse =  (portId: number, newPulses:number) =>  async () => {
        await addPulses(portId, newPulses);
    }
    return (
        <>
            <div className="port-section">
                <div className="port-header">
                    <div className="sensor-icon pulse-icon">
                        <i className="bi bi-activity"></i>
                    </div>
                    <strong>Pulse Sensor (Energy Meter)</strong>
                </div>
                <div className="port-item">
                    <label className="form-label mb-1">Pulse Count</label>
                    <div className="port-value">{port?.history[0]?.total}</div>
                    <div className="input-group input-group-sm simulate-btn">
                        <input type="number" className="form-control" placeholder="Pulse count" value={pulses} onChange={e => setNewPulse(parseInt(e.target.value))} />
                        <button className="btn btn-warning" type="button" onClick={sendPulse(port.id, pulses)}>
                            <i className="bi bi-play-fill"></i> Send
                        </button>
                    </div>
                </div>
            </div>
        </>
    );
}

export default PulsePort