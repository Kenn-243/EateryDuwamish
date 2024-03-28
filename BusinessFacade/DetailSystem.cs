using System;
using System.Collections.Generic;
using Common.Data;
using DataAccess;
using BusinessRule;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class DetailSystem
    {
        public List<DetailData> GetDetailListByRecipeID(int id)
        {
            try
            {
                return new DetailDB().GetDetailListByRecipeID(id);
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
                return new DetailDB().GetDetailByID(detailID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateDetail(DetailData detail)
        {
            try
            {
                return new DetailRule().InsertUpdateDetail(detail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDetails(IEnumerable<int> detailIDs)
        {
            try
            {
                return new DetailRule().DeleteDetails(detailIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
