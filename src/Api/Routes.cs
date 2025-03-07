using Api.Search;

namespace Api;

public static class Routes
{
	public static void MapRoutes(this WebApplication app)
	{
		var searchHandler = app.Services.GetRequiredService<SearchHandler>();
		var episodeHandler = app.Services.GetRequiredService<EpisodeHandler>();
		
		app.MapPost("/api/search", searchHandler.Search);
		app.MapGet("/api/s/{seasonNumber:int}/e/{episodeNumber:int}", episodeHandler.GetBySeasonAndEpisodeNumber);
		app.MapGet("/api/s/{seasonNumber:int}/e/{episodeNumber:int}/related", episodeHandler.GetRelatedEpisodes);
		app.MapHealthChecks("/health");
	}
}