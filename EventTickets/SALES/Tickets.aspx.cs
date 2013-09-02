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
    public partial class Tickets : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Form.DefaultButton = btnDisableEnter.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (BindSerial())
                {
                    Response.Redirect("paymentoptionsNew.aspx");
                }
            
                ((Button)payment.FindControl("bFinish")).Enabled = false;
            }
        }

        protected void bNext_Click(object sender, EventArgs e)
        {
            if (BindSerial())
            {
                Response.Redirect("paymentoptionsNew.aspx");
            }
            else
            {
                //Response.Write("<script>alert('Please enter all the required serial numbers before continuing')</script>");
            }
        }

        bool BindSerial()
        {
            bool bFinish = false;
            DBConnection DBConn = new DBConnection();
            string sql = "SELECT ShoppingCart.*,ProductMaster.Name,Category.IsPhysicalInventory FROM ShoppingCart,ProductMaster,Category WHERE ProductMaster.CategoryId = Category.CategoryId and ShoppingCart.ProductId = ProductMaster.ProductId and CurrentCartId = " + Functions.ToInt(Session["CartId"]) + " and IsSerialNoEntered = 0 limit 0,1";
            DataTable DT = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0]["IsPhysicalInventory"].ToString() == "1")
                {
                    dphy.Style.Add("display", "");
                    dnophy.Style.Add("display", "none");
                    lPname.Text = DT.Rows[0]["Name"].ToString() + " Finalization";
                    hPid.Value = DT.Rows[0]["ProductId"].ToString();
                    tSerails.InnerHtml = DT.Rows[0]["Quantity"].ToString() + " Serial Number(s) Required.";
                    int iQuantity = Functions.ToInt(DT.Rows[0]["Quantity"]);
                    tRemaining.InnerHtml = DT.Rows[0]["Quantity"].ToString() + " Remaining";

                    DT = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + DT.Rows[0]["ProductId"].ToString(), CommandType.Text).Tables[0];
                    sErialEnterd.InnerHtml = "<table>";
                    int iEntered = 0;
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        sErialEnterd.InnerHtml += "<tr><td>" + DT.Rows[i]["SerialNumber"] + "</td><td style='padding-left:5px'>Verified in inventory</td></tr>";
                        iEntered++;
                    }
                    sErialEnterd.InnerHtml += "</table>";
                    tRemaining.InnerHtml = (iQuantity - iEntered).ToString() + " Remaining";
                    if (iQuantity - iEntered > 0)
                    {
                        bNext.Enabled = false;
                    }
                    else
                    {
                        bNext.Enabled = true;
                    }
                }
                else
                {
                    hPid.Value = DT.Rows[0]["ProductId"].ToString();
                    dphy.Style.Add("display", "none");
                    dnophy.Style.Add("display", "");
                    lPname.Text = DT.Rows[0]["Name"].ToString() + " Finalization";
                    sql = "select DATE_FORMAT(expirydate,'%m/%d/%Y') as expirydate, count(*) as avilable_qty from ProductDetail where locationid = " + Session["LocationId"] + " and productid = " + DT.Rows[0]["ProductId"].ToString() + " and status in (0,3) and expirydate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' group by expirydate order by ProductDetail.expirydate";
                    DataTable dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    dlCards.DataSource = dt;
                    dlCards.DataBind();

                    tNumCards.InnerHtml = DT.Rows[0]["Quantity"].ToString() + " Card(s) Required.";
                    int iQuantity = Functions.ToInt(DT.Rows[0]["Quantity"]);
                    tNumCardsRemain.InnerHtml = DT.Rows[0]["Quantity"].ToString() + " Remaining";

                    DT = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + DT.Rows[0]["ProductId"].ToString(), CommandType.Text).Tables[0];
                    dCardsAdded.InnerHtml = "<table>";
                    int iEntered = 0;
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        dCardsAdded.InnerHtml += "<tr><td>" + DT.Rows[i]["SerialNumber"] + "</td><td style='padding-left:5px'>Verified in inventory</td></tr>";
                        iEntered++;
                    }
                    dCardsAdded.InnerHtml += "</table>";
                    tNumCardsRemain.InnerHtml = (iQuantity - iEntered).ToString() + " Remaining";
                    if (iQuantity - iEntered > 0)
                    {
                        bNext.Enabled = false;
                    }
                    else
                    {
                        bNext.Enabled = true;
                    }
                }
            }
            else
            {
                bFinish = true;
                bNext.Enabled = true;
            }
            DBConn = null;
            return bFinish;
        }
    }
}