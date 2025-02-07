using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Api;

public class Handler(EpisodeSearchService episodeSearchService, ILogger<Handler> logger)
{
	public IResult HealthCheck()
	{
		return Results.Ok();
	}
	
	public IResult Search([FromBody] SearchRequest request)
	{
		if (string.IsNullOrEmpty(request.SearchText) || request.SearchText.Length > 50)
		{
			return Results.BadRequest();
		}

		return Results.Ok(request.SearchText);
	}
}

public class EpisodeSearchService
{
	public Task<List<Episode>> Search(string searchText)
	{
		return Task.FromResult(new List<Episode>());
	}
}

public record SearchRequest(string SearchText);

public record EpisodeApiResponse(int Season, int Episode, string Title, string Summary);

public record SearchApiResponse(List<EpisodeApiResponse> Episodes);