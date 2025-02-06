namespace ImdbIngest;

public class EpisodeScraper(string baseUrl)
{
	public virtual async IAsyncEnumerable<Episode> ScrapeEpisodesAsync(string initialEpisodeId)
	{
		var nextUrl = BuildPlotSummaryUrl(initialEpisodeId);
		using var httpClient = new HttpClient();

		while (nextUrl != null)
		{
			var html = await httpClient.GetStringAsync(nextUrl);
			var episode = Parser.ParseHtml(html);

			yield return episode;

			if (episode.NextEpisodeId != null)
			{
				nextUrl = BuildPlotSummaryUrl(episode.NextEpisodeId);
			}
			else
			{
				nextUrl = null;
			}
		}
	}

	private Uri BuildPlotSummaryUrl(string episodeId)
	{
		return new Uri($"{baseUrl}{episodeId}/plotsummary/");
	}
}