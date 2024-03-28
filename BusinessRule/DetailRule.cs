using System;
using Common.Data;
using DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SystemFramework;

namespace BusinessRule
{
    public class DetailRule
    {
        public int InsertUpdateDetail(DetailData detail)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlConn = new SqlConnection(SystemConfigurations.EateryConnectionString);
                SqlConn.Open();
                SqlTran = SqlConn.BeginTransaction();
                int rowsAffected = new DetailDB().InsertUpdateDetail(detail, SqlTran);
                SqlTran.Commit();
                SqlConn.Close();

                return rowsAffected;
            }

            catch (Exception ex)
            {
                SqlTran.Rollback();
                SqlConn.Close();
                throw ex;
            }
        }

        public int DeleteDetails(IEnumerable<int> detailIDs)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlConn = new SqlConnection(SystemConfigurations.EateryConnectionString);
                SqlConn.Open();
                SqlTran = SqlConn.BeginTransaction();
                int rowsAffected = new DetailDB().DeleteDetails(String.Join(",", detailIDs), SqlTran);
                SqlTran.Commit();
                SqlConn.Close();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                SqlTran.Rollback();
                SqlConn.Close();
                throw ex;
            }
        }
    }
}
