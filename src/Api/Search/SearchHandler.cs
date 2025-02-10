using Api.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Search;

public class SearchHandler(EpisodeSearchService episodeSearchService)
{
	public async Task<IResult> Search([FromBody] SearchRequest request)
	{
		if (string.IsNullOrEmpty(request.SearchText) || request.SearchText.Length > 50)
		{
			return Results.BadRequest();
		}

		var result = await episodeSearchService.Search(request.SearchText);

		var response = result.ToEpisodeApiResponse();

		return Results.Ok(response);
	}
}