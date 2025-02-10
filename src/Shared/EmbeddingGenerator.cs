using OpenAI.Embeddings;

namespace Shared;

public class EmbeddingGenerator(EmbeddingClient client)
{
	private readonly EmbeddingClient _client = client ?? throw new ArgumentNullException(nameof(client));

	public virtual async Task<ReadOnlyMemory<float>> Generate(string s)
	{
		var embedding = await _client.GenerateEmbeddingAsync(s);

		return embedding.Value.ToFloats();
	}
}