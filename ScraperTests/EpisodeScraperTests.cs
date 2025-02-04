using Scraper;

namespace ScraperTests;

public class EpisodeScraperTests
{
    [Fact]
    public void Test1()
    {
        var scraper = new EpisodeScraper();

        Assert.NotNull(scraper);
    }
}