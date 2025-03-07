using System.Data.Common;
using Api.Search;
using Api.Search.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shared.Db;

namespace ApiTests.Search;

public class EpisodeHandlerTests
{
	private readonly EpisodeHandler _handler;
	private readonly Mock<EpisodeStore> _store;

	public EpisodeHandlerTests()
	{
		_store = new Mock<EpisodeStore>(new Mock<DbDataSource>().Object);
		_handler = new EpisodeHandler(_store.Object);
	}

	[Fact]
	public async Task GetBySeasonAndEpisodeNumber_NotFound()
	{
		_store.Setup(x => x.GetBySeasonAndEpisodeNumber(1, 1)).ReturnsAsync(default(EpisodeRow));
		
		var actual = await _handler.GetBySeasonAndEpisodeNumber(1, 1);

		Assert.Equal(Results.NotFound(), actual);
	}

	[Fact]
	public async Task GetBySeasonAndEpisodeNumber_Found()
	{
		_store.Setup(x => x.GetBySeasonAndEpisodeNumber(2, 3))
			.ReturnsAsync(new EpisodeRow(1, 2, 3, "title", "summary", "synopsis", null));
		var expected = new EpisodeApiResponse(2, 3, "title", "summary");
		
		var actual = await _handler.GetBySeasonAndEpisodeNumber(2, 3);

		Assert.IsType<Ok<EpisodeApiResponse>>(actual);
		var responseBody = actual as Ok<EpisodeApiResponse>;
		Assert.Equal(expected, responseBody.Value);
	}

	[Fact]
	public async Task GetRelatedEpisodes_NotFound()
	{
		_store.Setup(x => x.GetRelatedEpisodes(1, 1)).ReturnsAsync([]);

		var actual = await _handler.GetRelatedEpisodes(1, 1);

		Assert.Equal(Results.NotFound(), actual);
	}

	[Fact]
	public async Task GetRelatedEpisodes_Ok()
	{
		_store.Setup(x => x.GetRelatedEpisodes(2, 3)).ReturnsAsync([
			new EpisodeRow(1, 2, 3, "title", "summary", "synopsis", null),
			new EpisodeRow(2, 2, 4, "title", "summary", "synopsis", null)]);
		var expected = new RelatedEpisodesApiResponse([
			new EpisodeApiResponse(2, 3, "title", "summary"),
			new EpisodeApiResponse(2, 4, "title", "summary"),
		]);

		var actual = await _handler.GetRelatedEpisodes(2, 3);
		
		Assert.IsType<Ok<RelatedEpisodesApiResponse>>(actual);
		var actualValue = (actual as Ok<RelatedEpisodesApiResponse>).Value;

		Assert.Equal(expected.RelatedEpisodes, actualValue.RelatedEpisodes);
	}
}