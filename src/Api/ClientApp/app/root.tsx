import {
  isRouteErrorResponse,
  Links,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "react-router";

import type { Route } from "./+types/root";
import "./app.css";
import githubLogo from "./assets/github-mark-white.svg";

export const links: Route.LinksFunction = () => [
  { rel: "preload", href: "/fonts/TNG_Title.ttf", as: "font", type: "font/ttf", crossOrigin: "anonymous" },
];

export function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body>
        <main>
          {children}
          <ScrollRestoration/>
          <Scripts/>
        </main>
        <footer>
          <a href="https://github.com/dlamendola/TheNextRecommendation" target="_blank"
             aria-label="View source code on GitHub">
            <img src={githubLogo} alt="GitHub" width="32" height="32"/>
          </a>
        </footer>
      </body>
    </html>
  );
}

export function HydrateFallback() {
  return (
      <div id="loading-splash">
        <div id="loading-splash-spinner" />
        <p>Loading...</p>
      </div>
  );
}

export default function App() {
  return <Outlet/>;
}

export function ErrorBoundary({error}: Route.ErrorBoundaryProps) {
  let message = "Oops!";
  let details = "An unexpected error occurred.";
  let stack: string | undefined;

  if (isRouteErrorResponse(error)) {
    message = error.status === 404 ? "404" : "Error";
    details =
      error.status === 404
        ? "The requested page could not be found."
        : error.statusText || details;
  } else if (import.meta.env.DEV && error && error instanceof Error) {
    details = error.message;
    stack = error.stack;
  }

  return (
    <main className="pt-16 p-4 container mx-auto">
      <h1>{message}</h1>
      <p>{details}</p>
      {stack && (
        <pre className="w-full p-4 overflow-x-auto">
          <code>{stack}</code>
        </pre>
      )}
    </main>
  );
}
