import {type PortType, addPulses} from "../../../api.ts";
import {useState} from "react";

const PulsePort = ({port}: {port:PortType}) => {
    const [pulses, setNewPulse] = useState<number>(1);

    const sendPulse =  (portId: number, newPulses:number) =>  async () => {
        await addPulses(portId, newPulses);
    }
    return (
        <>
            Current Pulses :{port?.history[0]?.total}
            <input type="number" value={pulses} onChange={e => setNewPulse(parseInt(e.target.value))}/>
            <button onClick={sendPulse(port.id, pulses)}>Send</button>
        </>
    );
}

export default PulsePort