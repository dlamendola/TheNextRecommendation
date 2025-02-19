using Api.Search.Models;
using Shared;
using Shared.Db;

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

	public static List<Episode> ToEpisodes(this List<EpisodeRow> rows)
	{
		return rows
			.Select(x => new Episode(
				Season: x.SeasonNumber,
				EpisodeInSeason: x.EpisodeNumber,
				Title: x.Title,
				NextEpisodeId: null,
				Summary: x.Summary,
				Synopsis: x.Synopsis))
			.ToList();
	}
}