using System.ClientModel;
using Api;
using Api.Search;
using OpenAI;
using OpenAI.Embeddings;
using Shared;
using Shared.Db;

var builder = WebApplication.CreateBuilder(args);

var openaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrEmpty(openaiApiKey))
{
	Console.WriteLine("OPENAI_API_KEY environment variable is required");
	return;
}

var dbConn = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(dbConn))
{
	Console.WriteLine("POSTGRES_CONNECTION_STRING environment variable is required");
	return;
}

var embeddingClient = new EmbeddingClient(
	"text-embedding-3-small", 
	new ApiKeyCredential(openaiApiKey), 
	new OpenAIClientOptions
	{
		NetworkTimeout = TimeSpan.FromSeconds(2)
	});
var embeddingGenerator = new EmbeddingGenerator(embeddingClient);

await using var dataSource = PostgresVectorUtils.BuildDataSource(dbConn);

builder.Services.AddSingleton(embeddingGenerator);
builder.Services.AddSingleton(dataSource);
builder.Services.AddScoped<EpisodeStore>();

builder.Services.AddSingleton<EpisodeSearchService>();
builder.Services.AddSingleton<SearchHandler>();
builder.Services.AddSingleton<HealthCheckHandler>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapRoutes();

app.Run();