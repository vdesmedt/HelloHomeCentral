import {useEffect, useState} from "react";
import {getNodes, type NodeType} from "../api.ts";

const NodeList = () => {
    const [nodes, setNodes] = useState<NodeType[]>([]);

    useEffect(() => {
        getNodes().then(x => setNodes(x)).catch(console.error);
    }, []);

    return (
        <>
            <h3>Nodes</h3>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Signature</th>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                {nodes.map(t => (
                    <tr key={t.id}>
                        <td>{t.id}</td>
                        <td>(#{t.signature})</td>
                        <td>{t. metadata.name}</td>
                    </tr>
                ))}
                </tbody>
            </table>
            <button className="btn btn-primary">Add Node</button>
        </>
    )
}

export default NodeList;