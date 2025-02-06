using Pgvector;
using Scraper.Db;

namespace Scraper;

public class DataLoader(EpisodeStore episodeStore, EmbeddingGenerator embeddingGenerator)
{
    public async Task LoadDataAsync()
    {
        const string firstEpisodeId = "tt0094030";

        await foreach (var ep in EpisodeScraper.ScrapeEpisodesAsync(firstEpisodeId))
        {
            await SaveEpisode(ep);
        }
    }

    private async Task SaveEpisode(Episode ep)
    {
        var vector = await embeddingGenerator.Generate(ep.Synopsis);

        var row = new EpisodeRow(
            Id: null,
            SeasonNumber: ep.Season,
            EpisodeNumber: ep.EpisodeInSeason,
            Title: ep.Title,
            Summary: ep.Summary,
            Synopsis: ep.Synopsis,
            SynopsisEmbedding: new Vector(vector.ToArray()));

        await episodeStore.Save(row);
    }
}