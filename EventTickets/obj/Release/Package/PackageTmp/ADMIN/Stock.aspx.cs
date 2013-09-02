using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.Admin
{
    public partial class Stock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection DBConn = new DBConnection();
                string sql = "SELECT CategoryId,Name FROM Category WHERE Status = 1 order by Name";
                dCategory.DataSource = DBConn.ExecuteDataSet(sql, CommandType.Text);
                dCategory.DataTextField = "Name";
                dCategory.DataValueField = "CategoryId";
                dCategory.DataBind();
                dCategory.Items.Insert(0, "<--Select Category-->");
                //dProduct.Items.Insert(0, "<--Select Product-->");

                sql = "Select LocationId, Name from LocationMaster where status = 1 Order by Name";
                dLocation.DataSource = DBConn.ExecuteDataSet(sql, CommandType.Text);
                dLocation.DataTextField = "Name";
                dLocation.DataValueField = "LocationId";
                dLocation.DataBind();
                DBConn = null;
            }
        }
    }
}