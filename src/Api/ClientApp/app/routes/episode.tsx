import type { Route } from "./+types/episode";
import {getEpisode} from "~/api";

export async function clientLoader({params}: Route.ClientLoaderArgs) {
  return await getEpisode(parseInt(params.season), parseInt(params.episode));
}
clientLoader.hydrate = true as const;

export default function Episode({loaderData}: Route.ComponentProps) {
  return (
      <div>
         <h1>{loaderData.title} {`(S${loaderData.season}E${loaderData.episode})`}</h1>
         <p>{loaderData.summary}</p>
      </div>
  );
}