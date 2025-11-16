import type {PortType} from "../../../api.ts";

const EnvironmentPortView = (port:PortType) => {
    return (port.history[0].temperature + "°C, " + port.history[0].humidity + "%, " + port.history[0].pressure + "hPa")
}

export default EnvironmentPortView