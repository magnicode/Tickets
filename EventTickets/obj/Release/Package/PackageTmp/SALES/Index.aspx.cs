using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;

namespace EventTickets.Sales
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConnection DBConn = new DBConnection();
            string sql = "SELECT Distinct Category.* FROM Category,ProductMaster  WHERE Category.CategoryId = ProductMaster .CategoryId AND Category.Status = 1 ";
            sql += " UNION All SELECT Category.* FROM Category WHERE IsPhysicalInventory in (3,4) AND Status = 1";
            dC.DataSource = DBConn.ExecuteDataSet(sql, CommandType.Text);
            dC.DataBind();
            DBConn = null;
        }
    }
}