using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Assignment.Task.Data
{
	public class DataContextDapper
	{
		private readonly IConfiguration _config;

		public DataContextDapper(IConfiguration config)
		{
			_config = config;
		}

		public IEnumerable<T> LoadData <T>(string sql)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Query<T>(sql);

			 
		}

		public T? LoadDataSingle<T>(string sql)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.QueryFirstOrDefault<T>(sql);

		}

		public int ExecuteSql(string sql)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Execute(sql);

			
		}

		public bool ExecuteQueryWithParameter(string sql, DynamicParameters sqlParameters)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Execute(sql,sqlParameters)>0;
		}


		public IEnumerable<T> LoadDataWithParams<T>(string sql , DynamicParameters parameters)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Query<T>(sql,parameters);


		}

		public T? LoadDataSingleWithParams<T>(string sql, DynamicParameters parameters)
		{
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.QueryFirstOrDefault<T>(sql,parameters);

		}
	}
}
