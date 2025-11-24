// MqttContext.tsx
import React, {
    createContext,
    useContext,
    useEffect,
    useRef,
    useState,
} from "react";
import mqtt from "mqtt";
import type { MqttClient, IClientOptions, IClientPublishOptions } from "mqtt";

type MqttContextValue = {
    client: MqttClient | null;
    isConnected: boolean;
    publish: (topic: string, payload: string | Buffer, options?: IClientPublishOptions) => void;
};

const MqttContext = createContext<MqttContextValue | undefined>(undefined);

type MqttProviderProps = {
    url: string; // e.g. wss://your-broker:9002
    options?: IClientOptions;
    children: React.ReactNode;
};

export const MqttProvider: React.FC<MqttProviderProps> = ({
                                                              url,
                                                              options,
                                                              children,
                                                          }) => {
    const clientRef = useRef<MqttClient | null>(null);
    const [isConnected, setIsConnected] = useState(false);

    useEffect(() => {
        // Create client once
        const client = mqtt.connect(url, {
            clientId: "react-" + Math.random().toString(16).slice(2),
            clean: true,
            reconnectPeriod: 1000,
            ...options,
        });

        clientRef.current = client;

        client.on("connect", () => {
            setIsConnected(true);
            console.log("[MQTT] connected");
        });

        client.on("reconnect", () => {
            setIsConnected(false);
            console.log("[MQTT] reconnecting...");
        });

        client.on("close", () => {
            setIsConnected(false);
            console.log("[MQTT] connection closed");
        });

        client.on("error", (err) => {
            console.error("[MQTT] error", err);
        });

        // Clean up on unmount
        return () => {
            console.log("[MQTT] ending connection");
            client.end(true);
            clientRef.current = null;
        };
    }, [url, JSON.stringify(options)]);

    const publish = (
        topic: string,
        payload: string | Buffer,
        pubOptions?: IClientPublishOptions
    ) => {
        const client = clientRef.current;
        if (!client || !isConnected) return;
        client.publish(topic, payload, pubOptions);
    };

    return (
        <MqttContext.Provider
            value={{
                client: clientRef.current,
                isConnected,
                publish,
            }}
        >
            {children}
        </MqttContext.Provider>
    );
};

export function useMqtt() {
    const ctx = useContext(MqttContext);
    if (!ctx) {
        throw new Error("useMqtt must be used inside an MqttProvider");
    }
    return ctx;
}
