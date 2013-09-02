using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventFunctions;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.Cart
{
    public partial class Cart : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AmountDue"] = 0;
            if (Functions.ToInt(Session["CartId"]) <= 0) return;
            BindGrid();
        }

        private void BindGrid()
        {
            DBConnection DBConn = new DBConnection();
            Decimal dTotal = 0;
            string sSql = "SELECT CartId,ShoppingCart.Quantity,ProductPrice*Quantity as Total,Name FROM ShoppingCart,ProductMaster WHERE ShoppingCart.ProductId = ProductMaster.ProductId and IsSmartDestination = 0 and CurrentCartId = " + Session["CartId"];
            sSql += " UNION SELECT CartId,Concat('Adult: ', ShoppingCart.Quantity,' Child: ',ShoppingCart.ChildQuantity) as Quantity,ProductPrice*Quantity + ChildProductPrice*ChildQuantity as Total,ProductName as Name FROM ShoppingCart WHERE IsSmartDestination = 1 and CurrentCartId = " + Session["CartId"];
            DataTable DTE = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
            if (DTE.Rows.Count > 0)
            {
                dC.DataSource = DTE;
                dC.DataBind();

                if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("paymentoptions.aspx") >= 0 || Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("tickets.aspx") >= 0 || Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("authorize.aspx") >= 0)
                    bPurcahse.Disabled = true;
                else
                    bPurcahse.Disabled = false;
                for (int i = 0; i < DTE.Rows.Count; i++)
                {
                    dTotal += Functions.ToDecimal(DTE.Rows[i]["Total"]);
                }
            }
            else
            {
                dC.DataSource = null;
                dC.DataBind();
                bPurcahse.Disabled = true;
            }
            Session["AmountDue"] = Math.Round((dTotal), 2);// + dTotal * Functions.ToDecimal(7.5) / Functions.ToDecimal(100)), 2);
            CartTotal.InnerHtml = "<table cellpadding='0' cellspacing='0'><tr><td align='right' style='padding:0px 10px 0px 10px'>Subtotal: </td><td align='right'>$" + dTotal.ToString() + "</td></tr><tr><td align='right' style='padding:0px 10px 0px 0px'>Tax: </td><td align='right' >0.00%</td></tr><tr><td colspan='2' style='padding:0px 0px 0px 20px;height:5px !important'><div style='border-bottom:2px solid #222;width:100%'></div></td></tr><tr><td style='font-size:16px'>Total: </td><td style='font-size:16px' align='right'>$" + Session["AmountDue"] + "</td></tr></table>";
            DBConn = null;
        }

        protected void dC_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                HiddenField cId = ((HiddenField)e.Item.FindControl("cId"));
                DBConnection DBConn = new DBConnection();
                DBConn.Execute("Delete from ShoppingCart where CartId = " + cId.Value, CommandType.Text);
                DBConn = null;
                BindGrid();
            }
        }
    }
}