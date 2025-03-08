import type {Route} from "./+types/episode";
import {getEpisode, getRelatedEpisodes} from "~/api";

export async function clientLoader({params}: Route.ClientLoaderArgs) {
    const seasonNumber = parseInt(params.season);
    const episodeNumber = parseInt(params.episode);

    const getEpisodePromise = getEpisode(seasonNumber, episodeNumber);
    const getRelatedEpisodesPromise = getRelatedEpisodes(seasonNumber, episodeNumber);

    const episode = await getEpisodePromise;
    const relatedEpisodes = await getRelatedEpisodesPromise;

    return {episode, relatedEpisodes};
}

clientLoader.hydrate = true as const;

export default function Episode({loaderData}: Route.ComponentProps) {
    const episode = loaderData.episode;
    const relatedEpisodes = loaderData.relatedEpisodes;

    return (
        <div>
            <div>
                <h1>{episode.title} {`(S${episode.season}E${episode.episode})`}</h1>
                <p>{episode.summary}</p>
            </div>
            <div>
                {relatedEpisodes.map((ep, i) => (
                    <div key={i}>
                        <h4>{ep.title}</h4>
                        <p>{ep.summary}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}