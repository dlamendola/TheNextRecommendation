using Api.Search;

namespace Api;

public static class Routes
{
	public static void MapRoutes(this WebApplication app)
	{
		var searchHandler = app.Services.GetRequiredService<SearchHandler>();
		
		app.MapPost("/api/search", searchHandler.Search);
		app.MapHealthChecks("/health");
	}
}