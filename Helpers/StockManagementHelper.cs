using Assignment.Task.Data;
using Assignment.Task.Models;
using Dapper;
using System.Data;
using NLog;

namespace Assignment.Task.Helpers
{
	public class StockManagementHelper
	{
		private readonly DataContextDapper _dapper;
		private readonly Logger _logger = LogManager.GetLogger(nameof(StockManagementHelper));

		public StockManagementHelper(IConfiguration Config)
		{
			_dapper = new DataContextDapper(Config);
		}

		public List<StockTransaction> GetPreviousTransactions(bool Top)
		{
			string sql = "SELECT *  FROM [StockManagementDb].[dbo].[StockTransaction]";
			_logger.Info("GetPreviousTransactions called (Top={0})", Top);

			List<StockTransaction> stockTransactions = _dapper.LoadData<StockTransaction>(sql).ToList();
			return stockTransactions;
		}


		public bool InsertStockTranasction(StockTransaction stockTransaction)
		{
			string sql = @"
			EXEC [dbo].sp_StockTransaction_Insert
				    @MaterialId = @MaterialIdParam " +
				"  ,@TransactionType =  @TransactionTypeParam" +
				"  ,@Quantity =  @QuantityParam";

			DynamicParameters sqlParams = new DynamicParameters();
			sqlParams.Add("@MaterialIdParam", stockTransaction.MaterialId, DbType.Int64);
			sqlParams.Add("@TransactionTypeParam", stockTransaction.TransactionType == 1? true:false , DbType.Boolean);
			sqlParams.Add("@QuantityParam", stockTransaction.Quantity, DbType.Decimal);


			_logger.Info("InsertStockTransaction for MaterialId {0} Quantity {1}", stockTransaction.MaterialId, stockTransaction.Quantity);
			return _dapper.ExecuteQueryWithParameter(sql, sqlParams);

		}


		public bool DeleteStockTransaction(int Id)
		{
			string sql = @"
			EXEC [dbo].sp_StockTransaction_Delete
				    @Id = @TransactionIdParam ";

			DynamicParameters sqlParams = new DynamicParameters();
			sqlParams.Add("@TransactionIdParam", Id, DbType.Int64);
			_logger.Info("DeleteStockTransaction {0}", Id);
			return _dapper.ExecuteQueryWithParameter(sql, sqlParams);
		}


		public int GetStockInTransactions()
		{
			string sql = "SELECT * FROM [StockManagementDb].[dbo].[StockTransaction] where [TransactionType] = 1";
			_logger.Info("GetStockInTransactions called");
			List<StockTransaction> stockTransactions = _dapper.LoadData<StockTransaction>(sql).ToList();
			return stockTransactions.Count;
		}

		public int GetStockOutTransactions()
		{
			string sql = "SELECT * FROM [StockManagementDb].[dbo].[StockTransaction] where [TransactionType] = 0";
			_logger.Info("GetStockOutTransactions called");
			List<StockTransaction> stockTransactions = _dapper.LoadData<StockTransaction>(sql).ToList();
			return stockTransactions.Count;
		}
	}
}
