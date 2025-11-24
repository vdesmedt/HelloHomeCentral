import NodeSlot from "./NodeView/NodeSlot.tsx";

const NodeGrid = () => {


    return (
        <div className="row" id="nodeDetailsContainer">
            <NodeSlot slotNumber={1}/>
            <NodeSlot slotNumber={2}/>
            <NodeSlot slotNumber={3}/>
            <NodeSlot slotNumber={4}/>
            <NodeSlot slotNumber={5}/>
            <NodeSlot slotNumber={6}/>
        </div>
    );
}

export default NodeGrid;
