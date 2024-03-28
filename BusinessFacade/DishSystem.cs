using BusinessRule;
using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class DishSystem
    {
        public List<DishData> GetDishList()
        {
            try
            {
                return new DishDB().GetDishList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DishData GetDishByID(int dishID)
        {
            try
            {
                return new DishDB().GetDishByID(dishID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateDish(DishData dish)
        {
            try
            {
                return new DishRule().InsertUpdateDish(dish);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDishes(IEnumerable<int> dishIDs)
        {
            try
            {
                foreach (int dishId in dishIDs)
                {
                    List<RecipeData> ListRecipe = new RecipeSystem().GetRecipeListByDishID(dishId);
                    List<int> ListRecipeID = new List<int>();
                    foreach (RecipeData recipe in ListRecipe)
                    {
                        ListRecipeID.Add(recipe.RecipeID);
                    }
                    IEnumerable<int> ListDetailDeletedID = ListRecipeID;
                    new RecipeSystem().DeleteRecipes(ListRecipeID);
                }
                return new DishRule().DeleteDishes(dishIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
