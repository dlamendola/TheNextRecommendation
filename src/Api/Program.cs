using System.ClientModel;
using System.ClientModel.Primitives;
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

await using var dataSource = PostgresVectorUtils.BuildDataSource(dbConn);

var embeddingClient = new EmbeddingClient(
	"text-embedding-3-small", 
	new ApiKeyCredential(openaiApiKey), 
	new OpenAIClientOptions
	{
		NetworkTimeout = TimeSpan.FromSeconds(5),
		RetryPolicy = new ClientRetryPolicy(0),
	});

builder.Services.AddSingleton(embeddingClient); 
builder.Services.AddScoped<EmbeddingGenerator>();
builder.Services.AddSingleton(dataSource);
builder.Services.AddScoped<EpisodeStore>();
builder.Services.AddScoped<EpisodeSearchService>();
builder.Services.AddScoped<SearchHandler>();
builder.Services.AddScoped<EpisodeHandler>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRoutes();

app.Run();