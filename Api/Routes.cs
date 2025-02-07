namespace Api;

public static class Routes
{
	public static void MapRoutes(this WebApplication app)
	{
		var handler = app.Services.GetRequiredService<Handler>();
		app.MapGet("/health", handler.HealthCheck);
		app.MapPost("/search", handler.Search);
	}
}