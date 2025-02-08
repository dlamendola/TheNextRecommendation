using Api;
using Api.Search;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EpisodeSearchService>();
builder.Services.AddSingleton<SearchHandler>();
builder.Services.AddSingleton<HealthCheckHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapRoutes();

app.Run();