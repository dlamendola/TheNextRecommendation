using ImdbIngest.Db;
using Microsoft.Extensions.Logging;
using Pgvector;

namespace ImdbIngest;

public class IngestOrchestrator(
	EpisodeScraper scraper,
	EpisodeStore episodeStore,
	EmbeddingGenerator embeddingGenerator,
	ILogger<IngestOrchestrator> logger)
{
	private const string FirstEpisodeId = "tt0094030";

	public async Task LoadDataAsync()
	{
		var tasks = new List<Task>();

		await foreach (var ep in scraper.ScrapeEpisodesAsync(FirstEpisodeId))
		{
			logger.LogInformation($"Scraped and parsed S{ep.Season}E{ep.EpisodeInSeason}");

			tasks.Add(SaveAndLogEpisode(ep));
		}

		await Task.WhenAll(tasks);
	}

	private async Task SaveAndLogEpisode(Episode ep)
	{
		try
		{
			await SaveEpisode(ep);

			logger.LogInformation("Generated embedding and saved S{Season}E{Episode}", ep.Season, ep.EpisodeInSeason);
		}
		catch (Exception e)
		{
			logger.LogError(e, "Failed to save episode S{Season}E{Episode}", ep.Season, ep.EpisodeInSeason);
		}
	}

	private async Task SaveEpisode(Episode ep)
	{
		var vector = await embeddingGenerator.Generate(ep.Synopsis);

		var row = new EpisodeRow(
			null,
			ep.Season,
			ep.EpisodeInSeason,
			ep.Title,
			ep.Summary,
			ep.Synopsis,
			new Vector(vector.ToArray()));

		await episodeStore.Save(row);
	}
}