namespace Scraper;

public static class EpisodeScraper
{
    private const string BaseUrl = "https://www.imdb.com/title/";

    public static async IAsyncEnumerable<Episode> ScrapeEpisodesAsync(string initialEpisodeId)
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

    private static Uri BuildPlotSummaryUrl(string episodeId)
    {
        return new Uri($"{BaseUrl}{episodeId}/plotsummary/");
    }
}