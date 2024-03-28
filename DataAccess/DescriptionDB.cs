using System;
using System.Collections.Generic;
using Common.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SystemFramework;
using System.Data;

namespace DataAccess
{
    public class DescriptionDB
    {
        public DescriptionData GetDescriptionByRecipeID(int id)
        {
            try
            {
                string SpName = "dbo.Description_Get";
                DescriptionData description = new DescriptionData();
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            if (Convert.ToInt32(Reader["RecipeID"]) == id)
                            {
                                description.DescriptionID = Convert.ToInt32(Reader["DescriptionID"]);
                                description.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                                description.DescriptionRecipe = Convert.ToString(Reader["DescriptionRecipe"]);
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return description;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DescriptionData GetDescriptionByID(int descriptionID)
        {
            try
            {
                string SpName = "dbo.Description_GetByID";
                DescriptionData description = null;
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@DescriptionID", descriptionID));
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            Reader.Read();
                            description = new DescriptionData();
                            description.DescriptionID = Convert.ToInt32(Reader["DescriptionID"]);
                            description.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                            description.DescriptionRecipe = Convert.ToString(Reader["DescriptionRecipe"]);
                        }
                    }
                    SqlConn.Close();
                }
                return description;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        public int InsertUpdateDescription(DescriptionData description, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Description_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter DescriptionId = new SqlParameter("@DescriptionID", description.DescriptionID);
                DescriptionId.Direction = ParameterDirection.InputOutput;
                SqlCmd.Parameters.Add(DescriptionId);

                SqlCmd.Parameters.Add(new SqlParameter("@RecipeID", description.RecipeID));
                SqlCmd.Parameters.Add(new SqlParameter("@DescriptionRecipe", description.DescriptionRecipe));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDescriptions(string descriptionIDs, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Description_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@DescriptionIDs", descriptionIDs));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
