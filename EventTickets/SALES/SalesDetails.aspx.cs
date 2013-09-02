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
    public partial class SalesDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int eid = Functions.ToInt(Request.QueryString["eid"]);
            if (eid > 0)
            {
                DBConnection DBConn = new DBConnection();
                string sLocLocationId = Functions.ToInt(Functions.GetLocationId()).ToString();
                string sql = "SELECT ProductMaster .*,(SELECT TicketCount FROM ProductInventoryByLocation WHERE ProductInventoryByLocation.ProductId = ProductMaster.ProductId AND LocationId =  " + sLocLocationId + ") as Remaining FROM ProductMaster  WHERE ProductId = " + eid.ToString() + "";// AND Status = 1 ";
                DataTable DT = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    lName.Text = DT.Rows[0]["Name"].ToString();
                    tImgPrice.InnerHtml = "<img src='../images/" + DT.Rows[0]["ImageUrl"].ToString() + "' width='134' /><br>Remaining: " + DT.Rows[0]["Remaining"].ToString() + "<br>Price: $" + DT.Rows[0]["Price"].ToString() + "<br>Total: $" + (Functions.ToDecimal(DT.Rows[0]["ServiceFee"].ToString()) + Functions.ToDecimal(DT.Rows[0]["Price"].ToString())).ToString();//+ "<br>Service Fee: $" + DT.Rows[0]["ServiceFee"].ToString() 
                    tDesc.InnerHtml = DT.Rows[0]["Description"].ToString();
                }
                DBConn = null;
            }
        }

        protected void bAddToCart_Click(object sender, EventArgs e)
        {

        }
    }
}