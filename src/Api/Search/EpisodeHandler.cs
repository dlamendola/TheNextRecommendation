using Shared.Db;

namespace Api.Search;

public class EpisodeHandler(EpisodeStore episodeStore)
{
	public async Task<IResult> GetBySeasonAndEpisodeNumber(int seasonNumber, int episodeNumber)
	{
		var episode = await episodeStore.GetBySeasonAndEpisodeNumber(seasonNumber, episodeNumber);
		if (episode is null)
		{
			return Results.NotFound();
		}

		return Results.Ok(episode.ToEpisodeApiResponse());
	}

	public async Task<IResult> GetRelatedEpisodes(int seasonNumber, int episodeNumber)
	{
		var relatedEpisodes = await episodeStore.GetRelatedEpisodes(seasonNumber, episodeNumber);
		if (relatedEpisodes.Count == 0)
		{
			return Results.NotFound();
		}

		return Results.Ok(relatedEpisodes.ToRelatedEpisodesApiResponse());
	}
}