using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.Data;
using BusinessFacade;
using Common.Enum;

namespace EateryDuwamish
{
    public partial class Recipe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowNotificationIfExists();
                LoadRecipeTable();
            }
        }


        #region FORM MANAGEMENT
        private void FillForm(RecipeData recipe)
        {
            hdfRecipeId.Value = recipe.RecipeID.ToString();
            hdfDishId.Value = recipe.DishID.ToString();
            txtRecipeName.Text = recipe.RecipeName;
        }
        private void ResetForm()
        {
            hdfRecipeId.Value = String.Empty;
            txtRecipeName.Text = String.Empty;
        }
        private RecipeData GetFormData()
        {
            RecipeData recipe = new RecipeData();
            recipe.RecipeID = String.IsNullOrEmpty(hdfRecipeId.Value) ? 0 : Convert.ToInt32(hdfRecipeId.Value);
            recipe.DishID = int.Parse(Request["ID"]);
            recipe.RecipeName = txtRecipeName.Text.ToString();
            return recipe;
        }
        #endregion




        #region DATA TABLE MANAGEMENT
        private void LoadRecipeTable()
        {
            try
            {
                List<RecipeData> ListRecipe = new RecipeSystem().GetRecipeListByDishID(Convert.ToInt32(Request["ID"]));
                rptRecipe.DataSource = ListRecipe;
                rptRecipe.DataBind();
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptRecipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipeData recipe = (RecipeData)e.Item.DataItem;

                LinkButton lbRecipeName = (LinkButton)e.Item.FindControl("lbRecipeName");

                lbRecipeName.Text = recipe.RecipeName;
                lbRecipeName.CommandArgument = recipe.RecipeID.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", recipe.RecipeID.ToString());
            }
        }
        protected void rptRecipe_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbRecipeName = (LinkButton)e.Item.FindControl("lbRecipeName");

                int recipeID = Convert.ToInt32(e.CommandArgument.ToString());
                RecipeData recipe = new RecipeSystem().GetRecipeByID(recipeID);
                FillForm(new RecipeData
                {
                    RecipeID = recipe.RecipeID,
                    DishID = recipe.DishID,
                    RecipeName = recipe.RecipeName
                });
                litFormType.Text = $"UBAH: {lbRecipeName.Text}";
                pnlFormRecipe.Visible = true;
                txtRecipeName.Focus();
            }
        }
        #endregion



        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeData recipe = GetFormData();
                int rowAffected = new RecipeSystem().InsertUpdateRecipe(recipe);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                string id = Request["ID"];
                Response.Redirect("Recipe.aspx?ID=" + id);
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormRecipe.Visible = true;
            txtRecipeName.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string id = Request["ID"];
                string strDeletedIDs = hdfDeletedRecipes.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new RecipeSystem().DeleteRecipes(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("Recipe.aspx?ID=" + id);
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifRecipe.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifRecipe.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }

        #endregion

        protected void lbDetailID_Command(object sender, CommandEventArgs e)
        {
            string RecipeID = e.CommandArgument.ToString();
            Response.Redirect($"Detail.aspx?ID={RecipeID}");
        }
    }
}