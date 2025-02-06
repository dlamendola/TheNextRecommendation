using Microsoft.Extensions.Logging;
using OpenAI.Embeddings;
using Scraper;
using Scraper.Db;

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

var scraper = new EpisodeScraper("https://imdb.com/title/");

var embeddingGenerator = new EmbeddingGenerator(new EmbeddingClient("text-embedding-3-small", openaiApiKey));

await using var dataSource = PostgresVectorUtils.BuildDataSource(dbConn);
var episodeStore = new EpisodeStore(dataSource);

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<DataLoader>();

var dataLoader = new DataLoader(scraper, episodeStore, embeddingGenerator, logger);
await dataLoader.LoadDataAsync();

Console.WriteLine("Loading complete!");