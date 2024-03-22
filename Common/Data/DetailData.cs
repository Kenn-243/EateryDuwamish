using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DetailData
    {
        private int _detailId;
        public int DetailID
        {
            get { return _detailId; }
            set { _detailId = value; }
        }

        private int _recipeId;
        public int RecipeID
        {
            get { return _recipeId;}
            set { _recipeId = value; }
        }

        private string _detailIngredient;
        public string DetailIngredient
        {
            get { return _detailIngredient; }
            set { _detailIngredient = value; }
        }

        private int _detailQuantity;
        public int DetailQuantity
        {
            get { return _detailQuantity; }
            set { _detailQuantity = value; }
        }

        private string _detailUnit;
        public string DetailUnit
        {
            get { return _detailUnit; }
            set { _detailUnit = value; }
        }
    }
}
