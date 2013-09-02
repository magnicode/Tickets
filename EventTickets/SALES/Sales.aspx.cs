using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventFunctions;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.Sales
{
    public partial class Sales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int cid = Functions.ToInt(Request.QueryString["cid"]);
            if (cid > 0)
            {
                DBConnection DBConn = new DBConnection();
                string sLocLocationId = Functions.ToInt(Functions.GetLocationId()).ToString();
                string sql = "SELECT ProductMaster .*,(SELECT TicketCount FROM ProductInventoryByLocation WHERE ProductInventoryByLocation.ProductId = ProductMaster.ProductId AND LocationId =  "+ sLocLocationId +") as Remaining FROM ProductMaster  WHERE CategoryId = " + cid.ToString() + "";// AND Status = 1 ";
                dC.DataSource = DBConn.ExecuteDataSet(sql, CommandType.Text);
                dC.DataBind();
                DBConn = null;
            }
        }
    }
}