import type {NodeType} from "../../api.ts";
import {formatDateString} from "../../Formatters/DateFormatter.ts";

const nodeTitleView = (node:NodeType)=> {
    return <>{node.id}#{node.signature}-<b>{node.metadata.name}</b> <small>(LS: {formatDateString(node.lastSeen)})</small></>
}
export default nodeTitleView