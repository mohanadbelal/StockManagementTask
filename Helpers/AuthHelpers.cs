using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Assignment.Task.Data;
using Assignment.Task.Dtos;
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

	}
}
