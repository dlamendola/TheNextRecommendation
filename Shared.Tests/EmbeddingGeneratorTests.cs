namespace Shared.Tests;

public class EmbeddingGeneratorTests
{
	[Fact]
	public void NullClient_ThrowsArgumentNullException()
	{
		Assert.Throws<ArgumentNullException>(() => new EmbeddingGenerator(null!));
	}
}