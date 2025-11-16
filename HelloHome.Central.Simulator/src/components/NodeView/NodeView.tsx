import {useEffect, useState} from "react";
import {getNodeState, type NodeType, type PortType} from "../../api.ts";
import pulsePortView from "./PortView/PulsePort.tsx";
import EnvironmentPortView from "./PortView/EnvironmentPort.tsx";
import nodeTitleView from "./NodeTitle.tsx";
import nodeActionView from "./NodeActionView.tsx";

const NodeView = ({nodeId} : {nodeId:number}) => {
    const [node, setNode] = useState<NodeType>();

    useEffect(() => {
        getNodeState(nodeId).then(x => setNode(x)).catch(console.error);
    }, [nodeId]);

    const portView = (port:PortType) => {
        switch(port.$type) {
            case "Pulse": return pulsePortView(port);
            case "Environment": return EnvironmentPortView(port);
        }
    }

    return (
        <>
            <div>{node == undefined || nodeTitleView(node)}</div>
            <div>{node == undefined || nodeActionView(node)}</div>
            <ul>
                {node?.ports?.map(p =>
                    <li key={p.id}>{p.$type} : {portView(p)}
                    </li>)
                }
            </ul>
        </>
    );
}

export default NodeView