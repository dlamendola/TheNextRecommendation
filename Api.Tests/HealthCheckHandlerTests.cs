using Api;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiTests;

public class HealthCheckHandlerTests
{
	private readonly HealthCheckHandler _healthCheckHandler = new ();

	[Fact]
	public void HealthCheck_Ok()
	{
		var actual = _healthCheckHandler.HealthCheck();

		Assert.IsType<Ok>(actual);
	}
}