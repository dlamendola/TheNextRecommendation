using Api.Search;
using Api.Search.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shared;

namespace ApiTests;

public class SearchHandlerTests
{
	private readonly Mock<EpisodeSearchService> _episodeSearchServiceMock = new ();
	private readonly SearchHandler _handler;

	public SearchHandlerTests()
	{
		_handler = new SearchHandler(_episodeSearchServiceMock.Object);
	}

	[Fact]
	public async Task Search_NullSearchText()
	{
		var actual = await _handler.Search(new SearchRequest(null));

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
		_episodeSearchServiceMock.Setup(x => x.Search("search text")).ReturnsAsync(new List<Episode>());
		var expected = new SearchApiResponse(new List<EpisodeApiResponse>());

		var actual = await _handler.Search(request);

		Assert.IsType<Ok<SearchApiResponse>>(actual);
		var responseObject = actual as Ok<SearchApiResponse>;
		Assert.Empty(responseObject.Value.Episodes);
	}
}