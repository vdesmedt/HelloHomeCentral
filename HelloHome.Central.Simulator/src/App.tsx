import "./App.css";
import Banner from "./components/Banner.tsx";
import NodeGrid from "./components/NodeGrid.tsx";
import SideBar from "./components/SideBar.tsx";
function App() {
    return (
        <>
        <Banner headerText="HelloHome Simulator"/>
        <div className="container-fluid">
            <div className='row' >
                {/* Sidebar - Node List */}
                <div className='col-lg-2 col-md-3'>
                    <SideBar />
                </div>
                {/* Main Content - Node Details (Up to 4) */}
                <div className='col-lg-10 col-md-9'>
                    <NodeGrid />
                </div>
                </div>
        </div>
        </>
    );
}
export default App;
