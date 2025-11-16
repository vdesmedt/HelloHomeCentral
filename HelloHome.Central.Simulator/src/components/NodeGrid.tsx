import NodeView from "./NodeView/NodeView.tsx";

const NodeGrid = (  { ids } : { ids:number[] } ) => {
    return (<>
        <div className='row'>
            <div className='col-md-5 border border-1 p-3'>
                <NodeView nodeId={ids[0]}/>
            </div>
            <div className='col-md-5 border border-1 p-3'>
                <NodeView nodeId={ids[1]}/>
            </div>
        </div>
        <div className='row'>
            <div className='col-md-5 border border-1 p-3'>
                <NodeView nodeId={ids[2]}/>
            </div>
            <div className='col-md-5 border border-1 p-3'>
                <NodeView nodeId={ids[3]}/>
            </div>
        </div>
    </>);
}

export default NodeGrid;
