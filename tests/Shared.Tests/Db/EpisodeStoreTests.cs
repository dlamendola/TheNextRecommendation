using Pgvector;
using Shared.Db;

namespace Shared.Tests.Db;

public class EpisodeStoreTests : IClassFixture<DbFixture>
{
	private readonly EpisodeStore _store;

	public EpisodeStoreTests(DbFixture fixture)
	{
		_store = new EpisodeStore(fixture.DataSource);
	}

	[Fact]
	public async Task GetEpisode_DoesNotExist()
	{
		var episode = await _store.GetById(-1);

		Assert.Null(episode);
	}

	[Fact]
	public async Task SaveAndGetEpisode()
	{
		var episode = new EpisodeRow(
			null,
			3,
			5,
			"Darmok",
			"Crazy things happen",
			"A long description of what happens in the episode",
			CreateVector()
		);
		var id = await _store.Save(episode);
		var expected = episode with { Id = id };

		var result = await _store.GetById(id);

		Assert.Equal(expected, result);
	}

	[Fact]
	public async Task SearchBySemanticSimilarity()
	{
		var episode = new EpisodeRow(
			null,
			1,
			1,
			"Darmok",
			"Crazy things happen",
			"A long description of what happens in the episode",
			CreateVector()
		);
		var id = await _store.Save(episode);
		var vector = CreateVector();
		
		var result = await _store.GetNearestEpisodes(vector, 1);

		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetBySeasonAndEpisodeNumber_DoesNotExist()
	{
		var result = await _store.GetBySeasonAndEpisodeNumber(-1, -1);

		Assert.Null(result);
	}

	[Fact]
	public async Task GetBySeasonAndEpisodeNumber()
	{
		var episode = new EpisodeRow(
			null,
			8,
			9,
			"Darmok",
			"Crazy things happen",
			"A long description of what happens in the episode",
			CreateVector()
		);
		var id = await _store.Save(episode);
		var expected = episode with { Id = id };

		var result = await _store.GetBySeasonAndEpisodeNumber(8, 9);

		Assert.Equal(expected, result);
	}

	[Fact]
	public async Task GetRelatedEpisodes_EpisodeNotFound()
	{
		var actual = await _store.GetRelatedEpisodes(-1, -1);

		Assert.Empty(actual);
	}

	[Fact]
	public async Task GetRelatedEpisodes()
	{
		var season = 2;
		await InsertEpisodes(season, 10);
		
		var actual = await _store.GetRelatedEpisodes(season, 1);
		
		Assert.Equal(5, actual.Count);
		Assert.DoesNotContain(actual, x => x.SeasonNumber == season && x.EpisodeNumber == 1);
	}

	private async Task InsertEpisodes(int season, int numEpisodes)
	{
		for (int i = 0; i < numEpisodes; i++)
		{
			await _store.Save(new EpisodeRow(null, 2, i, "title", "summary", "synopsis", CreateVector()));
		}
	}

	private static Vector CreateVector()
	{
		return new Vector(Enumerable.Range(0, 1536).Select(i => (float)i).ToArray());
	}
}