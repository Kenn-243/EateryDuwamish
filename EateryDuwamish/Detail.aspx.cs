using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using BusinessFacade;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.Enum;

namespace EateryDuwamish
{
    public partial class Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowNotificationIfExists();
                LoadDetailTable();
                LoadDetailDescription();
            }
        }
        #region FORM MANAGEMENT
        private void FillForm(DetailData detail)
        {
            hdfDetailId.Value = detail.DetailID.ToString();
            hdfRecipeId.Value = detail.RecipeID.ToString();
            txtIngredient.Text = detail.DetailIngredient.ToString();
            txtQuantity.Text = detail.DetailQuantity.ToString();
            txtUnit.Text = detail.DetailUnit.ToString();
        }
        private void ResetForm()
        {
            hdfDetailId.Value = String.Empty;
            txtIngredient.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtUnit.Text = String.Empty;
        }
        private DetailData GetFormData()
        {
            DetailData detail = new DetailData();
            detail.DetailID = String.IsNullOrEmpty(hdfDetailId.Value) ? 0 : Convert.ToInt32(hdfDetailId.Value);
            detail.RecipeID = Convert.ToInt32((Request["ID"]));
            detail.DetailIngredient = txtIngredient.Text.ToString();
            detail.DetailQuantity = Convert.ToInt32(txtQuantity.Text);
            detail.DetailUnit = txtUnit.Text.ToString();
            return detail;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadDetailTable()
        {
            try
            {
                List<DetailData> ListDetail = new DetailSystem().GetDetailListByRecipeID(Convert.ToInt32(Request["ID"]));
                rptDetail.DataSource = ListDetail;
                rptDetail.DataBind();
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DetailData detail = (DetailData)e.Item.DataItem;

                LinkButton lbIngredient = (LinkButton)e.Item.FindControl("lbIngredient");
                Literal litQuantity = (Literal)e.Item.FindControl("litQuantity");
                Literal litUnit = (Literal)e.Item.FindControl("litUnit");

                lbIngredient.Text = detail.DetailIngredient;
                lbIngredient.CommandArgument = detail.DetailID.ToString();

                litQuantity.Text = detail.DetailQuantity.ToString();
                litUnit.Text = detail.DetailUnit.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", detail.DetailID.ToString());
            }
        }
        protected void rptDetail_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbIngredient = (LinkButton)e.Item.FindControl("lbIngredient");
                Literal litQuantity = (Literal)e.Item.FindControl("litQuantity");
                Literal litUnit = (Literal)e.Item.FindControl("litUnit");

                int detailID = Convert.ToInt32(e.CommandArgument.ToString());
                DetailData detail = new DetailSystem().GetDetailByID(detailID);
                FillForm(new DetailData
                {
                    DetailID = detail.DetailID,
                    RecipeID = detail.RecipeID,
                    DetailIngredient = detail.DetailIngredient,
                    DetailQuantity = detail.DetailQuantity,
                    DetailUnit = detail.DetailUnit
                });
                litFormType.Text = $"UBAH: {lbIngredient.Text}";
                pnlFormDetail.Visible = true;
                txtIngredient.Focus();
            }
        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DetailData detail = GetFormData();
                int rowAffected = new DetailSystem().InsertUpdateDetail(detail);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                string id = Request["ID"];
                Response.Redirect("Detail.aspx?ID=" + id);
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormDetail.Visible = true;
            txtIngredient.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedDetails.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new DetailSystem().DeleteDetails(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                string id = Request["ID"];
                Response.Redirect("Detail.aspx?ID=" + id);
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifDetail.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifDetail.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion

        #region Description
        private void LoadDetailDescription()
        {
            try
            {
                DescriptionData description = new DescriptionSystem().GetDescriptionByRecipeID(Convert.ToInt32(Request["ID"]));
                txtDescription.Text = description.DescriptionRecipe.ToString();
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }

        private DescriptionData GetDescription()
        {
            DescriptionData description = new DescriptionData();
            description.DescriptionID = String.IsNullOrEmpty(hdfDescriptionId.Value) ? 0 : Convert.ToInt32(hdfDescriptionId.Value);
            description.RecipeID = Convert.ToInt32((Request["ID"]));
            description.DescriptionRecipe = txtDescription.Text.ToString();
            return description;
        }

        protected void btnEditDescription_Click(object sender, EventArgs e)
        {
            txtDescription.ReadOnly = false;
            txtDescription.BackColor = System.Drawing.Color.White;
        }

        protected void btnSaveDescription_Click(object sender, EventArgs e)
        {
            txtDescription.ReadOnly = true;
            txtDescription.BackColor = System.Drawing.Color.FromArgb(215, 215, 215);
            try
            {
                DescriptionData description = GetDescription();
                int Affected = new DescriptionSystem().InsertUpdateDescription(description);
                Session["save-success"] = 1;
                string id = Request["ID"];
                Response.Redirect("Detail.aspx?ID=" + id);
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion
    }
}