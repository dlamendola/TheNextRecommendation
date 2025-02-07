using System.Data.Common;
using Dapper;
using Npgsql;
using Pgvector.Dapper;

namespace ImdbIngest.Db;

public static class PostgresVectorUtils
{
	public static DbDataSource BuildDataSource(string connectionString)
	{
		SqlMapper.AddTypeHandler(new VectorTypeHandler());
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
		dataSourceBuilder.UseVector();

		return dataSourceBuilder.Build();
	}
}