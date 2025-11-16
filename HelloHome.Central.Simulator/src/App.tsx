import "./App.css";
import Banner from "./components/Banner.tsx";
import NodeList from "./components/NodeList.tsx";
import NodeGrid from "./components/NodeGrid.tsx";
function App() {
    return (
        <>
        <Banner headerText="HelloHome Simulator"/>
        <div className='row' >
            <div className='col-md-2'>
                <NodeList />
            </div>
            <div className='col-md-10'>
                <NodeGrid ids={[1,3,7,5]}/>
            </div>
        </div>
        </>
    );
}

export default App;
