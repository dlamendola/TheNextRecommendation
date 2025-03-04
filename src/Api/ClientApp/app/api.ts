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