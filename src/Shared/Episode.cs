namespace Shared;

public record Episode(
	int Season,
	int EpisodeInSeason,
	string Title,
	string? NextEpisodeId,
	string Summary,
	string Synopsis
);