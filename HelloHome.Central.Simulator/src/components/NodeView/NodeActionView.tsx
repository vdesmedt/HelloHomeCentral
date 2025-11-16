import type {NodeType} from "../../api.ts";
import { restartNode } from "../../api.ts";
const nodeActionView = (node:NodeType) => {
    return <button onClick={restart(node.id)}>Restart</button>
}

const restart = (nodeId:number) => async () => {
    console.log("restarting node " + nodeId);
    await restartNode(nodeId);
}

export default nodeActionView