using Pgvector;
using Shared;
using Shared.Db;

namespace Api.Search;

public class EpisodeSearchService(EmbeddingGenerator embeddingGenerator, EpisodeStore store)
{
	public virtual async Task<List<Episode>> Search(string searchText, int numResults = 5)
	{
		var embedding = await embeddingGenerator.Generate(searchText);
		
		var semanticallySimilarEpisodes = await store.GetNearestEpisodes(new Vector(embedding), numResults);

		return semanticallySimilarEpisodes.ToEpisodes();
	}
}