using System.Data;
using System.Data.Common;
using Dapper;
using Pgvector;

namespace Shared.Db;

public class EpisodeStore(DbDataSource dataSource)
{
	public async Task<EpisodeRow?> GetById(int id)
	{
		using var conn = await GetConnection();

		var result = await conn.QuerySingleOrDefaultAsync<EpisodeRow>(
			"""
			select 
				id,
				season_number as seasonNumber, 
				episode_number as episodeNumber, 
				title, 
				summary, 
				synopsis, 
				synopsis_embedding as synopsisEmbedding 
			from 
				Episodes 
			where 
				Id = @id
			""", new { id });

		return result;
	}

	public virtual async Task<int> Save(EpisodeRow episode)
	{
		using var conn = await GetConnection();

		var insertedId = await conn.QuerySingleAsync<int>(
			"""
			insert into Episodes (season_number, episode_number, title, summary, synopsis, synopsis_embedding)
			values (@seasonNumber, @episodeNumber, @title, @summary, @synopsis, @synopsisEmbedding)
			returning id;
			""", episode);

		return insertedId;
	}

	public virtual async Task<List<EpisodeRow>> GetNearestEpisodes(Vector searchVector, int numResults)
	{
		using var conn = await GetConnection();
		
		var results = await conn.QueryAsync<EpisodeRow>(
			"""
				select 
					id,
					season_number as seasonNumber, 
					episode_number as episodeNumber, 
					title, 
					summary, 
					synopsis, 
					synopsis_embedding as synopsisEmbedding 
				from 
					Episodes 
				order by 
					synopsis_embedding <=> @searchVector
				limit @numResults
				""", new { searchVector, numResults});

		return results.ToList();
	}

	public virtual async Task<EpisodeRow?> GetBySeasonAndEpisodeNumber(int seasonNumber, int episodeNumber)
	{
		using var conn = await GetConnection();
		
		var result = await conn.QueryAsync<EpisodeRow>(
			"""
			select
				id,
				season_number as seasonNumber,
				episode_number as episodeNumber,
				title,
				summary,
				synopsis,
				synopsis_embedding as synopsisEmbedding
			from
				Episodes
			where
				season_number = @seasonNumber
				and episode_number = @episodeNumber
			""", new { seasonNumber, episodeNumber });

		return result.SingleOrDefault();
	}

	public virtual async Task<List<EpisodeRow>> GetRelatedEpisodes(int seasonNumber, int episodeNumber)
	{
		using var conn = await GetConnection();

		var result = await conn.QueryAsync<EpisodeRow>(
			"""
			select
				e.id,
				e.season_number as seasonNumber,
				e.episode_number as episodeNumber,
				e.title,
				e.summary,
				e.synopsis,
				e.synopsis_embedding as synopsisEmbedding
			from
				Episodes e,
				(select synopsis_embedding from Episodes where season_number = @seasonNumber and episode_number = @episodeNumber limit 1) target
			where
				not (e.season_number = @seasonNumber and e.episode_number = @episodeNumber)
			order by 
				e.synopsis_embedding <=> target.synopsis_embedding
			limit @numResults
			""", new { seasonNumber, episodeNumber, numResults = 5 });

		return result.ToList();
	}
	
	private async Task<IDbConnection> GetConnection()
	{
		return await dataSource.OpenConnectionAsync();
	}
}