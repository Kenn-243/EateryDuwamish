using System;
using DataAccess;
using Common.Data;
using BusinessRule;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class DescriptionSystem
    {
        public DescriptionData GetDescriptionByRecipeID(int id)
        {
            try
            {
                return new DescriptionDB().GetDescriptionByRecipeID(id);
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
                return new DescriptionDB().GetDescriptionByID(descriptionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateDescription(DescriptionData description)
        {
            try
            {
                return new DescriptionRule().InsertUpdateDescription(description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDescriptions(IEnumerable<int> descriptionIDs)
        {
            try
            {
                return new DescriptionRule().DeleteDescriptions(descriptionIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
