using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EventFunctions;
using EventTickets.classes;

namespace EventTickets
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (EventFunctions.Functions.GetSessionUserId() == "")
            {
                try
                {
                    Response.Redirect("/Account/Login.aspx");
                }
                catch (Exception)
                {

                    Response.Redirect("/Account/Login.aspx");
                }
            }
            if ((Request.ServerVariables["URL"]).ToLower().IndexOf("admin") > 0)
            {
                cart.Visible = false;
                tdleft.Width = "99%";
            }
            if ("" + Session["CartId"] != "")
                bSales.Attributes.Add("onclick", "return confirm('Click OK to remove everything from the cart and start over.');");
            if ((Request.ServerVariables["URL"]).ToLower().IndexOf("macys.aspx") > 0)
            {
                middiv.Style.Remove("max-height");
                middiv.Style.Add("max-height", "700px");
            }

            /*
            bAdministration.Visible = Functions.HasRights(Functions.enumPageId.Home, Functions.enumControlId.Home_Administration);
            bSales.Visible = Functions.HasRights(Functions.enumPageId.Home, Functions.enumControlId.Home_Sales);
            btnSalesReturn.Visible = Functions.HasRights(Functions.enumPageId.Home, Functions.enumControlId.Home_SalesReturn);
            bMacys.Visible = Functions.HasRights(Functions.enumPageId.Home, Functions.enumControlId.Home_Macys);
            */

            bAdministration.Visible = Users.HasRights(Users.enumRoles.BasicClerkReports) || Users.HasRights(Users.enumRoles.BasicSalesReports) || Users.HasRights(Users.enumRoles.BasicManagementReports) || Users.HasRights(Users.enumRoles.ProfitAndLossReport) || Users.HasRights(Users.enumRoles.MacysReports) || Users.HasRights(Users.enumRoles.InventoryManagement);
            bSales.Visible = Users.HasRights(Users.enumRoles.EnterOrders);
            btnSalesReturn.Visible = Users.HasRights(Users.enumRoles.EnterRefunds);
            bMacys.Visible = Users.HasRights(Users.enumRoles.EnterOrders);
        }

        protected void bLogout_Click(object sender, EventArgs e)
        {
            MYSQLDatabase.DBConnection dbCon = new MYSQLDatabase.DBConnection();
            string sql = "update UserLog t1, (select max(logindatetime) as logindatetime from UserLog) t2 set t1.logoutdatetime = now() where t1.userid = " + Session["userid"].ToString() + " and t1.logoutdatetime is null and t1.logindatetime = t2.logindatetime ";
            dbCon.Execute(sql, CommandType.Text);

            Session.Clear();
            Session.Abandon();
            HttpCookie cookie = Request.Cookies["Location"];
            if (cookie != null)
            {
                cookie["userid"] = "";                
                Response.Cookies.Remove("userid");
                Response.Cookies.Set(cookie);
            }
            Response.Redirect("/Account/Login.aspx");
        }

        protected void bSales_Click(object sender, EventArgs e)
        {

            Session["CartId"] = "";
            Session["Payment"] = null;
            Session["AmountDue"] = "";
            Session["AmountRemaining"] = "";
            Session["SalesMasterId"] = "";
            Response.Redirect("../sales/index.aspx");
        }

        protected void btnSalesReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../sales/SalesReturn.aspx");
        }
    }
}
