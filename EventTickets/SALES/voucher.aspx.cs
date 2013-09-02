using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;
using EventFunctions;

namespace EventTickets.SALES
{
    public partial class voucher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection DBConn = new DBConnection();
                Decimal dTotal = 0;
                DataTable DTE = DBConn.ExecuteDataSet("SELECT ShoppingCart.*,ProductPrice*Quantity as Total,Name FROM ShoppingCart,ProductMaster WHERE ShoppingCart.ProductId = ProductMaster.ProductId and CurrentCartId = " + Session["CartId"], CommandType.Text).Tables[0];
                if (DTE.Rows.Count > 0)
                {
                    dC.DataSource = DTE;
                    dC.DataBind();
                    
                    for (int i = 0; i < DTE.Rows.Count; i++)
                    {
                        dTotal += Functions.ToDecimal(DTE.Rows[i]["Total"]);
                    }
                }
                else
                {
                    dC.DataSource = null;
                    dC.DataBind();
                }
                DBConn = null;
                td_voucher_value.InnerText = "$" + Request.QueryString["amount"];    //dTotal.ToString();   single payment

                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    classes.Payment objPayment = new classes.Payment();
                    DataTable dtPayment = objPayment.GetPayment();
                    dtPayment.PrimaryKey = new DataColumn[] { dtPayment.Columns["Id"] };
                    DataRow dr = dtPayment.Rows.Find(Request.QueryString["Id"]);
                    if (dr != null)
                    {
                        tVoucherNo.Text = dr["CardNo"].ToString();
                        dr.Delete();
                    }
                    HttpContext.Current.Session["Payment"] = dtPayment;
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>"); return;
        }

        protected void OK_Click(object sender, EventArgs e)
        {
            if (tVoucherNo.Text.Trim() == "")
            {
                ErrMsg.Text = "Please enter voucher number";
                return;
            }

            DBConnection DBConn = new DBConnection();
            decimal amount = Convert.ToDecimal(Request.QueryString["amount"]);  //Convert.ToDecimal(Session["AmountDue"].ToString()); single payment

            classes.Payment payment = new classes.Payment();
            payment.CartId = Functions.ToInt(Session["CartId"].ToString());
            payment.PaymentType = "voucher";
            payment.Amount = amount;
            payment.CardType = "";
            payment.CardNo = tVoucherNo.Text.Trim().Replace("'","''");
            payment.CardExpMonth = 0;
            payment.CardExpYear = 0;
            payment.CardName = "";
            payment.SetPayment();

            if (!String.IsNullOrEmpty(Request.QueryString["from"]))
            {
                if (Request.QueryString["from"] == "a")
                    Response.Write("<script>window.opener.location.href='Authorize.aspx'; window.close();</script>");
                else if (Request.QueryString["from"] == "t")
                    Response.Write("<script>window.opener.location.href='Tickets.aspx'; window.close();</script>");
            }
            else
            {
                Response.Write("<script>window.opener.location.href='paymentoptionsNew.aspx'; window.close();</script>");
            }
            return;
        }
    }
}