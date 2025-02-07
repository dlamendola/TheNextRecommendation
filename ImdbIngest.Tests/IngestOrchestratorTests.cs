using System.Data.Common;
using ImdbIngest;
using ImdbIngest.Db;
using Microsoft.Extensions.Logging;
using Moq;
using OpenAI.Embeddings;
using Pgvector;
using Shared;
using Shared.Db;

namespace ImdbIngest.Tests;

public class IngestOrchestratorTests
{
	private readonly IngestOrchestrator _dataLoader;
	private readonly Mock<EmbeddingGenerator> _embeddingGenerator = new(new Mock<EmbeddingClient>().Object);
	private readonly Mock<ILogger<IngestOrchestrator>> _logger = new();
	private readonly Mock<EpisodeScraper> _scraper = new("fakeBaseUrl.com/");
	private readonly Mock<EpisodeStore> _store = new(new Mock<DbDataSource>().Object);

	public IngestOrchestratorTests()
	{
		_dataLoader =
			new IngestOrchestrator(_scraper.Object, _store.Object, _embeddingGenerator.Object, _logger.Object);
	}

	private IAsyncEnumerable<Episode> EmptyEpisodes => Enumerable.Empty<Episode>().ToAsyncEnumerable();

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
			new(7, 25, "The finale", null, "",
				"some details here")
		};
		_scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
		_embeddingGenerator.Setup(x => x.Generate("some details here")).ReturnsAsync(new[] { 1f, 2f, 3f });
		var expectedSavedEpisode = new EpisodeRow(null, 7, 25, "The finale", "", "some details here",
			new Vector(new[] { 1f, 2f, 3f }));

		await _dataLoader.LoadDataAsync();

		_store.Verify(x => x.Save(expectedSavedEpisode), Times.Once);
	}

	[Fact]
	public async Task LoadDataAsync_LogsErrorOnException()
	{
		var episode = new List<Episode>
		{
			new(7, 25, "The finale", null, "",
				"some details here")
		};
		_scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
		_embeddingGenerator.Setup(x => x.Generate("some details here"))
			.ThrowsAsync(new Exception("something went horribly wrong"));

		await _dataLoader.LoadDataAsync();

		_logger.Verify(
			x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
	}

	[Fact]
	public async Task LoadDataAsync_SavesAllEpisodes()
	{
		var episode = new List<Episode>
		{
			new(1, 1, "Episode 1", null, "",
				"some details here"),
			new(1, 2, "Episode 2", null, "",
				"some details here")
		};
		_scraper.Setup(x => x.ScrapeEpisodesAsync(It.IsAny<string>())).Returns(episode.ToAsyncEnumerable());
		_embeddingGenerator.Setup(x => x.Generate("some details here")).ReturnsAsync(new[] { 1f, 2f, 3f });

		await _dataLoader.LoadDataAsync();

		_store.Verify(x => x.Save(It.IsAny<EpisodeRow>()), Times.Exactly(2));
	}
}