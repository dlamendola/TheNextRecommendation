using ImdbIngest.Db;
using Pgvector;
using Shared.Db;

namespace ImdbIngest.Tests.Db;

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

	private static Vector CreateVector()
	{
		return new Vector(Enumerable.Range(0, 1536).Select(i => (float)i).ToArray());
	}
}