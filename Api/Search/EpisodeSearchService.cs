using Shared;

namespace Api.Search;

public class EpisodeSearchService
{
	public virtual Task<List<Episode>> Search(string searchText)
	{
		return Task.FromResult(new List<Episode>());
	}
}