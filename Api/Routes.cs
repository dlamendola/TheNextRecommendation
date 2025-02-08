using Api.Search;

namespace Api;

public static class Routes
{
	public static void MapRoutes(this WebApplication app)
	{
		var searchHandler = app.Services.GetRequiredService<SearchHandler>();
		var healthCheckHandler = app.Services.GetRequiredService<HealthCheckHandler>();
		
		app.MapGet("/health", healthCheckHandler.HealthCheck);
		app.MapPost("/search", searchHandler.Search);
	}
}