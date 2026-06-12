using Assignment.Task.Data;
using Assignment.Task.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Assignment.Task.Helpers
{
	public class SqlHelper
	{
		private readonly DataContextDapper _dapper;

		public SqlHelper(IConfiguration Config)
		{
			
			_dapper = new DataContextDapper(Config);
		}

		public bool RegisterUser(User user)
		{
			string sql = @"
			EXEC [dbo].[CreateUser]
				    @Username = @UsernameParam " +
				"  ,@PasswordHash =  @PasswordHashParam" +
				"  ,@Role =     @RoleParam ";

			DynamicParameters sqlParams = new DynamicParameters();
			sqlParams.Add("@UsernameParam", user.Username, DbType.String);
			sqlParams.Add("@PasswordHashParam", user.PasswordHash, DbType.Binary);
			sqlParams.Add("@RoleParam", user.Role, DbType.String);

			return _dapper.ExecuteQueryWithParameter(sql, sqlParams);
		}

		public bool LoginUser(string username , byte[] PasswordHash , out User user)
		{
            string sql = "select * from [StockManagementDb].[dbo].[User] where Username = @UsernameParam and PasswordHash = @PasswordHashParam";
            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UsernameParam", username, DbType.String);
            sqlParams.Add("@PasswordHashParam", PasswordHash, DbType.Binary);

			User currUser = _dapper.LoadDataSingleWithParams<User>(sql, sqlParams);

			user = null;

			if(currUser == null)
			{
				return false;
			}

            for (int i = 0; i < PasswordHash.Length; i++)
            {
				if (PasswordHash[i] != currUser.PasswordHash[i])
					return false;
            }

			user = currUser;
			return true;

        }


    }
}
