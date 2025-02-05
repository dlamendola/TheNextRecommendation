namespace Scraper;

public class EpisodeScraper(string initialEpisodeId)
{
    private const string BaseUrl = "https://www.imdb.com/title/";

    public async IAsyncEnumerable<Episode> ScrapeEpisodesAsync()
    {
        var nextUrl = BuildPlotSummaryUrl(initialEpisodeId);
        var httpClient = new HttpClient();

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
        return new Uri($"{BaseUrl}{episodeId}/plotsummary/");
    }
}