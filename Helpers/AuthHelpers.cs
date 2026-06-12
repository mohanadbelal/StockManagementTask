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

namespace Assignment.Task.Helpers
{
	public class AuthHelpers
	{
		private readonly IConfiguration _config;

		private readonly DataContextDapper _dapper;

		public AuthHelpers(IConfiguration config)
		{
			_config = config;
			_dapper = new DataContextDapper(config);
		}

		public byte[] GetPasswordHash(string password)
		{
			string PasswordKey = _config.GetSection("AppSettings:PasswordKey").Value;
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

			string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

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

			return tokenHandler.WriteToken(token);

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

            return _dapper.ExecuteQueryWithParameter(sql, sqlParams);
        }

        public bool LoginUser(string username, byte[] PasswordHash, out User user)
        {
            string sql = "select * from [StockManagementDb].[dbo].[User] where Username = @UsernameParam and PasswordHash = @PasswordHashParam";
            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UsernameParam", username, DbType.String);
            sqlParams.Add("@PasswordHashParam", PasswordHash, DbType.Binary);

            User currUser = _dapper.LoadDataSingleWithParams<User>(sql, sqlParams);

            user = null;

            if (currUser == null)
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
