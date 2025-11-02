import { useEffect, useState } from "react";
import { getHello, getTodos } from "./api";

function App() {
    const [hello, setHello] = useState<string>("");
    const [todos, setTodos] = useState<any[]>([]);

    useEffect(() => {
        getHello().then(x => setHello(x.message)).catch(console.error);
        getTodos().then(setTodos).catch(console.error);
    }, []);

    return (
        <div style={{ padding: 24, fontFamily: "system-ui, sans-serif" }}>
            <h1>Vite + React + ASP.NET Core</h1>
            <p>{hello}</p>
            <h2>Todos</h2>
            <ul>
                {todos.map(t => (
                    <li key={t.id}>
                        #{t.id} — {t.title} {t.done ? "✅" : "⬜️"}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;
