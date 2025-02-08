using Api.Search.Models;
using Shared;

namespace Api.Search;

public static class Mapper
{
	public static SearchApiResponse ToEpisodeApiResponse(this List<Episode> episodes)
	{
		return new SearchApiResponse(
			episodes.Select(x =>
					new EpisodeApiResponse(x.Season, x.EpisodeInSeason, x.Title, x.Summary))
				.ToList());
	}
}