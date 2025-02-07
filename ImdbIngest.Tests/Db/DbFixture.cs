using System.Data.Common;
using Dapper;
using ImdbIngest.Db;
using Npgsql;
using Testcontainers.PostgreSql;

namespace ImdbIngest.Tests.Db;

public class DbFixture : IAsyncLifetime
{
	private PostgreSqlContainer _postgres;

	public DbDataSource DataSource { get; private set; }

	public async Task InitializeAsync()
	{
		_postgres = new PostgreSqlBuilder()
			.WithImage("pgvector/pgvector:pg17")
			.Build();

		await _postgres.StartAsync();
		await CreateTable();

		DataSource = PostgresVectorUtils.BuildDataSource(_postgres.GetConnectionString());
	}

	public async Task DisposeAsync()
	{
		await DataSource.DisposeAsync();
		await _postgres.DisposeAsync();
	}

	private async Task CreateTable()
	{
		await using var conn = new NpgsqlConnection(_postgres.GetConnectionString());

		await conn.ExecuteAsync("CREATE EXTENSION IF NOT EXISTS vector;");

		await conn.ExecuteAsync(
			"""
			CREATE TABLE episodes (
			   id SERIAL PRIMARY KEY,
			   season_number INT NOT NULL,
			   episode_number INT NOT NULL,
			   title VARCHAR(255) NOT NULL,
			   summary TEXT NOT NULL,
			   synopsis TEXT NOT NULL,
			   synopsis_embedding vector(1536) NOT NULL,
			   UNIQUE(season_number, episode_number)
			);
			""");
	}
}