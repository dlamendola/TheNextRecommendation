using System.Data.Common;
using Api.Search;
using Moq;
using OpenAI.Embeddings;
using Pgvector;
using Shared;
using Shared.Db;

namespace ApiTests.Search;

public class EpisodeSearchServiceTests
{
	[Fact]
	public async Task Search()
	{
		var embeddingGenerator = new Mock<EmbeddingGenerator>(new Mock<EmbeddingClient>().Object);
		var db = new Mock<EpisodeStore>(new Mock<DbDataSource>().Object);
		var service = new EpisodeSearchService(embeddingGenerator.Object, db.Object);
		embeddingGenerator.Setup(x => x.Generate("search text")).ReturnsAsync(new []{1f, 2f, 3f});
		db.Setup(x => x.SearchBySemanticSimilarity(new Vector(new[] { 1f, 2f, 3f }), 1)).ReturnsAsync(new List<EpisodeRow>
		{
			new EpisodeRow(null, SeasonNumber: 3, EpisodeNumber: 12, Title: "Darmok", "some summary", "and a details synopsis", null)
		});
		var expected = new Episode(3, 12, "Darmok", null, "some summary", "and a details synopsis");

		var actual = await service.Search("search text", 1);

		Assert.Single(actual);
		Assert.Equal(expected, actual[0]);
	}
}