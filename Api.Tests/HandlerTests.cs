using Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiTests;

public class HandlerTests
{
	private readonly Handler _handler;
	private Mock<EpisodeSearchService> _episodeSearchServiceMock;

	public HandlerTests()
	{
		_episodeSearchServiceMock = new Mock<EpisodeSearchService>();
		_handler = new Handler(_episodeSearchServiceMock.Object, new Mock<ILogger<Handler>>().Object);
	}

	[Fact]
	public void HealthCheck_Ok()
	{
		var actual = _handler.HealthCheck();

		Assert.IsType<Ok>(actual);
	}

	[Fact]
	public void Search_NullSearchText()
	{
		var actual = _handler.Search(new SearchRequest(null));

		Assert.IsType<BadRequest>(actual);
	}

	[Fact]
	public void Search_EmptySearchText()
	{
		var actual = _handler.Search(new SearchRequest(string.Empty));

		Assert.IsType<BadRequest>(actual);
	}

	[Fact]
	public void Search_SearchTextOver50Characters()
	{
		string fiftyOneChars = new string('a', 51);
		
		var actual = _handler.Search(new SearchRequest(fiftyOneChars));

		Assert.IsType<BadRequest>(actual);
	}
}