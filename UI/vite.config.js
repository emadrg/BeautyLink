import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import mkcert from 'vite-plugin-mkcert'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [mkcert(), react()],
  optimizeDeps: {
    include: ['@mui/material', '@emotion/react', '@emotion/styled','@mui/material/Tooltip'],
  }
})
