import type {PortType} from "../../../api.ts";
import {useEffect, useState} from "react";

const defaultEnvironment = { temperature: 20, humidity: 50, pressure: 1013 };

const EnvironmentPortView = ({port} : {port:PortType}) => {
    const [env, setEnv] = useState<{temperature:number, humidity:number, pressure:number}>(defaultEnvironment);
     useEffect(() => {
         const latest = port.history[0];
         if (!latest) return;
         setEnv({
             temperature: latest.temperature ?? defaultEnvironment.temperature,
             humidity: latest.humidity ?? defaultEnvironment.humidity,
             pressure: latest.pressure ?? defaultEnvironment.pressure
         })
     }, [port]);

    return (
        <div className="port-section">
            <div className="port-header">
                <div className="sensor-icon environment-icon">
                    <i className="bi bi-thermometer-half"></i>
                </div>
                <div className="port-title">
                    <strong>Environment</strong>
                    <small>Port #{port.portNumber}</small>
                </div>
            </div>
            <div className="port-body">
                <div className="port-stat">
                    <p className="label"><i className="bi bi-thermometer"></i>Temperature</p>
                    <div className="port-value">{port.history[0]?.temperature ?? "-"}°C</div>
                    <div className="simulate-group">
                        <input type="number" className="form-control form-control-sm" placeholder="Value"
                               value={env.temperature} onChange={e => setEnv({...env, temperature: parseInt(e.target.value)})}/>
                    </div>
                </div>
                <div className="port-stat">
                    <p className="label"><i className="bi bi-droplet"></i>Humidity</p>
                    <div className="port-value">{port.history[0]?.humidity ?? "-"}%</div>
                    <div className="simulate-group">
                        <input type="number" className="form-control form-control-sm" placeholder="Value"
                               value={env.humidity} onChange={e => setEnv({...env, humidity: parseInt(e.target.value)})}/>
                    </div>
                </div>
                <div className="port-stat">
                    <p className="label"><i className="bi bi-speedometer"></i>Pressure</p>
                    <div className="port-value">{port.history[0]?.pressure ?? "-"} hPa</div>
                    <div className="simulate-group">
                        <input type="number" className="form-control form-control-sm" placeholder="Value"
                               value={env.pressure} onChange={e => setEnv({...env, pressure: parseInt(e.target.value)})}/>
                        <button className="btn btn-primary btn-sm" type="button">
                            <i className="bi bi-play-fill"></i> Simulate
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )

}

export default EnvironmentPortView