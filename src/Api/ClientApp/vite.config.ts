import { reactRouter } from "@react-router/dev/vite";
import tailwindcss from "@tailwindcss/vite";
import {defineConfig} from "vite";
import tsconfigPaths from "vite-tsconfig-paths";

export default defineConfig(({command})  => {
  return {
    server: {
      proxy: {
        '/api': 'http://localhost:5000',
      },
    },
    plugins: [tailwindcss(), reactRouter(), tsconfigPaths()],
    resolve: command === 'build' ? {
          alias: {
            'react-dom/server': 'react-dom/server.node', // to get bun and react to get along: https://github.com/remix-run/react-router/issues/12568#issuecomment-2629986004
          },
        }
      : {},
    }
});
