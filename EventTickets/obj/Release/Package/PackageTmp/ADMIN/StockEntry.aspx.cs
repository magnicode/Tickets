using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;

namespace EventTickets.Admin
{
    public partial class StockEntry : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Form.DefaultButton = btnDisableEnter.UniqueID;
        }

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

                dLocationnp.DataSource = dLocation.DataSource;
                dLocationnp.DataTextField = "Name";
                dLocationnp.DataValueField = "LocationId";
                dLocationnp.DataBind();

                sql = "SELECT CategoryId,Name FROM Category WHERE Status = 1 and IsPhysicalInventory = 0 order by Name";
                dCategorynp.DataSource = DBConn.ExecuteDataSet(sql, CommandType.Text);                
                dCategorynp.DataTextField = "Name";
                dCategorynp.DataValueField = "CategoryId";
                dCategorynp.DataBind();
                dCategorynp.Items.Insert(0, "<--Select Category-->");

                DBConn = null;
            }
        }       
    }
}