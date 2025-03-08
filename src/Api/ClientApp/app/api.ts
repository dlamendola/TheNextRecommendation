import { z } from "zod";

const EpisodeSchema = z.object({
    season: z.number(),
    episode: z.number(),
    title: z.string(),
    summary: z.string(),
});

const SearchResponseSchema = z.object({
    episodes: EpisodeSchema.array()
});

const RelatedEpisodesResponseSchema = z.object({
    relatedEpisodes: EpisodeSchema.array()
});

export type Episode = z.infer<typeof EpisodeSchema>;

export async function search(query: string): Promise<Episode[]> {
    const request = new Request("/api/search", {
        method: "POST",
        body: JSON.stringify({searchText: query}),
        headers: {'Content-Type': 'application/json'}
    });

    const response = await fetch(request);
    if (response.status !== 200) {
        throw new Error(`Search failed: ${response.status}`);
    }

    const episodes = await response.json();
    
    const validatedEpisodes = SearchResponseSchema.parse(episodes);

    return validatedEpisodes.episodes;
}

export async function getEpisode(seasonNumber: number, episodeNumber: number): Promise<Episode> {
    const response = await fetch(`/api/s/${seasonNumber}/e/${episodeNumber}`);
    if (response.status !== 200) {
        throw new Error(`Failed to fetch episode: ${response.status}`);
    }

    const episode = await response.json();

    const validatedEpisode = EpisodeSchema.parse(episode);

    return validatedEpisode;
}

export async function getRelatedEpisodes(seasonNumber: number, episodeNumber: number): Promise<Episode[]> {
    const response = await fetch(`/api/s/${seasonNumber}/e/${episodeNumber}/related`);
    if (response.status !== 200) {
        throw new Error(`Failed to fetch related episode: ${response.status}`);
    }
    const episodes = await response.json();
    const validatedEpisodes = RelatedEpisodesResponseSchema.parse(episodes);

    return validatedEpisodes.relatedEpisodes;
}