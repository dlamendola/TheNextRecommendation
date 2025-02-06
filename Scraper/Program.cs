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

var embeddingGenerator = new EmbeddingGenerator(new EmbeddingClient("text-embedding-3-small", openaiApiKey));

await using var dataSource = PostgresVectorUtils.BuildDataSource(dbConn);
var episodeStore = new EpisodeStore(dataSource);

var dataLoader = new DataLoader(episodeStore, embeddingGenerator);
await dataLoader.LoadDataAsync();