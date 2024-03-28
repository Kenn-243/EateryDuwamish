using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class RecipeData
    {
        private int _recipeId;
        public int RecipeID
        {
            get { return _recipeId; }
            set { _recipeId = value; }
        }

        private int _dishId;
        public int DishID
        {
            get { return _dishId; }
            set { _dishId = value; }
        }

        private string _recipeName;
        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }
    }
}
