import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()], 
    server: {
      host: true, 
      port: 5173,
      strictPort: true,
      proxy: {
        "/api": {
            target: 'http://host.docker.internal:8080/',
            changeOrigin: true,
            rewrite: (path) => path.replace(/^\/api/, ''),
            configure: (proxy, options) => {
                proxy.on('proxyReq', (_proxyReq, req) => {
                    console.log(`[proxy →] ${req.method} ${req.url} -> ${options.target}`)
                })
                proxy.on('proxyRes', (proxyRes, req) => {
                    console.log(`[proxy ←] ${req.method} ${req.url} ${proxyRes.statusCode}`)
                })
                proxy.on('error', (err, req) => {
                    console.error(`[proxy ✖] ${req.method} ${req.url} ${err.message}`)
                })
            }
        }
      }
    }
})
