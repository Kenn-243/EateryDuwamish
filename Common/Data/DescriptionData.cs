using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DescriptionData
    {
        private int _descriptionId;
        public int DescriptionID
        {
            get { return _descriptionId;}
            set { _descriptionId = value; }
        }

        private int _recipeId;
        public int RecipeID
        {
            get { return _recipeId; }
            set { _recipeId = value; }
        }

        private string _descriptionRecipe;
        public string DescriptionRecipe
        {
            get { return _descriptionRecipe; }
            set { _descriptionRecipe = value; }
        }
    }
}
