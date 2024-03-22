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
    public class DetailDB
    {
        public List<DetailData> GetDetailListByRecipeID(int id)
        {
            try
            {
                string SpName = "dbo.Detail_Get";
                List<DetailData> ListDetail = new List<DetailData>();
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                DetailData detail = new DetailData();
                                if(Convert.ToInt32(Reader["RecipeID"]) == id){
                                    detail.DetailID = Convert.ToInt32(Reader["DetailID"]);
                                    detail.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                                    detail.DetailIngredient = Convert.ToString(Reader["DetailIngredient"]);
                                    detail.DetailQuantity = Convert.ToInt32(Reader["DetailQuantity"]);
                                    detail.DetailUnit = Convert.ToString(Reader["DetailUnit"]);
                                    ListDetail.Add(detail);
                                }
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return ListDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DetailData GetDetailByID(int detailID)
        {
            try
            {
                string SpName = "dbo.Detail_GetByID";
                DetailData detail = null;
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@DetailID", detailID));
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            Reader.Read();
                            detail = new DetailData();
                            detail.DetailID = Convert.ToInt32(Reader["DetailID"]);
                            detail.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                            detail.DetailIngredient = Convert.ToString(Reader["DetailIngredient"]);
                            detail.DetailQuantity = Convert.ToInt32(Reader["DetailQuantity"]);
                            detail.DetailUnit = Convert.ToString(Reader["DetailUnit"]);
                        }
                    }
                    SqlConn.Close();
                }
                return detail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateDetail(DetailData detail, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Detail_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter DetailId = new SqlParameter("@DetailID", detail.DetailID);
                DetailId.Direction = ParameterDirection.InputOutput;
                SqlCmd.Parameters.Add(DetailId);

                SqlCmd.Parameters.Add(new SqlParameter("@RecipeID", detail.RecipeID));
                SqlCmd.Parameters.Add(new SqlParameter("@DetailIngredient", detail.DetailIngredient));
                SqlCmd.Parameters.Add(new SqlParameter("@DetailQuantity", detail.DetailQuantity));
                SqlCmd.Parameters.Add(new SqlParameter("@DetailUnit", detail.DetailUnit));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteDetails(string detailIDs, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Detail_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@DetailIDs", detailIDs));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
