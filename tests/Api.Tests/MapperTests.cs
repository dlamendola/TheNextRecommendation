using Api.Search;
using Api.Search.Models;
using Shared;
using Shared.Db;

namespace ApiTests;

public class MapperTests
{
	[Fact]
	public void ToEpisodeApiResponse()
	{
		var episodes = new List<Episode> { new Episode(3, 5, "title", null, "summary", "synopsis") };
		var expected = new SearchApiResponse(new List<EpisodeApiResponse>
		{
			new (3, 5, "title", "summary")
		});

		var actual = episodes.ToEpisodeApiResponse();
		
		Assert.Equal(expected.Episodes, actual.Episodes);
	}

	[Fact]
	public void ToEpisodes()
	{
		var rows = new List<EpisodeRow> { new (1, 3, 5, "title", "summary", "synopsis", null) };
		var expected = new List<Episode> { new (3, 5, "title", null, "summary", "synopsis") };
		
		var actual = rows.ToEpisodes();
		
		Assert.Equal(expected, actual);
	}
}