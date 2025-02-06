using System.Data.Common;
using Microsoft.Extensions.Logging;
using Moq;
using OpenAI.Embeddings;
using Pgvector;
using Scraper;
using Scraper.Db;

namespace ScraperTests;

public class DataLoaderTests
{
    private readonly Mock<EpisodeScraper> _scraper = new("fakeBaseUrl.com/");
    private readonly Mock<EpisodeStore> _store = new(new Mock<DbDataSource>().Object);
    private readonly Mock<EmbeddingGenerator> _embeddingGenerator = new(new Mock<EmbeddingClient>().Object);
    private readonly Mock<ILogger<DataLoader>> _logger = new();

    private readonly DataLoader _dataLoader;

    public DataLoaderTests()
    {
        _dataLoader = new DataLoader(_scraper.Object, _store.Object, _embeddingGenerator.Object, _logger.Object);
    }

    [Fact]
    public async Task LoadDataAsync_NoEpisodes()
    {
        _scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(EmptyEpisodes);

        await _dataLoader.LoadDataAsync();

        _embeddingGenerator.VerifyNoOtherCalls();
        _store.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoadDataAsync_StopsWhenNoNextEpisode()
    {
        var episode = new List<Episode>
        {
            new(Season: 7, EpisodeInSeason: 25, Title: "The finale", NextEpisodeId: null, Summary: "",
                Synopsis: "some details here")
        };
        _scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
        _embeddingGenerator.Setup(x => x.Generate("some details here")).ReturnsAsync(new float[] { 1f, 2f, 3f });
        var expectedSavedEpisode = new EpisodeRow(null, 7, 25, "The finale", "", "some details here",
            new Vector(new float[] { 1f, 2f, 3f }));

        await _dataLoader.LoadDataAsync();

        _store.Verify(x => x.Save(expectedSavedEpisode), Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_LogsErrorOnException()
    {
        var episode = new List<Episode>
        {
            new(Season: 7, EpisodeInSeason: 25, Title: "The finale", NextEpisodeId: null, Summary: "",
                Synopsis: "some details here")
        };
        _scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
        _embeddingGenerator.Setup(x => x.Generate("some details here"))
            .ThrowsAsync(new Exception("something went horribly wrong"));
        
        await _dataLoader.LoadDataAsync();
        
        _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_SavesAllEpisodes()
    {
        var episode = new List<Episode>
        {
            new(Season: 1, EpisodeInSeason: 1, Title: "Episode 1", NextEpisodeId: null, Summary: "",
                Synopsis: "some details here"),
            new(Season: 1, EpisodeInSeason: 2, Title: "Episode 2", NextEpisodeId: null, Summary: "",
                Synopsis: "some details here")
        };
        _scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
        _embeddingGenerator.Setup(x => x.Generate("some details here")).ReturnsAsync(new float[] { 1f, 2f, 3f });
        var expectedSavedEpisode = new EpisodeRow(null, 7, 25, "The finale", "", "some details here",
            new Vector(new float[] { 1f, 2f, 3f }));

        await _dataLoader.LoadDataAsync();

        _store.Verify(x => x.Save(It.IsAny<EpisodeRow>()), Times.Exactly(2));
    }

    IAsyncEnumerable<Episode> EmptyEpisodes => Enumerable.Empty<Episode>().ToAsyncEnumerable();
}