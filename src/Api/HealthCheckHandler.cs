namespace Api;

public class HealthCheckHandler
{
	public IResult HealthCheck()
	{
		return Results.Ok();
	}
}
