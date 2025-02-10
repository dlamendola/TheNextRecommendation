using System.Data.Common;
using Api.Search;
using Api.Search.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using OpenAI.Embeddings;
using Shared;
using Shared.Db;

namespace ApiTests.Search;

public class SearchHandlerTests
{
	private readonly Mock<EpisodeSearchService> _episodeSearchServiceMock;
	private readonly SearchHandler _handler;

	public SearchHandlerTests()
	{
		var embeddingGenerator = new Mock<EmbeddingGenerator>(new Mock<EmbeddingClient>().Object);
		var db = new Mock<EpisodeStore>(new Mock<DbDataSource>().Object);
		_episodeSearchServiceMock = new Mock<EpisodeSearchService>(embeddingGenerator.Object, db.Object);
		_handler = new SearchHandler(_episodeSearchServiceMock.Object);
	}

	[Fact]
	public async Task Search_NullSearchText()
	{
		var actual = await _handler.Search(new SearchRequest(null!));

		Assert.IsType<BadRequest>(actual);
	}

	[Fact]
	public async Task Search_EmptySearchText()
	{
		var actual = await _handler.Search(new SearchRequest(string.Empty));

		Assert.IsType<BadRequest>(actual);
	}

	[Fact]
	public async Task Search_SearchTextOver50Characters()
	{
		string fiftyOneChars = new string('a', 51);
		
		var actual = await _handler.Search(new SearchRequest(fiftyOneChars));

		Assert.IsType<BadRequest>(actual);
	}

	[Fact]
	public async Task Search_ReturnsSearchResults()
	{
		var request = new SearchRequest("search text");
		_episodeSearchServiceMock.Setup(x => x.Search("search text", 5)).ReturnsAsync(new List<Episode>());

		var actual = await _handler.Search(request);

		Assert.IsType<Ok<SearchApiResponse>>(actual);
		var responseObject = actual as Ok<SearchApiResponse>;
		Assert.Empty(responseObject!.Value!.Episodes);
	}
}