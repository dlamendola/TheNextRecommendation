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

