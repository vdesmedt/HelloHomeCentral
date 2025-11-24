import {useMqtt} from "./MqttProvider.tsx";

const MqttStatus = () => {
    const {isConnected} = useMqtt();

    return <div className={"badge " + (isConnected ? "bg-success" : "bg-danger")}>
        {isConnected ? "MQTT Online" : "MQTT Offline"}
    </div>
}

export default MqttStatus