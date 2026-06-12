using Assignment.Task.Data;
using Assignment.Task.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Assignment.Task.Helpers
{
	public class MaterialHelper
	{
		private readonly DataContextDapper _dapper;

		public MaterialHelper(IConfiguration Config)
		{
			
			_dapper = new DataContextDapper(Config);
		}

		
		public List<Material> GetMaterials()
		{
			string  sql = "SELECT * FROM dbo.Material";
			List<Material> materials = _dapper.LoadData<Material>(sql).ToList();
			return materials;
		}

		public Material? GetMaterialById(int id)
		{
			string sql = "SELECT * FROM dbo.Material WHERE Id = @MaterialIdParam";
			DynamicParameters sqlParams = new DynamicParameters();
			sqlParams.Add("@MaterialIdParam", id, DbType.Int64);

			Material? material = _dapper.LoadDataSingleWithParams<Material>(sql, sqlParams);
			return material;
		}


		public bool Upsert(Material material)
		{
            string sql = @"
			EXEC [dbo].sp_Material_Upsert
				    @Name = @NameParam " +
                "  ,@Color =  @ColorParam" +
                "  ,@MinimumRequiredStock =  @MinimumRequiredStockParam" +
                "  ,@inMaterialId =  @MaterialIdParam" +
                "  ,@CurrentStock =     @CurrentStockParam ";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@NameParam",material.Name , DbType.String);
            sqlParams.Add("@ColorParam", material.Color, DbType.String);
            sqlParams.Add("@MinimumRequiredStockParam", material.MinimumRequiredStock, DbType.String);
            sqlParams.Add("@CurrentStockParam", material.CurrentStock, DbType.String);
            sqlParams.Add("@MaterialIdParam", material.Id, DbType.Int64);

            return _dapper.ExecuteQueryWithParameter(sql, sqlParams);
        }


		public bool DeleteMaterial(int Id)
		{
            string sql = @"
			EXEC [dbo].sp_Material_Delete
				    @Id = @MaterialIdParam ";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@MaterialIdParam", Id, DbType.Int64);

            return _dapper.ExecuteQueryWithParameter(sql, sqlParams);
        }



    }
}
