import {type RouteConfig, index, route} from "@react-router/dev/routes";

export default [
    index("routes/home.tsx"),
    route("/app/season/:season/ep/:episode", "routes/episode.tsx"),
] satisfies RouteConfig;
