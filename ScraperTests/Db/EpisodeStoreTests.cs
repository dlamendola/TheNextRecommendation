using Pgvector;
using Scraper.Db;

namespace ScraperTests.Db;

public class EpisodeStoreTests : IClassFixture<DbFixture>
{
    private EpisodeStore _store;

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
            Id: null,
            SeasonNumber: 3,
            EpisodeNumber: 5,
            Title: "Darmok",
            Summary: "Crazy things happen",
            Synopsis: "A long description of what happens in the episode",
            SynopsisEmbedding: CreateVector()
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