import type {Route} from "./+types/episode";
import {getEpisode, getRelatedEpisodes} from "~/api";
import EpisodeCard from "~/components/EpisodeCard";

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
        <div className="episode-container">
            <div className="episode-details">
                <h1>{episode.title} {`(S${episode.season}E${episode.episode})`}</h1>
                <p>{episode.summary}</p>
            </div>
            <div className="related-episodes">
                <h2>Related episodes</h2>
                {relatedEpisodes.map((ep, i) => (
                    <EpisodeCard key={i} seasonNumber={ep.season} episodeNumber={ep.episode} title={ep.title} summary={ep.summary} />
                ))}
            </div>
        </div>
    );
}