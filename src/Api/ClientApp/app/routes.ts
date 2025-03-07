import {type RouteConfig, index, route} from "@react-router/dev/routes";

export default [
    index("routes/home.tsx"),
    route("/season/:season/ep/:episode", "routes/episode.tsx"),
] satisfies RouteConfig;
