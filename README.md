# The Next Recommendation
This app generates [embeddings](https://platform.openai.com/docs/guides/embeddings) for each episode synopsis of _Star Trek: The Next Generation_ and allows you to search using natural language, returning the most relevant episodes based on semantic similarity to the search input.

There are two programs contained in this repo:
- `ImdbIngest` - responsible for scraping episode data from IMDB and generating embeddings using the OpenAI API and storing them in a Postgres db using the [pgvector](https://github.com/pgvector/pgvector) extension.
- `Api` - handles search requests and serves the website itself

## Running locally
Both projects use .NET 9 and require two environment variables:
- `OPENAI_API_KEY` to generate embeddings using the OpenAI API
- `POSTGRES_CONNECTION_STRING` for Postgres to store and query episodes. The Postgres DB requires the pgvector extension to be installed.

Run the ImdbIngest project first to scrape episode data, generate embeddings, and store them in the Postgres database.

After that you can run the Api project which will accept search input, generate an embedding for it, then return the most relevant results using cosine similarity.

## Technical details
- OpenAI API to generate embeddings using the `text-embedding-3-small` model
- pgvector to store embeddings
- pgvector similarity search using cosine similarity to find most relevant episodes
- Deployed to AWS using RDS and EC2 
- C# / .NET 9
- React / TypeScript / react-router
