type SearchResponse = {
    episodes: Episode[];
}

export type Episode = {
    season: number;
    episode: number;
    title: string;
    summary: string;
}

export async function search(query: string): Promise<SearchResponse> {
    const request = new Request("/api/search", {
        method: "POST",
        body: JSON.stringify({searchText: query}),
        headers: {'Content-Type': 'application/json'}
    });
    console.log(request);

    const response = await fetch(request);
    if (response.status !== 200) {
        throw new Error(`Search failed: ${response.status}`);
    }

    // todo: validate response data
    const episodes = await response.json();

    return episodes;
}