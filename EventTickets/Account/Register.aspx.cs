using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EventTickets.classes;
using MYSQLDatabase;

namespace EventTickets.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!User.IsInRole(Convert.ToInt16(Users.enumRoles.UserCreation).ToString()))
            //{
            //    Response.Redirect("/Account/Login.aspx");
            //    return;
            //}

            if (!IsPostBack)
            {
                Users objUsers = new Users();
                DataTable dt = objUsers.GetGroupPermissions();
                lstGrpPermissions.DataSource = dt;
                lstGrpPermissions.DataTextField = "GroupName";
                lstGrpPermissions.DataValueField = "GroupId";
                lstGrpPermissions.DataBind();

                dt = objUsers.GetIndividualPermissions();
                lstIndPermissions.DataSource = dt;
                lstIndPermissions.DataTextField = "IndivPermissionName";
                lstIndPermissions.DataValueField = "IndivPermissionId";
                lstIndPermissions.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Users objUsers = new Users();
                objUsers.Username = tUsername.Text.Trim();
                objUsers.Password = tPassword.Value.Trim();
                objUsers.FirstName = tFirstName.Value.Trim();
                objUsers.LastName = tLastName.Value.Trim();
                objUsers.Address1 = tAddress1.Value.Trim();
                objUsers.Address2 = tAddress2.Value.Trim();
                objUsers.City = tCity.Value.Trim();
                objUsers.State = tState.Value.Trim();
                objUsers.Zip = tZip.Value.Trim();
                objUsers.Phone = tPhone.Value.Trim();
                objUsers.Email = tEmail.Value.Trim();

                string IndPermissions = "";
                foreach (ListItem Item in lstIndPermissions.Items)
                {
                    if (Item.Selected)
                        IndPermissions += (IndPermissions != "" ? "," : "") + Item.Value;
                }
                objUsers.IndPermissions = IndPermissions;

                string GrpPermissions = "";
                foreach (ListItem Item in lstGrpPermissions.Items)
                {
                    if (Item.Selected)
                        GrpPermissions += (GrpPermissions != "" ? "," : "") + Item.Value;
                }
                objUsers.GrpPermissions = GrpPermissions;

                if (hUserId.Value != "")
                    objUsers.UserId = Convert.ToInt32(hUserId.Value);
                int UserId = objUsers.SaveUser();
                if (UserId > 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('User created successfully');</script>");
                }
                else if (UserId == 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('User details updated successfully');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('An error occured while creating user');</script>");
                }

            }
            catch (Exception ex)
            {
                                
            }
        }

        protected void tUsername_TextChanged(object sender, EventArgs e)
        {
            DBConnection DBConn = new DBConnection();
            hUserId.Value = "";
            tFirstName.Value = "";
            tLastName.Value = "";
            for (int i = 0; i < lstIndPermissions.Items.Count; i++)
            {
                lstIndPermissions.Items[i].Selected = false;
            }
            for (int i = 0; i < lstGrpPermissions.Items.Count; i++)
            {
                lstGrpPermissions.Items[i].Selected = false;
            }

            if (classes.Users.IsUsernameExists(tUsername.Text.Trim()))
            {
                classes.Users objUsers = new classes.Users();
                DataTable dt = objUsers.GetUser(tUsername.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    hUserId.Value = dt.Rows[0]["UserId"].ToString();
                    tFirstName.Value = dt.Rows[0]["FirstName"].ToString();
                    tLastName.Value = dt.Rows[0]["LastName"].ToString();
                    if (dt.Rows[0]["IndividualPermissions"].ToString() != "")
                    {
                        string[] arrIndPermissions = dt.Rows[0]["IndividualPermissions"].ToString().Split(',');
                        for (int i = 0; i < lstIndPermissions.Items.Count; i++)
                        {
                            if (Array.IndexOf(arrIndPermissions, lstIndPermissions.Items[i].Value) >= 0)
                                lstIndPermissions.Items[i].Selected = true;
                            else
                                lstIndPermissions.Items[i].Selected = false;
                        }
                    }
                    if (dt.Rows[0]["GroupPermissions"].ToString() != "")
                    {
                        string[] arrGrpPermissions = dt.Rows[0]["GroupPermissions"].ToString().Split(',');
                        for (int i = 0; i < lstGrpPermissions.Items.Count; i++)
                        {
                            if (Array.IndexOf(arrGrpPermissions, lstGrpPermissions.Items[i].Value) >= 0)
                                lstGrpPermissions.Items[i].Selected = true;
                            else
                                lstGrpPermissions.Items[i].Selected = false;
                        }
                    }
                }
            }
            
        }

        

    }
}
