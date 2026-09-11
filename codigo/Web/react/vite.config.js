import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';


export default defineConfig({
  plugins: [react()],
  define: {
    'process.env.NODE_ENV': JSON.stringify('production'),
  },
  build: {
    // Salida directa a wwwroot para que ASP.NET la sirva como estático.
    outDir: resolve(__dirname, '../wwwroot/react'),
    emptyOutDir: true,
    lib: {
      entry: resolve(__dirname, 'src/index.jsx'),
      name: 'IsmeReact',
      formats: ['iife'],
      fileName: () => 'react-components.js',
    },
    rollupOptions: {
      output: {
        // Nombre fijo del CSS para poder enlazarlo desde Razor.
        assetFileNames: 'react-components.[ext]',
      },
    },
  },
});
