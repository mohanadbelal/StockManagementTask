using Dapper;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;

namespace Assignment.Task.Data
{
	public class DataContextDapper
	{
		private readonly IConfiguration _config;
		private readonly Logger _logger = LogManager.GetLogger(nameof(DataContextDapper));

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
			_logger.Debug("LoadDataSingle SQL: {0}", sql);
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.QueryFirstOrDefault<T>(sql);

		}

		public int ExecuteSql(string sql)
		{
			_logger.Debug("ExecuteSql SQL: {0}", sql);
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Execute(sql);

			
		}

		public bool ExecuteQueryWithParameter(string sql, DynamicParameters sqlParameters)
		{
			_logger.Debug("ExecuteQueryWithParameter SQL: {0} Params: {1}", sql, sqlParameters);
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Execute(sql,sqlParameters)>0;
		}


		public IEnumerable<T> LoadDataWithParams<T>(string sql , DynamicParameters parameters)
		{

			_logger.Debug("LoadDataWithParams SQL: {0} Params: {1}", sql, parameters);
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.Query<T>(sql,parameters);


		}

		public T? LoadDataSingleWithParams<T>(string sql, DynamicParameters parameters)
		{
			_logger.Debug("LoadDataSingleWithParams SQL: {0} Params: {1}", sql, parameters);
			IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DBConnection"));

			return dbConnection.QueryFirstOrDefault<T>(sql,parameters);

		}
	}
}
