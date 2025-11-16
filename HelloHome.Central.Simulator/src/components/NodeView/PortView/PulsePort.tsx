import type {PortType} from "../../../api.ts";

const PulsePortView = (port:PortType) => {
    return port.history[0].total;
}

export default PulsePortView