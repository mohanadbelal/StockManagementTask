using Assignment.Task.Data;
using Assignment.Task.Dtos;
using Assignment.Task.Models;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NLog;

namespace Assignment.Task.Helpers
{
	public class AuthHelpers
	{
		private readonly IConfiguration _config;

		private readonly DataContextDapper _dapper;

		private readonly Logger _logger = LogManager.GetLogger(nameof(AuthHelpers));

		public AuthHelpers(IConfiguration config)
		{
			_config = config;
			_dapper = new DataContextDapper(config);
		}

		public byte[] GetPasswordHash(string password)
		{
			string PasswordKey = _config.GetSection("PasswordKey").Value;
			byte[] passwordHash = KeyDerivation.Pbkdf2(
				password: password,
				salt: Encoding.ASCII.GetBytes(PasswordKey),
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 256 / 8
			);

			return passwordHash;
		}


		public string CreateToken(int userId,string userRole)
		{
			Claim[] claims = new Claim[] {
				new Claim("userId", userId.ToString()),
				new Claim(ClaimTypes.Role, userRole)
			};

			string? tokenKeyString = _config.GetSection("TokenKey").Value;

			SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(
						tokenKeyString != null ? tokenKeyString : ""
					)
				);

			SigningCredentials credentials = new SigningCredentials(
					tokenKey,
					SecurityAlgorithms.HmacSha512Signature
				);

			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = credentials,
				Expires = DateTime.Now.AddMinutes(15)
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken token = tokenHandler.CreateToken(descriptor);

			string written = tokenHandler.WriteToken(token);
			_logger.Info("Created JWT for user {0} with role {1}", userId, userRole);

			return written;

		}


        public bool RegisterUser(User user)
        {
            string sql = @"
			EXEC [dbo].sp_User_Create
				    @Username = @UsernameParam " +
                "  ,@PasswordHash =  @PasswordHashParam" +
                "  ,@Role =     @RoleParam ";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UsernameParam", user.Username, DbType.String);
            sqlParams.Add("@PasswordHashParam", user.PasswordHash, DbType.Binary);
            sqlParams.Add("@RoleParam", user.Role, DbType.String);

			_logger.Info("RegisterUser: attempting to register user {0}", user.Username);
			bool result = _dapper.ExecuteQueryWithParameter(sql, sqlParams);
			_logger.Info("RegisterUser: result {0} for user {1}", result, user.Username);
			return result;
        }

        public bool LoginUser(string username, byte[] PasswordHash, out User user)
        {
            string sql = "select * from [StockManagementDb].[dbo].[User] where Username = @UsernameParam and PasswordHash = @PasswordHashParam";
            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UsernameParam", username, DbType.String);
            sqlParams.Add("@PasswordHashParam", PasswordHash, DbType.Binary);

			_logger.Info("LoginUser: attempting login for {0}", username);
			User currUser = _dapper.LoadDataSingleWithParams<User>(sql, sqlParams);

			user = null;

			if (currUser == null)
			{
			_logger.Warn("LoginUser: user not found {0}", username);
				return false;
			}

			for (int i = 0; i < PasswordHash.Length; i++)
			{
				if (PasswordHash[i] != currUser.PasswordHash[i])
				{
					_logger.Warn("LoginUser: invalid password for {0}", username);
					return false;
				}
			}

			user = currUser;
			_logger.Info("LoginUser: successful login for {0}", username);
			return true;

        }

    }
}
