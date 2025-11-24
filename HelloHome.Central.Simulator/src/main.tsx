import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import App from './App.tsx'
import {MqttProvider} from "./components/MqttProvider.tsx";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <MqttProvider
            url="ws://127.0.0.1:8082"
            options={{}}>
            <App/>
        </MqttProvider>
    </StrictMode>,
)
